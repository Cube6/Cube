using Cube.Board.Domain;
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

		public Task CreateBoardItemAsync(DisscussionBoardItem disscussionBoardItem)
		{
			_context.DisscussionBoardItems.Add(disscussionBoardItem);
			return _context.SaveChangesAsync();
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

		public async Task<List<DisscussionBoardItem>> GetBoardItemByIdAsync(long boardId)
		{
			var result = await _context.DisscussionBoardItems.Where(it => it.Board.Id == (int)boardId).ToListAsync();
			return result;
		}

		public Task<List<DisscussionBoard>> ListAsync()
		{
			return _context.DisscussionBoards.ToListAsync();
		}

		public async Task<List<Comment>> GetCommentsByIdAsync(long boardItemId)
		{
			var result = await _context.Comments.Where(it => it.BoardItem.Id == (int)boardItemId).ToListAsync();
			return result;
		}

		public Task CreateCommentAsync(Comment comment)
		{
			_context.Comments.Add(comment);
			return _context.SaveChangesAsync();
		}

		public async Task<bool> DeleteCommentAsync(long commentId)
		{
			var comment = await _context.Comments.SingleAsync(it => it.Id == commentId);
			_context.Comments.Remove(comment);
			return _context.SaveChanges() > 0;
		}
	}
}
