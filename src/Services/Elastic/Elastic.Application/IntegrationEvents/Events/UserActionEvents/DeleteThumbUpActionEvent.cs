using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elastic.Application.IntegrationEvents.Events.UserActionEvents
{
	public record DeleteThumbUpActionEvent : CommentActionEvent
	{
		public DeleteThumbUpActionEvent(string userName, long commentId, string description = "") : base(userName, commentId, description)
		{
		}
	}
}
