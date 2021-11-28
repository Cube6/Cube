using Cube.Board.Application.Dtos;
using Cube.Board.Domain;
using Cube.Board.Respository;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Cube.Board.Application
{
	public class BoardAppService : IBoardAppService
	{
		private IBoardRepository _repository = new BoardRepository();

		public IEnumerable<BoardDto> GetAll()
		{
			var list = new List<BoardDto>();
			foreach (var item in _repository.ListAsync().Result)
			{
				list.Add(new BoardDto()
				{
					Id = item.Id,
					Name = item.Name,
					CreatedUser = item.CreatedUser,
					DateCreated = item.DateCreated,
					DateModified = item.DateModified,
				});
			}
			return list;
		}

		public async Task CreateBoard(CreateBoardDto createBoardDto)
		{
			var board = new DisscussionBoard()
			{
				Name = createBoardDto.Name,
				CreatedUser = createBoardDto.CreatedUser,
				DateCreated = DateTime.Now,
				DateModified = DateTime.Now
			};
			await _repository.CreateBoardAsync(board);
		}

		public async Task DeleteBoardByIdAsync(long id)
		{
			await _repository.DeleteBoardAsync(id);
		}

		public async Task<BoardItemDto> FindBoardItemByIdAsync(long id)
		{
			var item = await _repository.GetBoardItemByIdAsync(id);
			if (item == null)
			{
				return null;
			}
			return new BoardItemDto()
			{
				Id = item.Id,
				Detail = item.Detail,
				DateCreated = item.DateCreated,
				DateModified = item.DateModified,
			};
		}
	}
}
