﻿using Cube.BuildingBlocks.EventBus.Events;

namespace Elastic.Application.IntegrationEvents.Events.UserActionEvents
{
	public record BoardItemActionEvent : UserActionEvent
	{
		public int BoardItemId { get; init; }
		public int BoardId { get; init; }
		public BoardItemActionEvent(string userId, int boardItemId, int boardId, string description = "") : base(userId, description)
		{
			this.BoardItemId = boardItemId;
			this.BoardId = boardId;
		}
	}
}
