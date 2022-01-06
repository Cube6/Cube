using AutoMapper;
using Cube.Board.Application.Dtos;
using Cube.Board.Application.Configuration;
using Cube.Board.Domain;
using Cube.Board.Respository;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace Cube.Board.Application
{
	public class BoardAppService : IBoardAppService
	{
		private IBoardRepository _repository;
		private IMapper _mapper = MapperFactory.GetMapper();

		public BoardAppService(IBoardRepository repository)
		{
			_repository = repository;
		}

		public IEnumerable<BoardDto> GetAll()
		{
			var list = new List<BoardDto>();
			foreach (var item in _repository.ListAsync().Result)
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

		public async Task CreateBoard(CreateBoardDto createBoardDto)
		{
			var board = new DisscussionBoard()
			{
				Name = createBoardDto.Name,
				CreatedUser = createBoardDto.CreatedUser,
				DateCreated = DateTime.Now,
				DateModified = DateTime.Now
			};
			await _repository.CreateBoardAsync(board);
		}

		public async Task CreateBoardItem(BoardItemDto boardItemDto)
		{
			var boardItem = new DisscussionBoardItem()
			{
				Board = _repository.ListAsync().Result.Where(b => b.Id == boardItemDto.BoardId).FirstOrDefault(),
				Detail = boardItemDto.Detail,
				Action = boardItemDto.Action,
				ThumbsUp = boardItemDto.ThumbsUp,
				ThumbsDown = boardItemDto.ThumbsDown,
				DateCreated = DateTime.Now,
				DateModified = DateTime.Now,
				Type= boardItemDto.Type,
			};
			await _repository.CreateBoardItemAsync(boardItem);
		}

		public async Task UpdateBoardItem(BoardItemDto boardItemDto)
		{
			var boardItem = new DisscussionBoardItem()
			{
				Id = boardItemDto.Id,
				Detail = boardItemDto.Detail,
				Action = boardItemDto.Action,
				ThumbsUp = boardItemDto.ThumbsUp,
				ThumbsDown = boardItemDto.ThumbsDown,
				DateModified = DateTime.Now,
			};
			await _repository.UpdateBoardItemAsync(boardItem);
		}

		public async Task DeleteBoardByIdAsync(long id)
		{
			await _repository.DeleteBoardAsync(id);
		}

		public async Task DeleteBoardItemByIdAsync(long id)
		{
			await _repository.DeleteBoardItemAsync(id);
		}

		public async Task<List<BoardItemDto>> FindBoardItemByIdAsync(long boardId)
		{
			var ListBoardItemDto = await _repository.GetBoardItemByIdAsync(boardId);
			if (ListBoardItemDto == null)
			{
				return null;
			}


			var list = new List<BoardItemDto>();
			foreach (var item in ListBoardItemDto)
			{
				var boardItemDto = _mapper.Map<BoardItemDto>(item);
				list.Add(boardItemDto);
			}
			return list;
		}
	}
}
