using Cube.Board.Application.IntegrationEvents.Events;
using Cube.Board.Respository;
using Cube.BuildingBlocks.EventBus.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cube.Board.Application.IntegrationEvents.EventHandling;

public class CommentUpdatedEventHandler : IIntegrationEventHandler<CommentUpdatedEvent>
{
	private IBoardRepository _repository;
	public CommentUpdatedEventHandler(IBoardRepository repository)
	{
		_repository = repository;
	}

	public async Task Handle(CommentUpdatedEvent @event)
	{
		var comment = await _repository.GetCommentByIdAsync(@event.CommentId);
		comment.Detail = @event.NewComment;
		comment.DateModified = DateTime.Now;

		await _repository.UpdateCommentAsync(comment);
	}
}
