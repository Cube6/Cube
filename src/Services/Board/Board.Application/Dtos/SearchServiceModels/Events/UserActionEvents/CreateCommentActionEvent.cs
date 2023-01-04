using Elastic.Application.Dao;
using Cube.BuildingBlocks.EventBus.Events;

namespace Elastic.Application.IntegrationEvents.Events.UserActionEvents
{
	public record CreateCommentActionEvent : CommentActionEvent
	{
		public CommentDao Comment { get; set; }

		private CreateCommentActionEvent(string userName, long commentId, string description = "") : base(userName, commentId, description)
		{
		}

		public CreateCommentActionEvent(CommentDao comment, string description = "") : this(comment.Creator, comment.EntityId, description)
		{
			this.Comment = comment;
		}
	}
}
