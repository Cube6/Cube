using Cube.BuildingBlocks.EventBus.Events;

namespace Elastic.Application.IntegrationEvents.Events.UserActionEvents
{
	public record LogInActionEvent : MiscellaneousActionEvent
	{
		public LogInActionEvent(string userName, string description = "") : base(userName, description)
		{
		}
	}
}
