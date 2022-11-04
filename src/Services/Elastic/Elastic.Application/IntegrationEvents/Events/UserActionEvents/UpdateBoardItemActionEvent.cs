﻿using Elastic.Application.Dao;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elastic.Application.IntegrationEvents.Events.UserActionEvents
{
	public record UpdateBoardItemActionEvent : BoardItemActionEvent
	{
		public BoardItemDao BoardItem { get; set; }
		private UpdateBoardItemActionEvent(string userName, int boardItemId, int boardId, string description = "") : base(userName, boardItemId, boardId, description)
		{
		}

		public UpdateBoardItemActionEvent(BoardItemDao boardItem, string description = "") : this(boardItem.Creator, boardItem.EntityId, boardItem.BoardId, description)
		{
			this.BoardItem = boardItem;
		}
	}
}