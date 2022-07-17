using Cube.Board.Application.IntegrationEvents.Events;
using Cube.BuildingBlocks.EventBus.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cube.Board.Application.IntegrationEvents.EventHandling;

internal class CommentUpdatedEventHandler : IIntegrationEventHandler<CommentUpdatedEvent>
{
	public Task Handle(CommentUpdatedEvent @event)
	{
		throw new NotImplementedException();
	}
}
