using Cube.Board.Application.IntegrationEvents.Events;
using Cube.Board.Respository;
using Cube.BuildingBlocks.EventBus.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cube.Board.Application.IntegrationEvents.EventHandling;

public class CommentDeletedEventHandler : IIntegrationEventHandler<CommentDeletedEvent>
{
	private IBoardRepository _repository;
	public CommentDeletedEventHandler(IBoardRepository repository)
	{
		_repository = repository;
	}

	public async Task Handle(CommentDeletedEvent @event)
	{
		await _repository.DeleteCommentAsync(@event.CommentId);
	}
}
