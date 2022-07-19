using Cube.Board.Application.IntegrationEvents.EventHandling;
using Cube.Board.Application.IntegrationEvents.Events;
using Cube.BuildingBlocks.EventBus;
using Cube.BuildingBlocks.EventBus.EventBusRabbitMQ;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RabbitMq;
using System.Threading.Tasks;

namespace Application.FunctionalTests
{
	[TestClass]
	public class RabbitMQTest
	{
		[TestMethod]
		public async Task SendMessage()
		{
			IMessageQueue messageQueue = new RabbitMQService("amqp://admin:Welcome1@10.63.223.58:5672", "TestQueue");
			var eventBus = new EventBusRabbitMQ(messageQueue, new InMemoryEventBusSubscriptionsManager(), null);

			eventBus.Subscribe<CommentAddedEvent, CommentAddedEventHandler>();
			eventBus.Subscribe<CommentUpdatedEvent, CommentUpdatedEventHandler>();
			eventBus.Subscribe<CommentDeletedEvent, CommentDeletedEventHandler>();

			eventBus.Publish(new CommentUpdatedEvent(1, "newComment"));
			eventBus.Publish(new CommentDeletedEvent(404));
		}
	}
}
