using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elastic.Application.IntegrationEvents.Events.UserActionEvents
{
	public record CreateBoardItemActionEvent : BoardItemActionEvent
	{
		public CreateBoardItemActionEvent(string userId, int boardItemId, string description = "") : base(userId, boardItemId, description)
		{
		}
	}
}
