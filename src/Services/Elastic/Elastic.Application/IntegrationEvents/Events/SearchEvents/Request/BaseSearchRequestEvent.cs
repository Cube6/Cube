using Cube.BuildingBlocks.EventBus.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elastic.Application.IntegrationEvents.Events.SearchEvents.Request
{
	public record BaseSearchRequestEvent : IntegrationEvent
	{
		public bool Pagination { get; init; }
		public int PageSize { get; init; }
		public int Page { get; init; }
	}
}
