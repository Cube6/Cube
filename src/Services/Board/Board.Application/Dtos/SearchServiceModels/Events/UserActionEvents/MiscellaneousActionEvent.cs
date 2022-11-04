using Cube.BuildingBlocks.EventBus.Events;

namespace Elastic.Application.IntegrationEvents.Events.UserActionEvents
{
	public record MiscellaneousActionEvent : UserActionEvent
	{
		public MiscellaneousActionEvent(string userName, string description = "") : base(userName, description)
		{
		}
	}
}
