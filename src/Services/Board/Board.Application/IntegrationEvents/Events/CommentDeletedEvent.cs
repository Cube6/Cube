using Cube.BuildingBlocks.EventBus.Events;

namespace Cube.Board.Application.IntegrationEvents.Events;

public class CommentDeletedEvent : IntegrationEvent
{
	public int CommentId { get; private set; }

	public CommentDeletedEvent(int commentId)
	{
		CommentId = commentId;
	}
}
