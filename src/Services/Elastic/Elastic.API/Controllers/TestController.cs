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
		[Route("Test")]
		public void Test()
		{
			var random = new Random();
			var randNum = random.Next(1, 100000);
			IMessageQueue messageQueue = new RabbitMQService("amqp://admin:admin@localhost:5672", "TestQueue");
			var eventBus = new EventBusRabbitMQ(messageQueue, new InMemoryEventBusSubscriptionsManager(), null);
			var board = new BoardDao()
			{
				Keyword = "test" + randNum,
				CreationDate = DateTime.Now,
				Id = Guid.NewGuid(),
				EntityId = randNum,
				CreateUser = "tester"
			};
			eventBus.Publish(new CreateBoardActionEvent(board));
		}

	}
}