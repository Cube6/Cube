using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elastic.Application.IntegrationEvents.Events.UserActionEvents
{
	public record CreateBoardActionEvent : BoardActionEvent
	{
		public CreateBoardActionEvent(int userId, int boardId, string description = "") : base(userId, boardId, description)
		{
		}
	}
}
