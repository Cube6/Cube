using Cube.BuildingBlocks.EventBus.Abstractions;
using Cube.BuildingBlocks.EventBus.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cube.BuildingBlocks.EventBus
{
	public class InMemoryEventBusSubscriptionsManager : IEventBusSubscriptionsManager
	{
		private readonly Dictionary<string, Type> _handlers;
		private readonly List<Type> _eventTypes;

		public InMemoryEventBusSubscriptionsManager()
		{
			_handlers = new Dictionary<string, Type>();
			_eventTypes = new List<Type>();
		}

		public void AddSubscription<T, TH>()
			where T : IntegrationEvent
			where TH : IIntegrationEventHandler<T>
		{
			var eventName = GetEventKey<T>();
			if(!_handlers.ContainsKey(eventName))
			{
				_handlers[eventName]= typeof(TH);
			}

			if (!_eventTypes.Contains(typeof(T)))
			{
				_eventTypes.Add(typeof(T));
			}
		}

		public string GetEventKey<T>()
		{
			return typeof(T).Name;
		}

		public Type GetEventTypeByName(string eventName) => _eventTypes.SingleOrDefault(t => t.Name == eventName);

		public Type GetHandlerForEvent<T>() where T : IntegrationEvent
		{
			var eventName = GetEventKey<T>();
			return GetHandlerForEvent(eventName);
		}

		public Type GetHandlerForEvent(string eventName) => _handlers[eventName];

		public bool HasSubscriptionsForEvent<T>() where T : IntegrationEvent
		{
			var eventName = GetEventKey<T>();
			return HasSubscriptionsForEvent(eventName);
		}

		public bool HasSubscriptionsForEvent(string eventName) => _handlers.ContainsKey(eventName);
	}
}