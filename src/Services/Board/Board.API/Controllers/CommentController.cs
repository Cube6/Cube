using Cube.Board.Application;
using Cube.Board.Application.Dtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Board.API.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class CommentController : ControllerBase
	{
		private readonly IBoardAppService _appservice;
		private readonly ILogger<CommentController> _logger;
		public CommentController(IBoardAppService appservice, ILogger<CommentController> logger)
		{
			_appservice = appservice;
			_logger = logger;
		}

		[HttpPost]
		//[Authorize]
		public async Task Create(CommentDto commentDto)
		{
			await _appservice.CreateComment(commentDto);
		}

		[HttpGet("{boardItemId}")]
		public IEnumerable<CommentDto> FindComments(long boardItemId)
		{
			var ss = _appservice.FindCommentsByIdAsync(boardItemId);
			var result = ss.Result;
			return result;
		}

		[HttpDelete("{id}")]
		//[Authorize]
		public async Task Delete(long id)
		{
			await _appservice.DeleteCommentByIdAsync(id);
		}
	}
}
