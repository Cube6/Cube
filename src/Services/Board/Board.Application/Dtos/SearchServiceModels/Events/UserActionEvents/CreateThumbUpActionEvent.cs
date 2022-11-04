using Elastic.Application.Dao;
using Cube.BuildingBlocks.EventBus.Events;

namespace Elastic.Application.IntegrationEvents.Events.UserActionEvents
{
	public record CreateThumbUpActionEvent : CommentActionEvent
	{
		private CreateThumbUpActionEvent(string userName, int commentId, string description = "") : base(userName, commentId, description)
		{
		}

		public CreateThumbUpActionEvent(CommentDao comment, string description = "") : this(comment.Creator, comment.EntityId, description)
		{
		}
	}
}
