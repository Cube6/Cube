using Autofac;
using Cube.BuildingBlocks.EventBus.Abstractions;
using Cube.BuildingBlocks.EventBus.Events;
using RabbitMq;
using System;
using System.Text.Json;
using System.Threading.Tasks;

namespace Cube.BuildingBlocks.EventBus.EventBusRabbitMQ
{
	public class EventBusRabbitMQ : IEventBus
	{
		const string AUTOFAC_SCOPE_NAME = "cube_event_bus";
		private IMessageQueue _messageQueue;
		private IEventBusSubscriptionsManager _subscriptionsManager;
		private ILifetimeScope _autofac;

		public EventBusRabbitMQ(IMessageQueue messageQueue, IEventBusSubscriptionsManager subscriptionsManager, ILifetimeScope autofac)
		{
			_messageQueue = messageQueue;
			_subscriptionsManager = subscriptionsManager;
			_autofac = autofac;
		}

		public void Publish(IntegrationEvent @event)
		{
			_messageQueue.Publish(@event);
		}

		public void Subscribe<T, TH>()
			where T : IntegrationEvent
			where TH : IIntegrationEventHandler<T>
		{
			_messageQueue.Subscribe<T>(ProcessEvent);
			_subscriptionsManager.AddSubscription<T,TH>();
		}

		public void Unsubscribe<T, TH>()
			where T : IntegrationEvent
			where TH : IIntegrationEventHandler<T>
		{

		}

		private async Task ProcessEvent(string @event, string message)
		{
			if (_subscriptionsManager.HasSubscriptionsForEvent(@event))
			{
				using var scope = _autofac.BeginLifetimeScope(AUTOFAC_SCOPE_NAME);
				var subscription = _subscriptionsManager.GetHandlerForEvent(@event);
				var handler = scope.ResolveOptional(subscription);
				if (handler == null)
				{
					return;
				}

				var eventType = _subscriptionsManager.GetEventTypeByName(@event);
				var integrationEvent = JsonSerializer.Deserialize(message, eventType, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
				var concreteType = typeof(IIntegrationEventHandler<>).MakeGenericType(eventType);

				await (Task)concreteType.GetMethod("Handle").Invoke(handler, new object[] { integrationEvent });
			}
		}
	}
}
