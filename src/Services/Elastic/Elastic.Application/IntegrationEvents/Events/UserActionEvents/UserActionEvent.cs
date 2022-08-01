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
		public int UserId { get; init; }
		public string Description { get; init; }

		public UserActionEvent(int userId, string description = "")
		{
			this.UserId = userId;
			this.Description = description;
		}
	}
}
