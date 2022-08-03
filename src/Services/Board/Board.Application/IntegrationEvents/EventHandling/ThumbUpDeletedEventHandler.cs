using Cube.Board.Application.IntegrationEvents.Events;
using Cube.Board.Respository;
using Cube.BuildingBlocks.EventBus.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cube.Board.Application.IntegrationEvents.EventHandling;

public class ThumbUpDeletedEventHandler : IIntegrationEventHandler<ThumbUpDeletedEvent>
{
	private IBoardRepository _repository;
	public ThumbUpDeletedEventHandler(IBoardRepository repository)
	{
		_repository = repository;
	}

	public async Task Handle(ThumbUpDeletedEvent @event)
	{
		await _repository.DeleteThumbsUpAsync(@event.BoardItemId, @event.UserName);
	}
}
