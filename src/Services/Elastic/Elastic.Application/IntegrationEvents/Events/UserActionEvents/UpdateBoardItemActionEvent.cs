using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elastic.Application.IntegrationEvents.Events.UserActionEvents
{
	public record UpdateBoardItemActionEvent : BoardItemActionEvent
	{
		public UpdateBoardItemActionEvent(string userName, int boardItemId, string description = "") : base(userName, boardItemId, description)
		{
		}
	}
}
