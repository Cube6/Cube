using Cube.Board.Domain;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Cube.Board.Respository
{
	public interface IBoardRepository
	{
		Task CreateBoardAsync(DisscussionBoard disscussionBoard);
		Task<DisscussionBoardItem> GetBoardItemByIdAsync(long boardId);
		Task<List<DisscussionBoard>> ListAsync();
		Task<bool> DeleteBoardAsync(long id);
	}
}
