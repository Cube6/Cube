using Cube.BuildingBlocks.EventBus.Events;

namespace Cube.Board.Application.IntegrationEvents.Events;

public record CommentUpdatedEvent : IntegrationEvent
{
	public int CommentId { get; private set; }
	public string NewComment { get; private set; }

	public CommentUpdatedEvent(int commentId, string newComment)
	{
		CommentId = commentId;
		NewComment = newComment;
	}
}
