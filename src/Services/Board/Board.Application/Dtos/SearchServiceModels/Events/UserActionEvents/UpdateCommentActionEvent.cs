using Elastic.Application.Dao;
using Cube.BuildingBlocks.EventBus.Events;

namespace Elastic.Application.IntegrationEvents.Events.UserActionEvents
{
	public record UpdateCommentActionEvent : CommentActionEvent
	{
		public CommentDao Comment { get; set; }
		private UpdateCommentActionEvent(string userName, long commentId, string description = "") : base(userName, commentId, description)
		{
		}
		public UpdateCommentActionEvent(CommentDao comment, string description = "") : this(comment.Creator, comment.EntityId, description)
		{
			this.Comment = comment;
		}
	}
}
