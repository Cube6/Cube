using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elastic.Application.IntegrationEvents.Events.UserActionEvents
{
	public record CommentActionEvent : UserActionEvent
	{
		public int CommentId { get; init; }
		public int BoardItemId { get; init; }
		public CommentActionEvent(int userId, int commentId, string description = "") : base(userId, description)
		{
			this.CommentId = commentId;
		}
	}
}
