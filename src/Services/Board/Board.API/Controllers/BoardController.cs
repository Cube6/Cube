using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Board.Domain;
using Board.API.Models;

namespace Board.API.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class BoardController : ControllerBase
	{
		private readonly IBoardRepository _repository;
		private readonly ILogger<BoardController> _logger;

		public BoardController(IBoardRepository repository,ILogger<BoardController> logger)
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
					Name ="Testing Board",
				}
			};
		}

		[HttpGet("{id}")]
		public async Task<IEnumerable<DisscussionBoardItem>> FindItemsByIdAsync(long id)
        {
			return await _repository.GetBoardItemAsync(id);
        }

		[HttpDelete("{id}")]
		public async Task DeleteBoardByIdAsync(long id)
        {
			await _repository.DeleteBoardAsync(id);
        }
	}
}
