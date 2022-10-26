using Elastic.Application.Dao;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elastic.Application.IntegrationEvents.Events.UserActionEvents
{
	public record CreateBoardActionEvent : BoardActionEvent
	{
		public BoardDao Board { get; set; }
		private CreateBoardActionEvent(string username, int boardId, string description = "") : base(username, boardId, description)
		{
		}

		public CreateBoardActionEvent(BoardDao board, string description = "") : this(board.CreateUser, board.EntityId, description)
		{
			this.Board = board;
		}
	}
}
