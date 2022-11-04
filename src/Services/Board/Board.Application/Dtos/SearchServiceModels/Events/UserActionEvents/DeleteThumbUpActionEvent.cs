using Cube.BuildingBlocks.EventBus.Events;

namespace Elastic.Application.IntegrationEvents.Events.UserActionEvents
{
	public record DeleteThumbUpActionEvent : CommentActionEvent
	{
		public DeleteThumbUpActionEvent(string userName, int commentId, string description = "") : base(userName, commentId, description)
		{
		}
	}
}
