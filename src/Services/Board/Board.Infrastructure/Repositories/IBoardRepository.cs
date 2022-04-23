using Cube.Board.Domain;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Cube.Board.Respository
{
	public interface IBoardRepository
	{
		Task UpdateBoardAsync(DisscussionBoard disscussionBoard);
		Task<int> CreateBoardAsync(DisscussionBoard disscussionBoard);
		Task<List<DisscussionBoard>> ListAsync();
		Task<bool> DeleteBoardAsync(long id);
		Task<bool> SoftDeleteBoardAsync(long id);

		Task<DisscussionBoardItem> GetBoardItemByIdAsync(long id);
		Task<List<DisscussionBoardItem>> GetBoardItemsByBoardIdAsync(long boardId);
		Task<int> CreateBoardItemAsync(DisscussionBoardItem disscussionBoardItem);
		Task<bool> DeleteBoardItemAsync(long id);
		Task UpdateBoardItemAsync(DisscussionBoardItem disscussionBoardItem);

		Task<Comment> GetCommentByIdAsync(long id);
		Task<Comment> GetCommentByUserNameAsync(long borderItemId, string username);
		Task<List<Comment>> GetCommentsByIdAsync(long boardItemId);
		Task<List<Comment>> GetCommentsByIdAsync(long boardItemId, CommentType type);
		Task<int> CreateCommentAsync(Comment comment);
		Task<bool> DeleteCommentAsync(long id);
		Task<bool> DeleteCommentByUserNameAsync(long borderItemId, string username);
		Task UpdateCommentAsync(Comment comment);
	}
}
