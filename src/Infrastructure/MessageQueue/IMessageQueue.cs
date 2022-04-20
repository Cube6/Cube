using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RabbitMq
{
	public interface IMessageQueue
	{
		void Publish(string queueName, string message);
		Guid Subscribe(string queueName, Action<string> messageHandler);
		string Get(string queueName);
		void UnSubscribe(Guid consumerGuid);
	}
}
