using Board.Domain;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Board.Respository
{
	public interface IBoardRepository
	{
		Task CreateBoardAsync(DisscussionBoard disscussionBoard);
		Task<DisscussionBoardItem> GetBoardItemByIdAsync(long boardId);
		Task<List<DisscussionBoard>> ListAsync();
		Task<bool> DeleteBoardAsync(long id);
	}
}
