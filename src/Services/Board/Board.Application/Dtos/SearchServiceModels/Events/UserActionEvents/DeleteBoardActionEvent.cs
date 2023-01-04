using Cube.BuildingBlocks.EventBus.Events;

namespace Elastic.Application.IntegrationEvents.Events.UserActionEvents
{
	public record DeleteBoardActionEvent : BoardActionEvent
	{
		public DeleteBoardActionEvent(string username, long boardId, string description = "") : base(username, boardId, description)
		{
		}
	}
}
