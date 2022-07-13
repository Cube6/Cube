using Cube.Board.Application.Dtos;

namespace Cube.Board.Application.Events
{
	public class CommentAddedEvent: IntegrationEvent
	{
		public CommentDto Comment { get; set; }
	}
}
