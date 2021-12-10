using Cube.Board.Application.Dtos;
using Cube.Board.Domain;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Cube.Board.Application
{
	public interface IBoardAppService
	{
		Task CreateBoard(CreateBoardDto disscussionBoard);
		Task CreateBoardItem(BoardItemDto boardItemDto);
		Task DeleteBoardByIdAsync(long id);
		IEnumerable<BoardDto> GetAll();

		Task<BoardItemDto> FindBoardItemByIdAsync(long id);
	}
}
