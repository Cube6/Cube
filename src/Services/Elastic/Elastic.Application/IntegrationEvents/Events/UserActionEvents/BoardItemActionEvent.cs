using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elastic.Application.IntegrationEvents.Events.UserActionEvents
{
	public record BoardItemActionEvent : UserActionEvent
	{
		public long BoardItemId { get; init; }
		public long BoardId { get; init; }
		public BoardItemActionEvent(string userId, long boardItemId, long boardId, string description = "") : base(userId, description)
		{
			this.BoardItemId = boardItemId;
			this.BoardId = boardId;
		}
	}
}
