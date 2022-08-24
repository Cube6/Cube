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
		private ConcurrentDictionary<Guid, EventingBasicConsumer> consumers = new ConcurrentDictionary<Guid, EventingBasicConsumer>();
		private IConnection _connection { get; set; }
		private IModel _channel { get; set; }
		private string _queueName { get; set; }

		public RabbitMQService(string connectionString, string queueName)
		{
			_queueName = queueName;

			var uri = new Uri(connectionString);
			var factory = new ConnectionFactory() { Uri = uri };
			_connection = factory.CreateConnection();

			_channel = _connection.CreateModel();
			_channel.ExchangeDeclare(exchange: BROKER_NAME, type: "direct", durable:true);
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

			_channel.BasicPublish(
				exchange: BROKER_NAME, 
				routingKey: routingKey, 
				mandatory: true, 
				basicProperties: null, 
				body: body);
		}

		public Guid Subscribe<T>(Func<string, string, Task> messageHandler)
		{
			DeclareQueue();
			var eventName = typeof(T).Name;
			_channel.QueueBind(
				queue: _queueName,
				exchange: BROKER_NAME,
				routingKey: eventName);

			var consumer = new EventingBasicConsumer(_channel);
			consumer.Received += (model, ea) =>
			{
				var routingKey = ea.RoutingKey;
				var body = ea.Body.ToArray();
				var message = Encoding.UTF8.GetString(body);
				messageHandler(routingKey, message);
			};
			_channel.BasicConsume(queue: _queueName, autoAck: true, consumer: consumer);
			var guid = Guid.NewGuid();
			consumers.TryAdd(guid, consumer);
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
			var result = _channel.BasicGet(_queueName, true);
			if (result == null)
				return null;
			var response = Encoding.UTF8.GetString(result.Body.ToArray());
			return response;
		}
		public void UnSubscribe(Guid consumerGuid)
		{
			if (consumers.TryGetValue(consumerGuid, out _))
			{
				consumers.TryRemove(consumerGuid, out _);
			}
		}

		public void DeclareQueue()
		{
			_channel.QueueDeclare(queue: _queueName, durable: true, exclusive: false, autoDelete: false, arguments: null);
		}

		public void Dispose()
		{
			if (_channel.IsOpen)
			{
				_channel.Close();
			}
			if (_connection.IsOpen)
			{
				_connection.Close();
			}
		}
	}
}
