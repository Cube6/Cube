using Elastic.Application.Dao;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
