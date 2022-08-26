using Cube.Board.Application.IntegrationEvents.Events;
using Cube.Board.Domain;
using Cube.Board.Respository;
using Cube.BuildingBlocks.EventBus.Abstractions;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace Cube.Board.Application.IntegrationEvents.EventHandling;

public class CommentAddedEventHandler : IIntegrationEventHandler<CommentAddedEvent>
{
	private IBoardRepository _repository;
	private ILogger<CommentAddedEventHandler> _logger;

	public CommentAddedEventHandler(IBoardRepository repository, ILogger<CommentAddedEventHandler> logger)
	{
		_repository = repository;
		_logger = logger;
	}

	public async Task Handle(CommentAddedEvent @event)
	{
		var commentDto = @event.Comment;

		if (commentDto.Type == CommentType.ThumbsUp)
		{
			var existItem = await _repository.GetCommentByUserNameAsync(commentDto.BoardItemId, commentDto.CreatedUser);
			if (existItem != null)
			{
				return;
			}
		}

		var comment = new Comment()
		{
			BoardItem = await _repository.GetBoardItemByIdAsync(commentDto.BoardItemId),
			Detail = commentDto.Detail,
			CreatedUser = commentDto.CreatedUser,
			DateCreated = DateTime.Now,
			DateModified = DateTime.Now,
			Type = commentDto.Type,
		};

		await _repository.CreateCommentAsync(comment);

		_logger.LogInformation($"Thumbsup Added: BoardItem:{commentDto.BoardItemId} by {commentDto.CreatedUser}");
	}
}
