using Cube.Board.Domain;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Cube.Board.Respository
{
	public interface IBoardRepository
	{
		Task<int> CreateBoardAsync(DisscussionBoard disscussionBoard);
		Task<List<DisscussionBoard>> ListAsync();
		Task<bool> DeleteBoardAsync(long id);

		Task<List<DisscussionBoardItem>> GetBoardItemByIdAsync(long boardId);
		Task CreateBoardItemAsync(DisscussionBoardItem disscussionBoardItem);
		Task<bool> DeleteBoardItemAsync(long id);
		Task UpdateBoardItemAsync(DisscussionBoardItem disscussionBoardItem);

		Task<List<Comment>> GetCommentsByIdAsync(long boardItemId);
		Task CreateCommentAsync(Comment comment);
		Task<bool> DeleteCommentAsync(long id);
	}
}
