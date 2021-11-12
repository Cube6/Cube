using Board.Domain;
using System.Threading.Tasks;

namespace Board.Respository
{
	public interface IBoardRepository
	{
		Task<DisscussionBoardItem> GetBoardItemByIdAsync(long boardId);
		Task<bool> DeleteBoardAsync(long id);
	}
}
