using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elastic.Application.IntegrationEvents.Events.UserActionEvents
{
	public record BoardActionEvent : UserActionEvent
	{
		public long BoardId { get; init; }

		public BoardActionEvent(string username, long boardId, string description = "") : base(username, description)
		{
			this.BoardId = boardId;
		}
	}
}
