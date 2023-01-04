using Cube.BuildingBlocks.EventBus.Abstractions;
using Elastic.Application.Dao;
using Elastic.Application.IntegrationEvents.Events.UserActionEvents;
using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elastic.Application.IntegrationEvents.EventHandling
{
	public class BoardItemActionEventHandler : IIntegrationEventHandler<BoardItemActionEvent>
	{
		private ElasticClient _client;
		public BoardItemActionEventHandler(ElasticClient client)
		{
			this._client = client;
		}
		public async Task Handle(BoardItemActionEvent @event)
		{
			var response = _client.IndexDocumentAsync<UserActionDao>(new UserActionDao(@event));
			switch (@event)
			{
				case CreateBoardItemActionEvent createEvent:
					var indexResponse = await _client.IndexDocumentAsync<BoardItemDao>(createEvent.BoardItem);
					break;
				case UpdateBoardItemActionEvent updateEvent:
					var updateResponse = await _client.UpdateAsync<BoardItemDao>(updateEvent.BoardItemId, b => b.Doc(updateEvent.BoardItem));
					break;
				case DeleteBoardItemActionEvent deleteEvent:
					var deleteResponse = await _client.DeleteAsync<BoardItemDao>(deleteEvent.BoardItemId);
					break;
				default:
					throw new NotImplementedException();
			}
		}
	}
}
