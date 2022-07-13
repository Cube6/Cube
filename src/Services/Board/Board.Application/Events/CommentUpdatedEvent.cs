namespace Cube.Board.Application.Events
{
	public class CommentUpdatedEvent : IntegrationEvent
	{
		public int CommentId { get; set; }
		public string NewComment { get; set; }
	}
}
