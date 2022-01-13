using Cube.Board.Application.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Cube.Board.Application
{
	public interface IBoardAppService
	{
		Task CreateBoard(CreateBoardDto disscussionBoard);
		Task CreateBoardItem(BoardItemDto boardItemDto);

		Task DeleteBoardByIdAsync(long id);
		Task DeleteBoardItemByIdAsync(long id);

		Task UpdateBoardItem(BoardItemDto boardItemDto);

		IEnumerable<BoardDto> GetAll();
		Task<List<BoardItemDto>> FindBoardItemByIdAsync(long id);
	}
}
