using Board.Domain;
using Board.Respository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Board.API.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class BoardController : ControllerBase
	{
		private readonly IBoardRepository _repository;
		private readonly ILogger<BoardController> _logger;
		public BoardController(IBoardRepository repository, ILogger<BoardController> logger)
		{
			_repository = repository;
			_logger = logger;
		}

		[HttpGet]
		public IEnumerable<DisscussionBoard> GetAll()
		{
			//Show come from Repository
			return new List<DisscussionBoard>()
			{
				new DisscussionBoard()
				{
					Name = "Testing Board",
					DateCreated= DateTime.Now
				}
			};
		}

		[HttpGet("{id}")]
		public async Task<DisscussionBoardItem> FindItemsByIdAsync(long id)
		{
			return await _repository.GetBoardItemByIdAsync(id);
		}

		[HttpPost]
		public async Task CreateBoardByIdAsync(DisscussionBoard disscussionBoard)
		{
			await _repository.CreateBoardAsync(disscussionBoard);
		}

		[HttpDelete("{id}")]
		public async Task DeleteBoardByIdAsync(long id)
		{
			await _repository.DeleteBoardAsync(id);
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
