using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elastic.Application.IntegrationEvents.Events.UserActionEvents
{
	public record DeleteCommentActionEvent : CommentActionEvent
	{
		public DeleteCommentActionEvent(string username, int commentId, string description = "") : base(username, commentId, description)
		{
		}
	}
}
