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
		[Authorize]
		public async Task<int> CreateAsync(CommentDto commentDto)
		{
			return await _appservice.CreateCommentAsync(commentDto);
		}

		[HttpGet("{boardItemId}")]
		[Authorize]
		public async Task<IEnumerable<CommentDto>> FindCommentsAsync(long boardItemId)
		{
			return await _appservice.FindCommentsByIdAsync(boardItemId);
		}

		[HttpDelete("{id}")]
		[Authorize]
		public async Task DeleteAsync(long id)
		{
			await _appservice.DeleteCommentAsync(id);
		}

		[HttpDelete]
		[Authorize]
		public async Task DeleteByBoardItemIdAndUserNameAsync(long borderItemId, string userName)
		{
			await _appservice.DeleteCommentAsync(borderItemId, userName);
		}

		[HttpPut]
		[Authorize]
		public async Task UpdateAsync(CommentDto commentDto)
		{
			await _appservice.UpdateCommentAsync(commentDto);
		}
	}
}
