using Cube.BuildingBlocks.EventBus.Abstractions;
using Cube.BuildingBlocks.EventBus.Events;
using System;

namespace Cube.BuildingBlocks.EventBus.EventBusRabbitMQ
{
	public class EventBusRabbitMQ : IEventBus
	{
		public void Publish(IntegrationEvent @event)
		{
			throw new NotImplementedException();
		}

		public void Subscribe<T, TH>()
			where T : IntegrationEvent
			where TH : IIntegrationEventHandler<T>
		{
			throw new NotImplementedException();
		}

		public void Unsubscribe<T, TH>()
			where T : IntegrationEvent
			where TH : IIntegrationEventHandler<T>
		{
			throw new NotImplementedException();
		}
	}
}
