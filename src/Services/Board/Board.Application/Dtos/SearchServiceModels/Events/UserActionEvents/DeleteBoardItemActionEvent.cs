using Cube.BuildingBlocks.EventBus.Events;

namespace Elastic.Application.IntegrationEvents.Events.UserActionEvents
{
	public record DeleteBoardItemActionEvent : BoardItemActionEvent
	{
		public DeleteBoardItemActionEvent(string username, int boardItemId, int boardId, string description = "") : base(username, boardItemId, boardId, description)
		{
		}
	}
}
