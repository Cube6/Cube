using Cube.BuildingBlocks.EventBus.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elastic.Application.IntegrationEvents.Events
{
	public record UserActionEvent : IntegrationEvent
	{
		public string UserName { get; init; }
		public string Description { get; init; }

		public UserActionEvent(string userName, string description = "")
		{
			this.UserName = userName;
			this.Description = description;
		}
	}
}
