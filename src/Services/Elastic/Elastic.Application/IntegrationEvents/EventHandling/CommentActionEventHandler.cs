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
	public class CommentActionEventHandler : IIntegrationEventHandler<CommentActionEvent>
	{
		private ElasticClient _client;
		public CommentActionEventHandler(ElasticClient client)
		{
			this._client = client;
		}
		public async Task Handle(CommentActionEvent @event)
		{
			var response = _client.IndexDocumentAsync<UserActionDao>(new UserActionDao(@event));

			switch (@event)
			{
				case CreateCommentActionEvent createEvent:
					var indexResponse = await _client.IndexDocumentAsync<CommentDao>(createEvent.Comment);
					break;
				case UpdateCommentActionEvent updateEvent:
					var updateResponse = await _client.UpdateAsync<CommentDao>(updateEvent.CommentId, c => c.Doc(updateEvent.Comment));
					break;
				case DeleteCommentActionEvent deleteEvent:
					var deleteResponse = await _client.DeleteAsync<CommentDao>(deleteEvent.CommentId);
					break;
				case CreateThumbUpActionEvent _:
				case DeleteThumbUpActionEvent _:
					break;
				default:
					throw new NotImplementedException();
			}
		}
	}
}
