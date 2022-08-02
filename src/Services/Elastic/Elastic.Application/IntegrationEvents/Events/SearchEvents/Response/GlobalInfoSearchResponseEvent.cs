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
		public IEnumerable<UserActionDao> UserActions { get; set; }
		public IEnumerable<BoardDao> Boards { get; set; }
		public IEnumerable<BoardItemDao> boardItemDaos { get; set; }
	}
}
