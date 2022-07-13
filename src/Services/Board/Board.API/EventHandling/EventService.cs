//using Cube.Board.Application.Events;
//using Newtonsoft.Json;
//using RabbitMq;

//namespace Board.API.EventHandling
//{
//	public class EventService
//	{
//		public IMessageQueue _rabbitMQService;
//		public CommentEventHandler _commentEventHandler;
//		public string _queueName;

//		public EventService(IMessageQueue rabbitMQService)
//		{
//			_rabbitMQService = rabbitMQService;
//			//_queueName = Configuration.GetSection("RabbitMq")["QueueName"]);
//		}

//		public bool Process()
//		{
//			var eventMessage = _rabbitMQService.Get(_queueName);

//			var commentEvent = JsonConvert.DeserializeObject<CommentAddedEvent>(eventMessage);

//			return true;
//		}
//	}
//}
