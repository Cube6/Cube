using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elastic.Application.IntegrationEvents.Events.UserActionEvents
{
	public record BoardItemActionEvent : UserActionEvent
	{
		public int BoardItemId { get; init; }
		public int BoardId { get; init; }
		public BoardItemActionEvent(int userId, int boardItemId, string description = "") : base(userId, description)
		{
			this.BoardItemId = boardItemId;
		}
	}
}
