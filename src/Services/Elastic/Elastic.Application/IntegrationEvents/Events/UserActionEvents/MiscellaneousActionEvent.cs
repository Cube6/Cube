using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elastic.Application.IntegrationEvents.Events.UserActionEvents
{
	public record MiscellaneousActionEvent : UserActionEvent
	{
		public MiscellaneousActionEvent(string userName, string description = "") : base(userName, description)
		{
		}
	}
}
