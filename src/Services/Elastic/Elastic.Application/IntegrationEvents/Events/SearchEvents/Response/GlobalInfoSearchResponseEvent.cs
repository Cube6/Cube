using Cube.BuildingBlocks.EventBus.Events;
using Elastic.Application.Dao;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elastic.Application.IntegrationEvents.Events.SearchEvents.Response
{
	public record GlobalInfoSearchResponseEvent : IntegrationEvent
	{
		public List<EntityDao> Boards { get; set; } = new List<EntityDao>();
		public List<EntityDao> BoardItems { get; set; } = new List<EntityDao>();
		public List<EntityDao> Comments { get; set; } = new List<EntityDao>();
	}
}
