using Cube.Board.Application;
using Cube.Board.Application.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Board.API.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class BoardController : ControllerBase
	{
		private readonly IBoardAppService _appservice;
		private readonly ILogger<BoardController> _logger;
		public BoardController(IBoardAppService appservice, ILogger<BoardController> logger)
		{
			_appservice = appservice;
			_logger = logger;
		}

		[HttpGet("")]
		public IEnumerable<BoardDto> GetAll()
		{
			return _appservice.GetBoards();
		}

		[HttpPost]
		[Authorize]
		public Task<int> CreateBoard(string operation, CreateBoardDto ceateBoard)
		{
			return _appservice.CreateBoard(ceateBoard);
		}

		[HttpPut]
		[Authorize]
		public Task UpdateBoard(BoardDto Board)
		{
			return _appservice.UpdateBoard(Board);
		}

		[HttpDelete("{id}")]
		[Authorize]
		public async Task DeleteBoardByIdAsync(long id)
		{
			await _appservice.DeleteBoardByIdAsync(id);
		}
	}
}
