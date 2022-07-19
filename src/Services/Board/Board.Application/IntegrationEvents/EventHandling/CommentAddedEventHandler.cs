using Cube.Board.Application.IntegrationEvents.Events;
using Cube.Board.Respository;
using Cube.BuildingBlocks.EventBus.Abstractions;
using System;
using System.Threading.Tasks;

namespace Cube.Board.Application.IntegrationEvents.EventHandling;

public class CommentAddedEventHandler : IIntegrationEventHandler<CommentAddedEvent>
{
	private IBoardRepository _repository;
	public CommentAddedEventHandler(IBoardRepository repository)
	{
		_repository = repository;
	}

	public async Task Handle(CommentAddedEvent @event)
	{
		await _repository.CreateCommentAsync(@event.Comment);
	}
}
