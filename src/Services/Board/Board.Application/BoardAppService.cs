using AutoMapper;
using Cube.Board.Application.Configuration;
using Cube.Board.Application.Dtos;
using Cube.Board.Application.Factory;
using Cube.Board.Application.IntegrationEvents.Events;
using Cube.Board.Domain;
using Cube.Board.Respository;
using Cube.BuildingBlocks.EventBus.Abstractions;
using Cube.Infrastructure.Redis;
using Elastic.Application.Dao;
using Elastic.Application.IntegrationEvents.Events.UserActionEvents;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BoardItemState = Cube.Board.Domain.BoardItemState;
using BoardItemType = Cube.Board.Domain.BoardItemType;
using CommentType = Cube.Board.Domain.CommentType;

namespace Cube.Board.Application
{
	public class BoardAppService : IBoardAppService
	{
		private IBoardRepository _repository;
		private IMapper _mapper = MapperFactory.GetMapper();
		private IRedisInstance _redis;
		private IEventBus _eventBus;

		public BoardAppService(IBoardRepository repository, IRedisInstance redis, IEventBus eventBus)
		{
			_repository = repository;
			_redis = redis;
			_eventBus = eventBus;
		}

		public IEnumerable<ProjectDto> GetProjects()
		{
			var projects = _repository.GetProjects().Result.Where(t => !t.IsDeleted);
			var projectDtos = projects.Select(p => _mapper.Map<ProjectDto>(p));
			return projectDtos;
		}

		public IEnumerable<BoardDto> GetBoards(int projectId, BoardType type)
		{
			var list = new List<BoardDto>();
			IEnumerable<DisscussionBoard> boards = new List<DisscussionBoard>();
			if (type == BoardType.All)
			{
				boards = _repository.GetBoards().Result.Where(t => !t.IsDeleted);
			}
			else if (type == BoardType.InProgress)
			{
				boards = _repository.GetBoards().Result.Where(t => !t.IsDeleted && t.State == BoardState.InProgress);
			}
			else if (type == BoardType.Completed)
			{
				boards = _repository.GetBoards().Result.Where(t => !t.IsDeleted && t.State == BoardState.Completed);
			}
			else if (type == BoardType.Deleted)
			{
				boards = _repository.GetBoards().Result.Where(t => t.IsDeleted);
			}

			foreach (var item in boards.Where(t => t.Project?.Id == projectId))
			{
				var boardDto = _mapper.Map<BoardDto>(item);
				list.Add(boardDto);
			}
			return list;
		}

		public IEnumerable<BoardDto> GetRemovedBoards()
		{
			var list = new List<BoardDto>();
			foreach (var item in _repository.GetBoards().Result.Where(t => t.IsDeleted))
			{
				var boardDto = _mapper.Map<BoardDto>(item);
				list.Add(boardDto);
			}
			return list;
		}

		public BoardDto GetDetail(long boardId)
		{
			var DisscussionBoard = _repository.GetBoards().Result.Where(b => b.Id == boardId).FirstOrDefault();
			return _mapper.Map<BoardDto>(DisscussionBoard);
		}

		public Task<int> CreateBoardAsync(CreateBoardDto createBoardDto)
		{
			var board = new DisscussionBoard()
			{
				Name = createBoardDto.Name,
				CreatedUser = createBoardDto.CreatedUser,
				State = BoardState.InProgress,
				IsDeleted = false,
				DateCreated = DateTime.Now,
				DateModified = DateTime.Now,
				Project = _repository.GetProjects().Result.Where(b => b.Id == createBoardDto.ProjectId).FirstOrDefault(),
			};

			var result =  _repository.CreateBoardAsync(board);

			_eventBus.Publish(new CreateBoardActionEvent(new BoardDao
			{
				CreateUser = board.CreatedUser,
				State = (int)board.State,
				CreationDate = board.DateCreated,
				EntityId = board.Id,
				Creator = board.CreatedUser,
				IsDeleted = board.IsDeleted,
				Keyword = board.Name
			}));

			return result;
		}

		public async Task UpdateBoardAsync(BoardDto boardDto)
		{
			var board = _repository.GetBoards().Result.Where(b => b.Id == boardDto.Id).FirstOrDefault();
			board.Name = boardDto.Name;
			board.DateModified = DateTime.Now;
			if (boardDto.State != BoardState.None)
			{
				board.State = boardDto.State;
			}

			await _repository.UpdateBoardAsync(board);

			_eventBus.Publish(new UpdateBoardActionEvent(new BoardDao
			{
				CreateUser = board.CreatedUser,
				State = (int)board.State,
				CreationDate = board.DateCreated,
				EntityId = board.Id,
				Creator = board.CreatedUser,
				IsDeleted = board.IsDeleted,
				Keyword = board.Name
			}));
		}

		public async Task<BoardItemDto> CreateBoardItemAsync(BoardItemDto boardItemDto)
		{
			var boardItem = new DisscussionBoardItem()
			{
				Board = _repository.GetBoards().Result.Where(b => b.Id == boardItemDto.BoardId).FirstOrDefault(),
				State = BoardItemState.InProgress,
				Detail = boardItemDto.Detail,
				Action = boardItemDto.Action,
				CreatedUser = boardItemDto.CreatedUser,
				DateCreated = DateTime.Now,
				DateModified = DateTime.Now,
				Type = boardItemDto.Type,
				AssociatedBoardItemId = boardItemDto.AssociatedBoardItemId,
				Assignee = boardItemDto.Assignee
			};
			var id = await _repository.CreateBoardItemAsync(boardItem);
			boardItem.Id = id;

			_eventBus.Publish(new CreateBoardItemActionEvent(new BoardItemDao
			{
				CreateUser = boardItem.CreatedUser,
				CreationDate = boardItem.DateCreated,
				EntityId = boardItem.Id,
				Creator = boardItem.CreatedUser,
				Keyword = boardItem.Detail,
				BoardId = boardItem.Board.Id,
				Type = (int)boardItem.Type,
				State = (int)boardItem.State,
				Action = boardItem.Action,
			}));

			var result = _mapper.Map<BoardItemDto>(boardItem);
			return result;
		}

		public async Task UpdateBoardItemAsync(BoardItemDto boardItemDto)
		{
			var boardItem = await _repository.GetBoardItemByIdAsync(boardItemDto.Id);
			boardItem.Detail = boardItemDto.Detail;
			boardItem.Action = boardItemDto.Action;
			boardItem.Assignee = boardItemDto.Assignee;
			boardItem.DateModified = DateTime.Now;
			if (boardItemDto.State != BoardItemState.None)
			{
				boardItem.State = boardItemDto.State;
			}

			await _repository.UpdateBoardItemAsync(boardItem);

			_eventBus.Publish(new UpdateBoardItemActionEvent(new BoardItemDao
			{
				CreateUser = boardItem.CreatedUser,
				CreationDate = boardItem.DateCreated,
				EntityId = boardItem.Id,
				Creator = boardItem.CreatedUser,
				Keyword = boardItem.Detail,
				BoardId = boardItem.Board.Id,
				Type = (int)boardItem.Type,
				State = (int)boardItem.State,
				Action = boardItem.Action,
			}));
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
				boardItemDto.Messages = comments.Where(t => t.Type == CommentType.Message);

				list.Add(boardItemDto);
			}
			return list;
		}

		public async Task<int> CreateCommentAsync(CommentDto commentDto)
		{
			if (commentDto.Type == CommentType.ThumbsUp)
			{
				var ownerBoardItem = _repository.GetBoardItemByIdAsync(commentDto.BoardItemId).Result;

				if (!await _redis.SetContainsValueAsync(ownerBoardItem.Id, commentDto.CreatedUser))
				{
					await _repository.CreateIntegrationEventAsync(
										IntegrationEventModelCreator.Create(
											new CommentAddedEvent(commentDto)));
					await _redis.SetAddAsync(ownerBoardItem.Id, commentDto.CreatedUser, CacheSettings.DefaultExpiryInSecondsForComments);
					_eventBus.Publish(new CreateThumbUpActionEvent(new CommentDao
					{
						Creator = commentDto.CreatedUser,
						Type = (int)commentDto.Type,
						EntityId = commentDto.BoardItemId
					}));
				}
			}
			else if (commentDto.Type == CommentType.Message)
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

				var result = await _repository.CreateCommentAsync(comment);

				_eventBus.Publish(new CreateCommentActionEvent(new CommentDao
				{
					Creator = comment.CreatedUser,
					CreationDate = comment.DateCreated,
					EntityId = comment.Id,
					Keyword = comment.Detail,
					BoardItemId = comment.BoardItem.Id,
					Type = (int)comment.Type,
				}));

				return result;
			}
			else
			{
				throw new NotSupportedException($"{commentDto.Type} is not supported");
			}

			return -1;
		}

		public async Task DeleteCommentAsync(long boardItemId, string username)
		{
			await _repository.CreateIntegrationEventAsync(
								IntegrationEventModelCreator.Create(
									new ThumbUpDeletedEvent(boardItemId, username)));
			await _redis.SetRemoveAsync(boardItemId, username);
			_eventBus.Publish(new DeleteThumbUpActionEvent(username, boardItemId));
		}

		public async Task DeleteCommentAsync(long commentId)
		{
			await _repository.CreateIntegrationEventAsync(
								IntegrationEventModelCreator.Create(
									new CommentDeletedEvent(commentId)));
			_eventBus.Publish(new DeleteCommentActionEvent("", commentId));
		}

		public async Task<List<CommentDto>> FindCommentsByIdAsync(long boardItemId)
		{
			List<Comment> comments = new List<Comment>();
			var userNames = await _redis.SetAllAsync<long, string>(boardItemId);
			if (userNames.Any())
			{
				var boardItem = await _repository.GetBoardItemByIdAsync(boardItemId);
				foreach (var userName in userNames)
				{
					comments.Add(new Comment() { CreatedUser = userName, Type = CommentType.ThumbsUp, BoardItem = boardItem });
				}

				var messageComments = await _repository.GetCommentsByIdAsync(boardItemId, CommentType.Message);
				comments.AddRange(messageComments);
			}
			else
			{
				comments = await _repository.GetCommentsByIdAsync(boardItemId);
				foreach (var comment in comments.Where(c => c.Type == CommentType.ThumbsUp))
				{
					await _redis.SetAddAsync(comment.BoardItem.Id, comment.CreatedUser, CacheSettings.DefaultExpiryInSecondsForComments);
				}
			}

			var list = new List<CommentDto>();
			foreach (var item in comments)
			{
				var boardItemDto = _mapper.Map<CommentDto>(item);
				list.Add(boardItemDto);
			}
			return list;
		}

		public async Task UpdateCommentAsync(CommentDto commentDto)
		{
			await _repository.CreateIntegrationEventAsync(
								IntegrationEventModelCreator.Create(
									new CommentUpdatedEvent(commentDto.Id, commentDto.Detail)));

			_eventBus.Publish(new UpdateCommentActionEvent(new CommentDao
			{
				Creator = commentDto.CreatedUser,
				CreationDate = commentDto.DateCreated,
				EntityId = commentDto.Id,
				Keyword = commentDto.Detail,
				BoardItemId = commentDto.BoardItemId,
				Type = (int)commentDto.Type,
			}));
		}

		public IEnumerable<BoardItemStatsDto> GetBoardItemStats()
		{
			var dict = new Dictionary<string, BoardItemStatsDto>();

			IEnumerable<DisscussionBoard> boards = _repository.GetBoards().Result.Where(t => !t.IsDeleted);
			foreach (var board in boards)
			{
				var boardItems = _repository.GetBoardItemsByBoardIdAsync(board.Id).Result;

				foreach (var item in boardItems.GroupBy(d => d.CreatedUser))
				{
					if (!dict.ContainsKey(item.Key))
					{
						dict[item.Key] = new BoardItemStatsDto()
						{
							CreatedUser = item.Key,
						};
					}

					foreach (var boardItem in item.GroupBy(t => t.Type))
					{
						if (boardItem.Key == BoardItemType.WentWell)
						{
							dict[item.Key].CountOfWell += boardItem.Count();
						}
						else if (boardItem.Key == BoardItemType.NeedsImproved)
						{
							dict[item.Key].CountOfImproved += boardItem.Count();
						}
					}
				}
			}

			var comments = _repository.GetComments().Result;
			foreach (var item in comments.GroupBy(t => t.CreatedUser))
			{
				if (!dict.ContainsKey(item.Key))
				{
					dict[item.Key] = new BoardItemStatsDto()
					{
						CreatedUser = item.Key,
					};
				}

				foreach (var comment in item.GroupBy(t => t.Type))
				{
					if (comment.Key == CommentType.ThumbsUp)
					{
						dict[item.Key].CountOfThumbsup += comment.Count();
					}
					else if (comment.Key == CommentType.Message)
					{
						dict[item.Key].CountOfComments += comment.Count();
					}
				}
			}

			return dict.Values.ToList().OrderByDescending(t => t.Count);
		}
	}
}
