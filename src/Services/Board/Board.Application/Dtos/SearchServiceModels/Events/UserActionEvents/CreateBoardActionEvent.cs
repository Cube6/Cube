using Elastic.Application.Dao;
using Cube.BuildingBlocks.EventBus.Events;

namespace Elastic.Application.IntegrationEvents.Events.UserActionEvents
{
	public record CreateBoardActionEvent : BoardActionEvent
	{
		public BoardDao Board { get; set; }
		private CreateBoardActionEvent(string username, long boardId, string description = "") : base(username, boardId, description)
		{
		}

		public CreateBoardActionEvent(BoardDao board, string description = "") : this(board.CreateUser, board.EntityId, description)
		{
			this.Board = board;
		}
	}
}
