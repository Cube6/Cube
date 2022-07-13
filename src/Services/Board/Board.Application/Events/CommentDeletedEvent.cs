namespace Cube.Board.Application.Events
{
	public class CommentDeletedEvent : IntegrationEvent
	{
		public int CommentId { get; set; }
	}
}
