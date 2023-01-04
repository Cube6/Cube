using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elastic.Application.IntegrationEvents.Events.UserActionEvents
{
	public record DeleteBoardItemActionEvent : BoardItemActionEvent
	{
		public DeleteBoardItemActionEvent(string username, long boardItemId, int boardId, string description = "") : base(username, boardItemId, boardId, description)
		{
		}
	}
}
