using Cube.Board.Respository;
using Cube.BuildingBlocks.EventBus.Abstractions;
using Cube.Infrastructure.Redis;
using Microsoft.Extensions.Logging;
using Quartz;
using System;
using System.Text.Json;
using System.Threading.Tasks;
using IntegrationEvent = Cube.BuildingBlocks.EventBus.Events.IntegrationEvent;

namespace Board.API.QuartzJobs
{
	public class PublishEventJob : IJob
	{
		private IBoardRepository _boardRepository;
		private IEventBusSubscriptionsManager _subscriptionsManager;
		private IEventBus _eventBus;
		private ILogger<PublishEventJob> _logger;
		private IRedisInstance _redis;
		private string EventPublishKey = "EventPublishKey";

		public PublishEventJob(IBoardRepository boardRepository,
								IEventBusSubscriptionsManager subscriptionsManager,
								IEventBus eventBus,
								IRedisInstance redis,
								ILogger<PublishEventJob> logger)
		{
			_boardRepository = boardRepository;
			_subscriptionsManager = subscriptionsManager;
			_eventBus = eventBus;
			_logger = logger;
			_redis = redis;
		}

		public async Task Execute(IJobExecutionContext context)
		{
			try
			{
				if (!_redis.LockTake(EventPublishKey))
				{
					return;
				}

				var @event = await _boardRepository.GetIntegrationEventAsync();
				if (@event == null)
				{
					return;
				}

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
			catch (Exception ex)
			{
				_logger.LogError(ex, $"Failed to publish event. Exception:{ex.Message}");
			}
			finally
			{
				_redis.LockRelease(EventPublishKey);
			}
		}

		private void PublishEvent(Cube.Board.Domain.IntegrationEvent @event, IntegrationEvent integrationEvent)
		{
			_eventBus.Publish(integrationEvent);

			_logger.LogInformation($"Event Published: {@event.EventType} {integrationEvent.Id}");
		}

		private async Task MarkEventAsPublishedAsync(Cube.Board.Domain.IntegrationEvent @event, IntegrationEvent integrationEvent)
		{
			@event.Published = true;
			await _boardRepository.UpdateIntegrationEventAsync(@event);

			_logger.LogInformation($"Mark Published: {@event.EventType} {integrationEvent.Id}");
		}
	}
}
