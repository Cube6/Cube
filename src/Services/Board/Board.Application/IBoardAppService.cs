using Cube.Board.Application.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Cube.Board.Application
{
	public interface IBoardAppService
	{
		IEnumerable<ProjectDto> GetProjects();
		IEnumerable<BoardDto> GetBoards(int projectId, BoardType type);
		IEnumerable<BoardDto> GetRemovedBoards();
		Task UpdateBoardAsync(BoardDto disscussionBoard);
		Task<int> CreateBoardAsync(CreateBoardDto disscussionBoard);
		Task DeleteBoardByIdAsync(long id);

		Task<BoardItemDto> CreateBoardItemAsync(BoardItemDto boardItemDto);
		Task DeleteBoardItemByIdAsync(long id);
		Task UpdateBoardItemAsync(BoardItemDto boardItemDto);
		Task<List<BoardItemDto>> FindBoardItemByBoardIdAsync(long id);

		Task<int> CreateCommentAsync(CommentDto commentDto);
		Task UpdateCommentAsync(CommentDto commentDto);
		Task DeleteCommentAsync(long borderItemId, string userName);
		Task DeleteCommentAsync(long commentId);
		Task<List<CommentDto>> FindCommentsByIdAsync(long boardItemId);

		IEnumerable<BoardItemStatsDto> GetBoardItemStats();
	}
}
