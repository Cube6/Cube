using Cube.Board.Application.Dtos;
using Cube.Board.Domain;
using Cube.BuildingBlocks.EventBus.Events;

namespace Cube.Board.Application.IntegrationEvents.Events;

public class CommentAddedEvent : IntegrationEvent
{
	public Comment Comment { get; private set; }

	public CommentAddedEvent(Comment comment)
	{
		Comment = comment;
	}
}
