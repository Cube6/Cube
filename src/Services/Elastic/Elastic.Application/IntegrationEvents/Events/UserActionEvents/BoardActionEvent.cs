using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elastic.Application.IntegrationEvents.Events.UserActionEvents
{
	public record BoardActionEvent : UserActionEvent
	{
		public int BoardId { get; init; }

		public BoardActionEvent(int userId, int boardId, string description = "") : base(userId, description)
		{
			this.BoardId = boardId;
		}
	}
}
