using Cube.Board.Application;
using Cube.Board.Application.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Board.API.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class CommentController : ControllerBase
	{
		private readonly IBoardAppService _appservice;
		private readonly ILogger<BoardController> _logger;
		public CommentController(IBoardAppService appservice, ILogger<BoardController> logger)
		{
			_appservice = appservice;
			_logger = logger;
		}

		[HttpPost]
		[Authorize]
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

		[HttpDelete("{id}")]
		[Authorize]
		public async Task DeleteBoardItemByIdAsync(long id)
		{
			await _appservice.DeleteBoardItemByIdAsync(id);
		}

		[HttpPut]
		[Authorize]
		public async Task Update(BoardItemDto boardItemDto)
		{
			await _appservice.UpdateBoardItem(boardItemDto);
		}
	}
}
