using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elastic.Application.IntegrationEvents.Events.UserActionEvents
{
	public record LogOutActionEvent : MiscellaneousActionEvent
	{
		public LogOutActionEvent(int userId, string description = "") : base(userId, description)
		{
		}
	}
}
