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
		public CommentActionEvent(string username, int commentId, string description = "") : base(username, description)
		{
			this.CommentId = commentId;
		}
	}
}
