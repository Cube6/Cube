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

		[HttpGet("{projectId}/{type}")]
		[Authorize]
		public IEnumerable<BoardDto> GetAll(int projectId, int type)
		{
			return _appservice.GetBoards(projectId, (BoardType)type);
		}

		[HttpPost]
		[Authorize]
		public async Task<int> CreateBoardAsync(CreateBoardDto ceateBoard)
		{
			return await _appservice.CreateBoardAsync(ceateBoard);
		}

		[HttpPut]
		[Authorize]
		public async Task UpdateBoardAsync(BoardDto board)
		{
			await _appservice.UpdateBoardAsync(board);
		}

		[HttpDelete("{id}")]
		[Authorize]
		public async Task DeleteBoardByIdAsync(long id)
		{
			await _appservice.DeleteBoardByIdAsync(id);
		}
	}
}
