﻿using Cube.BuildingBlocks.EventBus.Abstractions;
using Elastic.Application.Dao;
using Elastic.Application.IntegrationEvents.Events.UserActionEvents;
using Nest;

namespace Elastic.Application.IntegrationEvents.EventHandling
{
	public class BoardActionEventHandler : IIntegrationEventHandler<BoardActionEvent>
	{
		private ElasticClient _client;
		public BoardActionEventHandler(ElasticClient client)
		{
			this._client = client;
		}
		public async Task Handle(BoardActionEvent @event)
		{
			var response = _client.IndexDocumentAsync<UserActionDao>(new UserActionDao(@event));
			switch (@event)
			{
				case CreateBoardActionEvent createEvent:
					var indexResponse = await _client.IndexDocumentAsync<BoardDao>(createEvent.Board);
					break;
				case UpdateBoardActionEvent updateEvent:
					var updateResponse = await _client.UpdateAsync<BoardDao>(updateEvent.BoardId, u => u.Doc(updateEvent.Board));
					break;
				case DeleteBoardActionEvent deleteEvent:
					var deleteResponse = await _client.DeleteAsync<BoardDao>(deleteEvent.BoardId);
					break;
				default:
					throw new NotImplementedException();
			}
		}
	}
}
