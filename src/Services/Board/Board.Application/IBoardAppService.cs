using Cube.Board.Application.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Cube.Board.Application
{
	public interface IBoardAppService
	{
		IEnumerable<BoardDto> GetBoards();
		IEnumerable<BoardDto> GetRemovedBoards();
		Task CreateBoard(CreateBoardDto disscussionBoard);
		Task UpdateBoard(BoardDto disscussionBoard);
		Task DeleteBoardByIdAsync(long id);

		Task CreateBoardItem(BoardItemDto boardItemDto);
		Task DeleteBoardItemByIdAsync(long id);
		Task UpdateBoardItem(BoardItemDto boardItemDto);
		Task<List<BoardItemDto>> FindBoardItemByIdAsync(long id);

		Task CreateComment(CommentDto commentDto);
		Task DeleteCommentByIdAsync(long id);
		Task<List<CommentDto>> FindCommentsByIdAsync(long boardItemId);
	}
}
