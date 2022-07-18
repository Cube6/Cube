using Cube.Board.Application.IntegrationEvents.Events;
using Cube.BuildingBlocks.EventBus.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cube.Board.Application.IntegrationEvents.EventHandling;

internal class CommentDeletedEventHandler : IIntegrationEventHandler<CommentDeletedEvent>
{
	public Task Handle(CommentDeletedEvent @event)
	{
		throw new NotImplementedException();
	}
}
