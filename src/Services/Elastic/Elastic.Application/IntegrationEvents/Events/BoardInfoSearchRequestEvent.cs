using Cube.BuildingBlocks.EventBus.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elastic.Application.IntegrationEvents.Events
{
	public record BoardInfoSearchRequestEvent : IntegrationEvent
	{
		public int BoardId { get; init; }
		public bool Pagination { get; init; }
		public int PageSize { get; init; }
		public int Page { get; init; }
		
	}
}
