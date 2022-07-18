using Cube.Board.Application.IntegrationEvents.Events;
using Cube.BuildingBlocks.EventBus.Abstractions;
using System;
using System.Threading.Tasks;

namespace Cube.Board.Application.IntegrationEvents.EventHandling;

internal class CommentAddedEventHandler : IIntegrationEventHandler<CommentAddedEvent>
{
	public Task Handle(CommentAddedEvent @event)
	{
		throw new NotImplementedException();
	}
}
