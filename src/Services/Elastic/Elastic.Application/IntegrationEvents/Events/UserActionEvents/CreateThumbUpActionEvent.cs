using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elastic.Application.IntegrationEvents.Events.UserActionEvents
{
	public record CreateThumbUpActionEvent : CommentActionEvent
	{
		public CreateThumbUpActionEvent(int userId, int commentId, string description = "") : base(userId, commentId, description)
		{
		}
	}
}
