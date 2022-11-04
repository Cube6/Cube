using Elastic.Application.Dao;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elastic.Application.IntegrationEvents.Events.UserActionEvents
{
	public record UpdateBoardActionEvent : BoardActionEvent
	{
		public BoardDao Board { get; set; }
		private UpdateBoardActionEvent(string userName, int boardId, string description = "") : base(userName, boardId, description)
		{

		}
		public UpdateBoardActionEvent(BoardDao board, string description = "") : this(board.CreateUser, board.EntityId, description)
		{
			this.Board = board;
		}
	}
}
