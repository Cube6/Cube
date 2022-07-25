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
		public async Task<BoardItemDto> CreateAsync(BoardItemDto boardItemDto)
		{
			return await _appservice.CreateBoardItemAsync(boardItemDto);
		}

		[HttpGet("{boardId}")]
		[Authorize]
		public async Task<IEnumerable<BoardItemDto>> FindAsync(long boardId)
		{
			return await _appservice.FindBoardItemByBoardIdAsync(boardId);
		}

		[HttpDelete("{id}")]
		[Authorize]
		public async Task DeleteBoardItemByIdAsync(long id)
		{
			await _appservice.DeleteBoardItemByIdAsync(id);
		}

		[HttpPut]
		[Authorize]
		public async Task UpdateAsync(BoardItemDto boardItemDto)
		{
			await _appservice.UpdateBoardItemAsync(boardItemDto);
		}
	}
}
