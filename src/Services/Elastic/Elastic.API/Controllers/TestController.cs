using Elastic.Application;
using Microsoft.AspNetCore.Mvc;
using Elastic.Application.IntegrationEvents.Events.SearchEvents.Response;
using Elastic.Application.IntegrationEvents.Events.SearchEvents.Request;
using RabbitMq;
using Cube.BuildingBlocks.EventBus.EventBusRabbitMQ;
using Elastic.Application.Dao;
using Cube.BuildingBlocks.EventBus;
using Elastic.Application.IntegrationEvents.Events.UserActionEvents;

namespace Elastic.API.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class TestController : ControllerBase
	{


		private readonly ILogger<ElasticController> _logger;
		private readonly IElasticService _service;

		public TestController(ILogger<ElasticController> logger, IElasticService service)
		{
			_logger = logger;
			_service = service;
		}

		[HttpGet]
		[Route("CreateTest")]
		public void CreateTest()
		{
			var random = new Random();
			var boardId = random.Next(1, 100000);
			IMessageQueue messageQueue = new RabbitMQService("amqp://admin:admin@localhost:5672", "TestQueue");
			var eventBus = new EventBusRabbitMQ(messageQueue, new InMemoryEventBusSubscriptionsManager(), null);
			var board = new BoardDao()
			{
				Keyword = "test" + boardId,
				CreationDate = DateTime.Now,
				Id = Guid.NewGuid(),
				EntityId = boardId,
				CreateUser = "tester"
			};
			var newEvent = new CreateBoardActionEvent(board);
			newEvent.CreationTime = DateTime.Now;
			eventBus.Publish(newEvent);

			var boardItemId = random.Next(1, 100000);
			var boardItem = new BoardItemDao()
			{
				BoardId = boardId,
				CreateUser = "tester",
				Keyword = "test detail",
				EntityId = boardItemId,
				CreationDate = DateTime.Now
			};
			var newEvent2 = new CreateBoardItemActionEvent(boardItem);
			newEvent2.CreationTime = DateTime.Now;
			eventBus.Publish(newEvent2);

			var comment = new CommentDao()
			{
				BoardItemId = boardItemId,
				Keyword = "test comment",
				EntityId = random.Next(1, 100000),
				CreationDate = DateTime.Now
			};
			var newEvent3 = new CreateCommentActionEvent(comment);
			newEvent3.CreationTime = DateTime.Now;
			eventBus.Publish(newEvent3);
		}

		[HttpGet]
		[Route("UpdateTest")]
		public void UpdateTest()
		{
			IMessageQueue messageQueue = new RabbitMQService("amqp://admin:admin@localhost:5672", "TestQueue");
			var eventBus = new EventBusRabbitMQ(messageQueue, new InMemoryEventBusSubscriptionsManager(), null);
			var board = new BoardDao()
			{
				Keyword = "test" + 100000,
				CreationDate = DateTime.Now,
				Id = Guid.NewGuid(),
				EntityId = 100000,
				CreateUser = "tester"
			};
			var newEvent = new CreateBoardActionEvent(board);
			newEvent.CreationTime = DateTime.Now;
			eventBus.Publish(newEvent);
			board.Keyword = "ModifiedEvent";

			var newEvent2 = new UpdateBoardActionEvent(board);
			newEvent2.CreationTime = DateTime.Now;
			eventBus.Publish(newEvent2);
		}

		[HttpGet]
		[Route("DeleteTest")]
		public void DeleteTest()
		{
			var newEvent = new DeleteBoardActionEvent("test", 100000);
			newEvent.CreationTime = DateTime.Now;
			IMessageQueue messageQueue = new RabbitMQService("amqp://admin:admin@localhost:5672", "TestQueue");
			var eventBus = new EventBusRabbitMQ(messageQueue, new InMemoryEventBusSubscriptionsManager(), null);
			eventBus.Publish(newEvent);
		}
	}
}