using Elastic.Application.Dao;
using Cube.BuildingBlocks.EventBus.Events;

namespace Elastic.Application.IntegrationEvents.Events.UserActionEvents
{
	public record CreateBoardItemActionEvent : BoardItemActionEvent
	{
		public BoardItemDao BoardItem { get; set; }
		private CreateBoardItemActionEvent(string userName, int boardItemId, int boardId, string description = "") : base(userName, boardItemId, boardId, description)
		{
		}

		public CreateBoardItemActionEvent(BoardItemDao boardItem, string description = "") : this(boardItem.Creator, boardItem.EntityId, boardItem.BoardId, description)
		{
			this.BoardItem = boardItem;
		}
	}
}
