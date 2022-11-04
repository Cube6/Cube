using Cube.BuildingBlocks.EventBus.Events;

namespace Elastic.Application.IntegrationEvents.Events.UserActionEvents
{
	public record LogOutActionEvent : MiscellaneousActionEvent
	{
		public LogOutActionEvent(string userName, string description = "") : base(userName, description)
		{
		}
	}
}
