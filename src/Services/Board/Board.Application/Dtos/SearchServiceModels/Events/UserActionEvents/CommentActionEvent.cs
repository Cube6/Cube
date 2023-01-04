using Cube.BuildingBlocks.EventBus.Events;

namespace Elastic.Application.IntegrationEvents.Events.UserActionEvents
{
	public record CommentActionEvent : UserActionEvent
	{
		public long CommentId { get; init; }
		public long BoardItemId { get; init; }
		public CommentActionEvent(string username, long commentId, string description = "") : base(username, description)
		{
			this.CommentId = commentId;
		}
	}
}
