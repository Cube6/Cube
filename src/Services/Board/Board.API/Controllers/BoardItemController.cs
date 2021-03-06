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
		[Authorize]
		public async Task<BoardItemDto> Create(BoardItemDto boardItemDto)
		{
			return await _appservice.CreateBoardItem(boardItemDto);
		}

		[HttpGet("{boardId}")]
		[Authorize]
		public IEnumerable<BoardItemDto> Find(long boardId)
		{
			var boardItems = _appservice.FindBoardItemByBoardIdAsync(boardId);
			var result = boardItems.Result;
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
