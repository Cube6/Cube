using Cube.BuildingBlocks.EventBus.Abstractions;
using Elastic.Application.IntegrationEvents.Events;
using Nest;

using ActionType = Elastic.Application.IntegrationEvents.Events.ActionType;

namespace Elastic.Application
{
	public class ElasticService : IElasticService
	{
		private readonly ElasticClient _client;
		private readonly IEventBus _eventBus;
		public ElasticService(ElasticClient client, IEventBus eventBus)
		{
			_client = client;
			_eventBus = eventBus;
		}

		public void Test()
		{
			_client.Ping();
			_eventBus.Publish(new UserActionEvent(1, ActionType.LoggedIn, "This is a test event"));
		}
	}
}