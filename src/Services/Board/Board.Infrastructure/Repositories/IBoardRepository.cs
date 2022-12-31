using Cube.Board.Domain;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Cube.Board.Respository
{
	public interface IBoardRepository
	{
		Task<List<Project>> GetProjects();

		Task UpdateBoardAsync(DisscussionBoard disscussionBoard);
		Task<int> CreateBoardAsync(DisscussionBoard disscussionBoard);
		Task<List<DisscussionBoard>> GetBoards();
		Task<bool> DeleteBoardAsync(long id);
		Task<bool> SoftDeleteBoardAsync(long id);

		Task<DisscussionBoardItem> GetBoardItemByIdAsync(long id);
		Task<List<DisscussionBoardItem>> GetBoardItemsByBoardIdAsync(long boardId);
		Task<int> CreateBoardItemAsync(DisscussionBoardItem disscussionBoardItem);
		Task<bool> DeleteBoardItemAsync(long id);
		Task UpdateBoardItemAsync(DisscussionBoardItem disscussionBoardItem);

		Task<List<Comment>> GetComments();
		Task<Comment> GetCommentByIdAsync(long id);
		Task<Comment> GetCommentByUserNameAsync(long borderItemId, string userName);
		Task<List<Comment>> GetCommentsByIdAsync(long boardItemId);
		Task<List<Comment>> GetCommentsByIdAsync(long boardItemId, CommentType type);
		Task<int> CreateCommentAsync(Comment comment);
		Task<bool> DeleteCommentAsync(long id);
		Task<bool> DeleteThumbsUpAsync(long borderItemId, string userName);
		Task UpdateCommentAsync(Comment comment);

		Task<int> CreateIntegrationEventAsync(IntegrationEvent @event);
		Task UpdateIntegrationEventAsync(IntegrationEvent @event);
		Task<IntegrationEvent> GetIntegrationEventAsync();
	}
}
