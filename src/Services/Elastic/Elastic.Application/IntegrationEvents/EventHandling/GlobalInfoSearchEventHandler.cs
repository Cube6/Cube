using Cube.BuildingBlocks.EventBus.Abstractions;
using Elastic.Application.IntegrationEvents.Events.SearchEvents.Request;
using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elastic.Application.IntegrationEvents.EventHandling
{
	public class GlobalInfoSearchEventHandler : IIntegrationEventHandler<GlobalInfoSearchRequestEvent>
	{
		private IEventBus _eventBus;
		private IElasticService _service;
		public GlobalInfoSearchEventHandler(IEventBus eventBus, IElasticService service)
		{
			this._eventBus = eventBus;
			this._service = service;
		}
		public async Task Handle(GlobalInfoSearchRequestEvent @event)
		{
			//TODO: Check this to do pagination concurrently https://www.elastic.co/guide/en/elasticsearch/client/net-api/current/scrolling-documents.html
			var result = await _service.GlobalInfoSearchAsync(@event);
			_eventBus.Publish(result);
		}
	}
}
