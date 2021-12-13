using Cube.Board.Domain;
using Cube.Infrastructure.Repository;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Cube.Board.Respository
{
	public class BoardRepository : Repository, IBoardRepository
	{
		private BoardContext _context = new BoardContext();

		public BoardRepository()
		{
			_context.Database.EnsureCreated();
		}

		public Task CreateBoardAsync(DisscussionBoard disscussionBoard)
		{
			_context.DisscussionBoards.Add(disscussionBoard);
			return _context.SaveChangesAsync();
		}

		public Task CreateBoardItemAsync(DisscussionBoardItem disscussionBoardItem)
		{
			_context.DisscussionBoardItems.Add(disscussionBoardItem);
			return _context.SaveChangesAsync();
		}

		public async Task<bool> DeleteBoardAsync(long boardId)
		{
			var board = await _context.DisscussionBoards.SingleAsync(it => it.Id == boardId);
			_context.DisscussionBoards.Remove(board);
			return _context.SaveChanges() > 0;
		}

		public async Task<DisscussionBoardItem> GetBoardItemByIdAsync(long boardId)
		{
			var result = await _context.DisscussionBoardItems.FirstAsync(it => it.Board.Id == boardId);
			return result;
		}

		public Task<List<DisscussionBoard>> ListAsync()
		{
			return _context.DisscussionBoards.ToListAsync();
		}
	}
}
