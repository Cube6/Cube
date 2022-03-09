using AutoMapper;
using Cube.Board.Application.Dtos;
using Cube.Board.Application.Configuration;
using Cube.Board.Domain;
using Cube.Board.Respository;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using RedisPractice;

namespace Cube.Board.Application
{
	public class BoardAppService : IBoardAppService
	{
		private IBoardRepository _repository;
		private IMapper _mapper = MapperFactory.GetMapper();
		private IRedisInstance _redis;

		public BoardAppService(IBoardRepository repository, IRedisInstance redis)
		{
			_repository = repository;
			_redis = redis;
		}

		public IEnumerable<BoardDto> GetBoards()
		{
			var list = new List<BoardDto>();
			foreach (var item in _repository.ListAsync().Result.Where(t=>!t.IsDeleted))
			{
				var boardDto = _mapper.Map<BoardDto>(item);
				list.Add(boardDto);
			}
			return list;
		}

		public IEnumerable<BoardDto> GetRemovedBoards()
		{
			var list = new List<BoardDto>();
			foreach (var item in _repository.ListAsync().Result.Where(t => t.IsDeleted))
			{
				var boardDto = _mapper.Map<BoardDto>(item);
				list.Add(boardDto);
			}
			return list;
		}

		public BoardDto GetDetail(long boardId)
		{
			var DisscussionBoard = _repository.ListAsync().Result.Where(b=>b.Id==boardId).FirstOrDefault();
			return _mapper.Map<BoardDto>(DisscussionBoard);
		}

		public Task<int> CreateBoard(CreateBoardDto createBoardDto)
		{
			var board = new DisscussionBoard()
			{
				Name = createBoardDto.Name,
				CreatedUser = createBoardDto.CreatedUser,
				State = BoardState.InProgress,
				IsDeleted = false,
				DateCreated = DateTime.Now,
				DateModified = DateTime.Now
			};
			return _repository.CreateBoardAsync(board);
		}

		public async Task UpdateBoard(BoardDto boardDto)
		{
			var board = new DisscussionBoard()
			{
				Id = boardDto.Id,
				Name = boardDto.Name,
				CreatedUser = boardDto.CreatedUser,
				State = boardDto.State,
				DateModified = DateTime.Now
			};
			await _repository.UpdateBoardAsync(board);
		}

		public async Task<BoardItemDto> CreateBoardItem(BoardItemDto boardItemDto)
		{
			var boardItem = new DisscussionBoardItem()
			{
				Board = _repository.ListAsync().Result.Where(b => b.Id == boardItemDto.BoardId).FirstOrDefault(),
				Detail = boardItemDto.Detail,
				Action = boardItemDto.Action,
				CreatedUser = boardItemDto.CreatedUser,
				DateCreated = DateTime.Now,
				DateModified = DateTime.Now,
				Type= boardItemDto.Type,
			};
			var id = await _repository.CreateBoardItemAsync(boardItem);
			boardItem.Id = id;

			return _mapper.Map<BoardItemDto>(boardItem);
		}

		public async Task UpdateBoardItem(BoardItemDto boardItemDto)
		{
			var boardItem = new DisscussionBoardItem()
			{
				Id = boardItemDto.Id,
				Detail = boardItemDto.Detail,
				CreatedUser = boardItemDto.CreatedUser,
				Action = boardItemDto.Action,
				DateModified = DateTime.Now,
				Type= boardItemDto.Type,
			};
			await _repository.UpdateBoardItemAsync(boardItem);
		}

		public async Task DeleteBoardByIdAsync(long id)
		{
			await _repository.SoftDeleteBoardAsync(id);
		}

		public async Task DeleteBoardItemByIdAsync(long id)
		{
			await _repository.DeleteBoardItemAsync(id);
		}

		public async Task<List<BoardItemDto>> FindBoardItemByBoardIdAsync(long boardId)
		{
			var ListBoardItemDto = await _repository.GetBoardItemsByBoardIdAsync(boardId);
			if (ListBoardItemDto == null)
			{
				return null;
			}

			var list = new List<BoardItemDto>();
			foreach (var item in ListBoardItemDto)
			{
				var boardItemDto = _mapper.Map<BoardItemDto>(item);
				var comments = await FindCommentsByIdAsync(boardItemDto.Id);

				boardItemDto.ThumbsUp = comments.Where(t => t.Type == CommentType.ThumbsUp);
				list.Add(boardItemDto);
			}
			return list;
		}

		public async Task CreateComment(CommentDto commentDto)
		{
			var comment = new Comment()
			{
				BoardItem = _repository.GetBoardItemByIdAsync(commentDto.BoardItemId).Result,
				Detail = commentDto.Detail,
				CreatedUser = commentDto.CreatedUser,
				DateCreated = DateTime.Now,
				DateModified = DateTime.Now,
				Type = commentDto.Type,
			};
			if(!await _redis.SetContainsValueAsync(comment.BoardItem.Id, comment.CreatedUser))
			{
				await _repository.CreateCommentAsync(comment);
				await _redis.SetAddAsync(comment.BoardItem.Id, comment.CreatedUser);
			}
		}

		public async Task DeleteCommentByIdAsync(long id)
		{
			await _repository.DeleteBoardItemAsync(id);
		}

		public async Task DeleteCommentAsync(long borderItemId, string username)
		{
			await _repository.DeleteCommentByUserNameAsync(borderItemId, username);
			await _redis.SetRemoveAsync(borderItemId, username);
		}
		public async Task<List<CommentDto>> FindCommentsByIdAsync(long boardItemId)
		{
			List<Comment> comments = new List<Comment>();
			var userNames = await _redis.ListRangeAsync<long, string>(boardItemId, 0, -1);
			if (userNames.Any())
			{
				var boardItem =await _repository.GetBoardItemByIdAsync(boardItemId);
				foreach(var userName in userNames)
				{
					comments.Add(new Comment() { CreatedUser = userName, BoardItem = boardItem });
				}
			}
			else
			{
				comments = await _repository.GetCommentsByIdAsync(boardItemId);
			}

			var list = new List<CommentDto>();
			foreach (var item in comments)
			{
				var boardItemDto = _mapper.Map<CommentDto>(item);
				list.Add(boardItemDto);
			}
			return list;
		}
	}
}
