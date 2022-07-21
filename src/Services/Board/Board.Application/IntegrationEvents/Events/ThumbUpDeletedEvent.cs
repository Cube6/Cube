using Cube.Board.Application.Dtos;
using Cube.Board.Domain;
using Cube.BuildingBlocks.EventBus.Events;

namespace Cube.Board.Application.IntegrationEvents.Events;

public record ThumbUpDeletedEvent : IntegrationEvent
{
	public long BoardItemId { get; private set; }
	public string UserName { get; private set; }

	public ThumbUpDeletedEvent(long boardItemId, string userName)
	{
		BoardItemId = boardItemId;
		UserName = userName;
	}
}