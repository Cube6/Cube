using Cube.Board.Domain;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Cube.Board.Respository
{
	public interface IBoardRepository
	{
		Task CreateBoardAsync(DisscussionBoard disscussionBoard);
		Task UpdateBoardAsync(DisscussionBoard disscussionBoard);
		Task<List<DisscussionBoard>> ListAsync();
		Task<bool> DeleteBoardAsync(long id);
		Task<bool> SoftDeleteBoardAsync(long id);

		Task<List<DisscussionBoardItem>> GetBoardItemByIdAsync(long boardId);
		Task CreateBoardItemAsync(DisscussionBoardItem disscussionBoardItem);
		Task<bool> DeleteBoardItemAsync(long id);
		Task UpdateBoardItemAsync(DisscussionBoardItem disscussionBoardItem);

		Task<List<Comment>> GetCommentsByIdAsync(long boardItemId);
		Task CreateCommentAsync(Comment comment);
		Task<bool> DeleteCommentAsync(long id);
	}
}
