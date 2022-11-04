using Elastic.Application.Dao;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elastic.Application.IntegrationEvents.Events.UserActionEvents
{
	public record CreateThumbUpActionEvent : CommentActionEvent
	{
		private CreateThumbUpActionEvent(string userName, long commentId, string description = "") : base(userName, commentId, description)
		{
		}

		public CreateThumbUpActionEvent(CommentDao comment, string description = "") : this(comment.Creator, comment.EntityId, description)
		{
		}
	}
}
