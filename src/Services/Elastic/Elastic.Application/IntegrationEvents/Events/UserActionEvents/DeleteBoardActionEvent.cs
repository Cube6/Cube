using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elastic.Application.IntegrationEvents.Events.UserActionEvents
{
	public record DeleteBoardActionEvent : BoardActionEvent
	{
		public DeleteBoardActionEvent(int userId, int boardId, string description = "") : base(userId, boardId, description)
		{
		}
	}
}
