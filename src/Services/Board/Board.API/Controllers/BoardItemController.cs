using Cube.Board.Application;
using Cube.Board.Application.Dtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace Board.API.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class BoardItemController : ControllerBase
	{
		private readonly IBoardAppService _appservice;
		private readonly ILogger<BoardController> _logger;
		public BoardItemController(IBoardAppService appservice, ILogger<BoardController> logger)
		{
			_appservice = appservice;
			_logger = logger;
		}

		[HttpPost]
		public async Task Create(BoardItemDto boardItemDto)
		{
			await _appservice.CreateBoardItem(boardItemDto);
		}

		[HttpGet("{id}")]
		public async Task<BoardItemDto> Find(long id)
		{
			return await _appservice.FindBoardItemByIdAsync(id);
		}
	}
}
