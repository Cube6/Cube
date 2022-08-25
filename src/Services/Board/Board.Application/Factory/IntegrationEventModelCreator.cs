using Cube.Board.Domain;
using Newtonsoft.Json;
using System;

namespace Cube.Board.Application.Factory
{
	internal class IntegrationEventModelCreator
	{
		public static IntegrationEvent Create(BuildingBlocks.EventBus.Events.IntegrationEvent @event)
		{
			return new IntegrationEvent()
			{
				DateCreated = DateTime.Now,
				DateModified = DateTime.Now,
				EventType = @event.GetType().Name,
				EventBody = JsonConvert.SerializeObject(@event)
			};
		}
	}
}
