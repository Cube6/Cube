using Consul;
using Cube.Board.Application;
using Cube.Board.Respository;
using Cube.BuildingBlocks.EventBus.Abstractions;
using Cube.BuildingBlocks.EventBus.Events;
using Microsoft.Extensions.Logging;
using Quartz;
using System;
using System.Text.Json;
using System.Threading.Tasks;

namespace Board.API.QuartzJobs
{
	public class PublishEventJob : IJob
	{
		private IBoardRepository _boardRepository;
		private IEventBusSubscriptionsManager _subscriptionsManager;
		private IEventBus _eventBus;
		private ILogger<PublishEventJob> _logger;

		public PublishEventJob(IBoardRepository boardRepository, 
								IEventBusSubscriptionsManager subscriptionsManager, 
								IEventBus eventBus, 
								ILogger<PublishEventJob> logger)
		{
			_boardRepository = boardRepository;
			_subscriptionsManager = subscriptionsManager;
			_eventBus = eventBus;
			_logger = logger;
		}

		public async Task Execute(IJobExecutionContext context)
		{
			if(!PublishLockManager.AcquareLock(nameof(PublishEventJob)))
			{
				return;
			}

			var @event = await _boardRepository.GetIntegrationEventAsync();
			if (@event != null)
			{
				var eventType = _subscriptionsManager.GetEventTypeByName(@event.EventType);
				var integrationEventObj = JsonSerializer.Deserialize(@event.EventBody,
																	eventType,
																	new JsonSerializerOptions()
																	{
																		PropertyNameCaseInsensitive = true
																	});
				var integrationEvent = integrationEventObj as IntegrationEvent;

				PublishEvent(@event, integrationEvent);

				await MarkEventAsPublishedAsync(@event, integrationEvent);
			}

			PublishLockManager.ReleaseLock(nameof(PublishEventJob));
		}

		private void PublishEvent(Cube.Board.Domain.IntegrationEvent @event, IntegrationEvent integrationEvent)
		{
			_eventBus.Publish(integrationEvent);
			_logger.LogInformation($"Publish {@event.EventType} {integrationEvent.Id}");
		}

		private async Task MarkEventAsPublishedAsync(Cube.Board.Domain.IntegrationEvent @event, IntegrationEvent integrationEvent)
		{
			@event.Published = true;
			await _boardRepository.UpdateIntegrationEventAsync(@event);

			_logger.LogInformation($"Mark {@event.EventType} {integrationEvent.Id} as Published");
		}
	}
}
