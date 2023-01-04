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
	public class MiscellaneousActionEventHandler : IIntegrationEventHandler<MiscellaneousActionEvent>
	{
		private ElasticClient _client;
		public MiscellaneousActionEventHandler(ElasticClient client)
		{
			this._client = client;
		}
		public async Task Handle(MiscellaneousActionEvent @event)
		{
			switch(@event)
			{
				case LogInActionEvent logInEvent:
					var logInAction = new UserActionDao(@event);
					await _client.IndexAsync(logInAction, o => o.Index("Cube.Action.LogIn"));
					break;
				case LogOutActionEvent logOutEvent:
					var logOutAction = new UserActionDao(@event);
					await _client.IndexAsync(logOutAction, o => o.Index("Cube.Action.LogOut"));
					break;
				default:
					throw new NotImplementedException();
			}
		}
	}
}
