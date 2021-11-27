using Cube.Board.Domain;
using Cube.Infrastructure.Repository;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Cube.Board.Respository
{
	public class BoardRepository : Repository, IBoardRepository
	{
		BoardContext _context = new BoardContext();

		public BoardRepository()
		{
			_context.Database.EnsureCreated();
		}

		public Task CreateBoardAsync(DisscussionBoard disscussionBoard)
		{
			_context.DisscussionBoards.Add(disscussionBoard);
			return _context.SaveChangesAsync();
		}

		public async Task<bool> DeleteBoardAsync(long id)
		{
			var board = await _context.DisscussionBoards.SingleAsync(it => it.Id == id);
			_context.DisscussionBoards.Remove(board);
			_context.SaveChanges();
			return _context.SaveChanges() > 0;
		}


		public async Task<DisscussionBoardItem> GetBoardItemByIdAsync(long id)
		{
			var result = await _context.DisscussionBoardItems.FirstAsync(it => it.Id == id);
			return result;
		}

		public Task<List<DisscussionBoard>> ListAsync()
		{
			return _context.DisscussionBoards.ToListAsync();
		}
	}
}
