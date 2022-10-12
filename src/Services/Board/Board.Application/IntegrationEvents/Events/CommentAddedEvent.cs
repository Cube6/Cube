using Cube.Board.Application.Dtos;
using Cube.BuildingBlocks.EventBus.Events;

namespace Cube.Board.Application.IntegrationEvents.Events;

public record CommentAddedEvent : IntegrationEvent
{
	public CommentDto Comment { get; private set; }

	public CommentAddedEvent(CommentDto comment)
	{
		Comment = comment;
	}
}
