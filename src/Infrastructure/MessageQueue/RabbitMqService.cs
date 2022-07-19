using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Concurrent;
using System.Text;
using System.Threading.Tasks;

namespace RabbitMq
{
	public class RabbitMQService : IMessageQueue, IDisposable
	{
		private readonly string BROKER_NAME = "cube_event_bus";
		private ConcurrentDictionary<Guid, EventingBasicConsumer> Consumers = new ConcurrentDictionary<Guid, EventingBasicConsumer>();
		private IConnection Connection { get; set; }
		private IModel Channel { get; set; }
		private string _queueName { get; set; }

		public RabbitMQService(string connectionString, string queueName)
		{
			_queueName = queueName;

			var uri = new Uri(connectionString);
			var factory = new ConnectionFactory() { Uri = uri };
			Connection = factory.CreateConnection();
			Channel = Connection.CreateModel();

			Channel.ExchangeDeclare(exchange: BROKER_NAME, type: "direct", durable:true);
		}

		public void Publish<MessageType>(MessageType message)
		{
			string routingKey = message.GetType().Name;
			var jsonObject = JsonConvert.SerializeObject(message);
			Publish(routingKey, jsonObject);
		}

		public void Publish(string routingKey, string message)
		{
			DeclareQueue();
			var body = Encoding.UTF8.GetBytes(message);
			Channel.BasicPublish(exchange: BROKER_NAME, routingKey: routingKey, mandatory: true, basicProperties: null, body: body);
		}

		//public Guid Subscribe<T>(Action<string, T> messageHandler)
		//{
		//	//var eventName = "String";
		//	//Channel.QueueBind(queue: _queueName,
		//	//					exchange: BROKER_NAME,
		//	//					routingKey: eventName);

		//	var consumer = new EventingBasicConsumer(Channel);
		//	consumer.Received += (model, ea) =>
		//	{
		//		var routingKey = ea.RoutingKey;
		//		var body = ea.Body.ToArray();
		//		var message = Encoding.UTF8.GetString(body);
		//		var obj = JsonConvert.DeserializeObject<T>(message);
		//		messageHandler(routingKey,obj);
		//	};
		//	Channel.BasicConsume(queue: _queueName, autoAck: true, consumer: consumer);
		//	var guid = Guid.NewGuid();
		//	Consumers.TryAdd(guid, consumer);
		//	return guid;
		//}

		public Guid Subscribe<T>(Func<string, string, Task> messageHandler)
		{
			DeclareQueue();
			var eventName = typeof(T).Name;
			Channel.QueueBind(queue: _queueName,
								exchange: BROKER_NAME,
								routingKey: eventName);

			var consumer = new EventingBasicConsumer(Channel);
			consumer.Received += (model, ea) =>
			{
				var routingKey = ea.RoutingKey;
				var body = ea.Body.ToArray();
				var message = Encoding.UTF8.GetString(body);
				messageHandler(routingKey, message);
			};
			Channel.BasicConsume(queue: _queueName, autoAck: true, consumer: consumer);
			var guid = Guid.NewGuid();
			Consumers.TryAdd(guid, consumer);
			return guid;
		}

		public T Get<T>()
		{
			string str = Get();
			var obj = JsonConvert.DeserializeObject<T>(str);
			return obj;
		}

		public string Get()
		{
			DeclareQueue();
			var result = Channel.BasicGet(_queueName, true);
			if (result == null)
				return null;
			var response = Encoding.UTF8.GetString(result.Body.ToArray());
			return response;
		}
		public void UnSubscribe(Guid consumerGuid)
		{
			if (Consumers.TryGetValue(consumerGuid, out _))
			{
				Consumers.TryRemove(consumerGuid, out _);
			}
		}

		public void DeclareQueue()
		{
			Channel.QueueDeclare(queue: _queueName, durable: true, exclusive: false, autoDelete: false, arguments: null);
		}

		public void Dispose()
		{
			if (Channel.IsOpen)
			{
				Channel.Close();
			}
			if (Connection.IsOpen)
			{
				Connection.Close();
			}
		}
	}
}
