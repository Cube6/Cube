using Cube.Board.Application.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Cube.Board.Application
{
	public interface IBoardAppService
	{
		IEnumerable<BoardDto> GetBoards();
		IEnumerable<BoardDto> GetRemovedBoards();
		Task UpdateBoard(BoardDto disscussionBoard);
		Task<int> CreateBoard(CreateBoardDto disscussionBoard);
		Task DeleteBoardByIdAsync(long id);

		Task<BoardItemDto> CreateBoardItem(BoardItemDto boardItemDto);
		Task DeleteBoardItemByIdAsync(long id);
		Task UpdateBoardItem(BoardItemDto boardItemDto);
		Task<List<BoardItemDto>> FindBoardItemByBoardIdAsync(long id);

		Task CreateComment(CommentDto commentDto);
		Task DeleteCommentByIdAsync(long id);
		Task<List<CommentDto>> FindCommentsByIdAsync(long boardItemId);
		Task<bool> DeleteCommentAsync(long borderItemId, string username);
	}
}
