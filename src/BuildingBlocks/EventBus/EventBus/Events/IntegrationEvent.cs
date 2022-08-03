using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cube.BuildingBlocks.EventBus.Events
{
	public record IntegrationEvent
	{
		public Guid Id { get; set; }
		public DateTime CreationTime { get; set; }

		public string EventSource { get; set; }

		public IntegrationEvent()
		{
			Id = Guid.NewGuid();
			CreationTime = DateTime.UtcNow;
		}
	}
}
