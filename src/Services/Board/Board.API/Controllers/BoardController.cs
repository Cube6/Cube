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
		public async Task CreateBoard(CreateBoardDto ceateBoard)
		{
			await _appservice.CreateBoard(ceateBoard);
		}

		[HttpDelete("{id}")]
		[Authorize]
		public async Task DeleteBoardByIdAsync(long id)
		{
			await _appservice.DeleteBoardByIdAsync(id);
		}
	}
}
