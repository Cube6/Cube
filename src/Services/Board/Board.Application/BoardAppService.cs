using AutoMapper;
using Cube.Board.Application.Dtos;
using Cube.Board.Application.Configuration;
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
		private IMapper _mapper = MapperFactory.GetMapper();

		public BoardAppService()
		{
		}

		public IEnumerable<BoardDto> GetAll()
		{
			var list = new List<BoardDto>();
			foreach (var item in _repository.ListAsync().Result)
			{
				var boardDto = _mapper.Map<BoardDto>(item);
				list.Add(boardDto);
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

			return _mapper.Map<BoardItemDto>(item);
		}
	}
}
