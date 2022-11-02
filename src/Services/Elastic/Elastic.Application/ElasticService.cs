using Cube.BuildingBlocks.EventBus.Abstractions;
using Cube.Infrastructure.Redis;
using Elastic.Application.Dao;
using Elastic.Application.Helper;
using Elastic.Application.IntegrationEvents.Events;
using Elastic.Application.IntegrationEvents.Events.SearchEvents.Request;
using Elastic.Application.IntegrationEvents.Events.SearchEvents.Response;
using Nest;
using Newtonsoft.Json;

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
			//_client.Search<Object>(o => o.Index("indexname").Query(q => q.MatchAll()));
			//_client.IndexAsync("hello", f => f.Index("test").Id(Guid.NewGuid().ToString()));
			//_eventBus.Publish(new UserActionEvent(1, ActionType.LoggedIn, "This is a test event"));

			var result = _client.IndexDocumentAsync<BoardDao>(new BoardDao() { Keyword = "test" }).GetAwaiter().GetResult();
			Console.WriteLine(result);
		}
		public async Task<GlobalInfoSearchResponseEvent> GlobalInfoSearchAsync(GlobalInfoSearchRequestEvent request)
		{
			var response = new GlobalInfoSearchResponseEvent();

			//{
			//	var searchDescriptor = new SearchDescriptor<BoardDao>().Query(q => q.Match(
			//		m => m.Field(b => b.Name).Fuzziness(Fuzziness.Auto).Query(request.Keyword)));
			//	var boardResult = await _client.SearchAsync<BoardDao>(searchDescriptor);
			//	response.Boards = boardResult.Documents;
			//}

			//{
			//	var searchDescriptor = new SearchDescriptor<BoardItemDao>().Query(q => q.Match(m => m.Field(b => b.Detail).Fuzziness(Fuzziness.Auto).Query(request.Keyword)));
			//	var boardItemResult = await _client.SearchAsync<BoardItemDao>(searchDescriptor);
			//	response.BoardItems = boardItemResult.Documents;
			//}

			//{
			//	var searchDescriptor = new SearchDescriptor<CommentDao>().Query(q => q.Match(m => m.Field(b => b.Detail).Fuzziness(Fuzziness.Auto).Query(request.Keyword)));
			//	var commentResult = await _client.SearchAsync<CommentDao>(searchDescriptor);
			//	response.Comments = commentResult.Documents;
			//}


			//"cube.entity.default, cube.eneity.board, cube.entity.boardItem, cube.entity.comment"

			var searchDescriptor = new SearchDescriptor<EntityDao>($"{IndexHelper.CommentIndex}, {IndexHelper.BoardItemIndex}, {IndexHelper.BoardIndex}");

			if (request.Pagination)
			{
				int from = (request.Page - 1) * request.PageSize;
				int size = request.PageSize;
				searchDescriptor.From(from).Size(size);
			}
			

			searchDescriptor.Query(query =>
			{
				var queryContainer = query.Match(m => m.Field(f => f.Keyword).Fuzziness(Fuzziness.Auto).Query(request.Keyword)) && query.DateRange(r => r.Field(f => f.CreationDate).GreaterThanOrEquals(request.StartTime).LessThanOrEquals(request.EndTime));

				if (!string.IsNullOrEmpty(request.UserName))
				{
					queryContainer = queryContainer && query.Match(m => m.Field(f => f.Creator).Query(request.UserName));
				}

				return queryContainer;
			});

			var result = await _client.SearchAsync<EntityDao>(searchDescriptor);
			
			foreach(var hit in result.Hits)
			{
				if(hit.Index == IndexHelper.BoardIndex)
				{
					response.Boards.Add(hit.Source);
				}else if(hit.Index == IndexHelper.BoardItemIndex)
				{
					response.BoardItems.Add(hit.Source);
				}else if(hit.Index == IndexHelper.CommentIndex)
				{
					response.Comments.Add(hit.Source);
				}
			}
			return response;
		}
	}
}