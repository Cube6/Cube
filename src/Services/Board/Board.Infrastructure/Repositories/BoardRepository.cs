﻿using Cube.Board.Domain;
using Cube.Infrastructure.Repository;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cube.Board.Respository
{
	public class BoardRepository : Repository, IBoardRepository
	{
		private BoardContext _context;

		public BoardRepository(BoardContext context)
		{
			_context = context;
			_context.Database.EnsureCreated();
		}

		public async Task<int> CreateBoardAsync(DisscussionBoard disscussionBoard)
		{
			_context.DisscussionBoards.Add(disscussionBoard);
			await _context.SaveChangesAsync();
			return disscussionBoard.Id;
		}

		public async Task<int> CreateBoardItemAsync(DisscussionBoardItem disscussionBoardItem)
		{
			_context.DisscussionBoardItems.Add(disscussionBoardItem);
			await _context.SaveChangesAsync();
			return disscussionBoardItem.Id;
		}

		public Task UpdateBoardAsync(DisscussionBoard disscussionBoard)
		{
			_context.DisscussionBoards.Update(disscussionBoard);
			return _context.SaveChangesAsync();
		}

		public Task UpdateBoardItemAsync(DisscussionBoardItem disscussionBoardItem)
		{
			_context.DisscussionBoardItems.Update(disscussionBoardItem);
			return _context.SaveChangesAsync();
		}

		public async Task<bool> DeleteBoardAsync(long boardId)
		{
			var board = await _context.DisscussionBoards.SingleAsync(it => it.Id == boardId);
			_context.DisscussionBoards.Remove(board);
			return _context.SaveChanges() > 0;
		}

		public async Task<bool> SoftDeleteBoardAsync(long boardId)
		{
			var board = await _context.DisscussionBoards.SingleAsync(it => it.Id == boardId);
			board.IsDeleted = true;
			_context.DisscussionBoards.Update(board);
			return _context.SaveChanges() > 0;
		}

		public async Task<bool> DeleteBoardItemAsync(long boardItemId)
		{
			var boardItem = await _context.DisscussionBoardItems.SingleAsync(it => it.Id == boardItemId);
			_context.DisscussionBoardItems.Remove(boardItem);
			return _context.SaveChanges() > 0;
		}

		public async Task<DisscussionBoardItem> GetBoardItemByIdAsync(long id)
		{
			var result = await _context.DisscussionBoardItems.Include(t => t.Board).SingleAsync(it => it.Id == (int)id);
			return result;
		}

		public async Task<List<DisscussionBoardItem>> GetBoardItemsByBoardIdAsync(long boardId)
		{
			var result = await _context.DisscussionBoardItems.Include(t=>t.Board)
																.Where(it => it.Board.Id == (int)boardId)
																.OrderByDescending(t=>t.Id)
																.ToListAsync();
			return result;
		}

		public Task<List<DisscussionBoard>> GetBoards()
		{
			return _context.DisscussionBoards.Include(t=>t.Project).Include(t=>t.BoardItems.Where(i=>i.Type== BoardItemType.Action)).OrderByDescending(t=>t.DateCreated).ToListAsync();
		}

		public Task<List<Project>> GetProjects()
		{
			return _context.Projects.OrderByDescending(t => t.DateCreated).ToListAsync();
		}

		public async Task<List<Comment>> GetComments()
		{
			var result = await _context.Comments.ToListAsync();
			return result;
		}

		public async Task<List<Comment>> GetCommentsByIdAsync(long boardItemId)
		{
			var result = await _context.Comments.Where(it => it.BoardItem.Id == (int)boardItemId).ToListAsync();
			return result;
		}

		public async Task<List<Comment>> GetCommentsByIdAsync(long boardItemId, CommentType type)
		{
			var result = await _context.Comments.Where(it => it.BoardItem.Id == (int)boardItemId && it.Type == type).ToListAsync();
			return result;
		}

		public async Task<Comment> GetCommentByIdAsync(long id)
		{
			var result = await _context.Comments.Include(t => t.BoardItem).SingleAsync(it => it.Id == (int)id);
			return result;
		}

		public async Task<Comment> GetCommentByUserNameAsync(long borderItemId, string username)
		{
			var result = await _context.Comments.Include(t => t.BoardItem).SingleOrDefaultAsync(c => c.BoardItem.Id == borderItemId && c.CreatedUser == username);
			return result;
		}

		public async Task<int> CreateCommentAsync(Comment comment)
		{
			_context.Comments.Add(comment);
			await _context.SaveChangesAsync();
			return comment.Id;
		}

		public async Task<bool> DeleteCommentAsync(long commentId)
		{
			var comment = await _context.Comments.SingleOrDefaultAsync(it => it.Id == commentId);
			if (comment != null)
			{
				_context.Comments.Remove(comment);
				return _context.SaveChanges() > 0;
			}

			return true;
		}

		public async Task<bool> DeleteThumbsUpAsync(long borderItemId, string username)
		{
			var comment = await _context.Comments
				.SingleOrDefaultAsync(c => c.BoardItem.Id == borderItemId &&
											c.CreatedUser == username &&
											c.Type == CommentType.ThumbsUp);
			if (comment != null)
			{
				_context.Comments.Remove(comment);
				return _context.SaveChanges() > 0;
			}

			return true;
		}

		public Task UpdateCommentAsync(Comment comment)
		{
			_context.Comments.Update(comment);
			return _context.SaveChangesAsync();
		}

		public async Task<int> CreateIntegrationEventAsync(IntegrationEvent @event)
		{
			_context.IntegrationEvents.Add(@event);
			await _context.SaveChangesAsync();
			return @event.Id;
		}

		public Task UpdateIntegrationEventAsync(IntegrationEvent @event)
		{
			_context.IntegrationEvents.Update(@event);
			return _context.SaveChangesAsync();
		}

		public async Task<IntegrationEvent> GetIntegrationEventAsync()
		{
			var result = await _context.IntegrationEvents.Where(e => !e.Published)
															.OrderBy(e => e.DateCreated)
															.FirstOrDefaultAsync();
			return result;
		}
	}
}
