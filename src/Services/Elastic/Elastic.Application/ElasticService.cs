using Cube.BuildingBlocks.EventBus.Abstractions;
using Cube.Infrastructure.Redis;
using Elastic.Application.Dao;
using Elastic.Application.IntegrationEvents.Events;
using Elastic.Application.IntegrationEvents.Events.SearchEvents.Request;
using Elastic.Application.IntegrationEvents.Events.SearchEvents.Response;
using Nest;

namespace Elastic.Application
{
	public class ElasticService : IElasticService
	{
		private readonly ElasticClient _client;
		private readonly IEventBus _eventBus;
		private readonly IRedisInstance _redis;
		public ElasticService(ElasticClient client, IEventBus eventBus, IRedisInstance redis)
		{
			_client = client;
			_eventBus = eventBus;
			_redis = redis;
		}

		public void Test()
		{
			//_redis.SetAsync("testKey", "testValue");
			//var result = _redis.GetAsync<string, string>("testKey").GetAwaiter().GetResult();
			//Console.WriteLine(result);
			//var search = new SearchDescriptor<Object>();
			//search.
			//_client.Search<Object>(o => o.Index("indexname").Query(q =>  q.MatchAll() ));
			//_client.IndexAsync("hello", f => f.Index("test").Id(Guid.NewGuid().ToString()));
			//_eventBus.Publish(new UserActionEvent(1, ActionType.LoggedIn, "This is a test event"));
		}
		public async Task<GlobalInfoSearchResponseEvent> GlobalInfoSearchAsync(GlobalInfoSearchRequestEvent request)
		{
			var response = new GlobalInfoSearchResponseEvent();
			//TODO: This is only a test, shall be replaced later.
			var result = await _client.SearchAsync<UserActionDao>(s => s.Query(q => q.MatchAll()));
			var actions = result.Documents;
			response = response with { UserActions = actions };
			return response;
		}
	}
}