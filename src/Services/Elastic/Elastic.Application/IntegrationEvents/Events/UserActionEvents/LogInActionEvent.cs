using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elastic.Application.IntegrationEvents.Events.UserActionEvents
{
	public record LogInActionEvent : MiscellaneousActionEvent
	{
		public LogInActionEvent(int userId, string description = "") : base(userId, description)
		{
		}
	}
}
