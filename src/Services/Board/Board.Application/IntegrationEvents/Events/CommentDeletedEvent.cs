using Cube.BuildingBlocks.EventBus.Events;

namespace Cube.Board.Application.IntegrationEvents.Events;

public record CommentDeletedEvent : IntegrationEvent
{
	public long CommentId { get; private set; }

	public CommentDeletedEvent(long commentId)
	{
		CommentId = commentId;
	}
}
