using Cube.Board.Application;
using Cube.Board.Application.Dtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
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
		public IEnumerable<BoardItemDto> Find(long id)
		{
			var ss = _appservice.FindBoardItemByIdAsync(id);
			var result= ss.Result;
			return result;
		}
	}
}
