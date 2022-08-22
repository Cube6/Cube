using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RabbitMq
{
	public interface IMessageQueue
	{
		void Publish<MessageType>(MessageType message);
		Guid Subscribe<T>(Func<string, string, Task> messageHandler);
		string Get();
		void UnSubscribe(Guid consumerGuid);
	}
}
