using Cube.BuildingBlocks.EventBus.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cube.BuildingBlocks.EventBus.Abstractions
{
	public interface IEventBusSubscriptionsManager
	{
		void AddSubscription<T, TH>()
			where T : IntegrationEvent
			where TH : IIntegrationEventHandler<T>;

		bool HasSubscriptionsForEvent<T>() where T : IntegrationEvent;

		bool HasSubscriptionsForEvent(string eventName);

		Type GetHandlerForEvent<T>() where T : IntegrationEvent;

		Type GetHandlerForEvent(string eventName);

		Type GetEventTypeByName(string eventName);
	}
}
