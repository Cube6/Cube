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

		[HttpGet]
		public IEnumerable<BoardDto> GetAll()
		{
			return _appservice.GetAll();
		}

		[HttpPost]
		public async Task CreateBoard(CreateBoardDto ceateBoard)
		{
			await _appservice.CreateBoard(ceateBoard);
		}


		[HttpPost]
		[Route("CreateBoardItem")]
		public async Task CreateBoardItem(BoardItemDto boardItemDto)
		{
			await _appservice.CreateBoardItem(boardItemDto);
		}


		[HttpDelete("{id}")]
		public async Task DeleteBoardByIdAsync(long id)
		{
			await _appservice.DeleteBoardByIdAsync(id);
		}

		[HttpGet("{id}")]
		[Route("FindBoardItemById")]
		public async Task<BoardItemDto> FindBoardItemByIdAsync(long id)
		{
			return await _appservice.FindBoardItemByIdAsync(id);
		}

		[Authorize]
		[HttpGet]
		[Route("SecureAction")]
		public IActionResult SecureAction()
		{
			return Ok("SecureAction");
		}
	}
}
