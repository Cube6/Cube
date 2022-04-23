using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Concurrent;
using System.Text;

namespace RabbitMq
{
	public class RabbitMQService : IMessageQueue, IDisposable
	{
		private ConcurrentDictionary<Guid, EventingBasicConsumer> Consumers = new ConcurrentDictionary<Guid, EventingBasicConsumer>();
		private IConnection Connection { get; set; }
		private IModel Channel { get; set; }
		public RabbitMQService(string connectionString)
		{
			var uri = new Uri(connectionString);
			var factory = new ConnectionFactory() { Uri = uri };
			Connection = factory.CreateConnection();
			Channel = Connection.CreateModel();
		}

		public void Publish(string queueName, string message)
		{
			DeclareQueue(queueName);
			var body = Encoding.UTF8.GetBytes(message);
			Channel.BasicPublish(exchange: "", routingKey: queueName, basicProperties: null, body: body);
		}

		public Guid Subscribe(string queueName, Action<string> messageHandler)
		{
			var consumer = new EventingBasicConsumer(Channel);
			consumer.Received += (model, ea) =>
			{
				var body = ea.Body.ToArray();
				var message = Encoding.UTF8.GetString(body);
				messageHandler(message);
			};
			Channel.BasicConsume(queue: queueName, autoAck: true, consumer: consumer);
			var guid = Guid.NewGuid();
			Consumers.TryAdd(guid, consumer);
			return guid;
		}

		public string Get(string queueName)
		{
			DeclareQueue(queueName);
			var result = Channel.BasicGet(queueName, true);
			if (result == null)
				return null;
			var response = Encoding.UTF8.GetString(result.Body.ToArray());
			return response;
		}
		public void UnSubscribe(Guid consumerGuid)
		{
			if(Consumers.TryGetValue(consumerGuid, out _))
			{
				Consumers.TryRemove(consumerGuid, out _);
			}
		}

		public void DeclareQueue(string queueName)
		{
			Channel.QueueDeclare(queue: queueName, durable: true, exclusive: false, autoDelete: false, arguments: null);
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
