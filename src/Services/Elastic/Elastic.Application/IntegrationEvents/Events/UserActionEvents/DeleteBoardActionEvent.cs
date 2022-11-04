using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elastic.Application.IntegrationEvents.Events.UserActionEvents
{
	public record DeleteBoardActionEvent : BoardActionEvent
	{
		public DeleteBoardActionEvent(string username, long boardId, string description = "") : base(username, boardId, description)
		{
		}
	}
}
