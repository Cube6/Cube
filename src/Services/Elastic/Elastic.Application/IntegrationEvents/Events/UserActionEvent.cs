using Cube.BuildingBlocks.EventBus.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elastic.Application.IntegrationEvents.Events
{
	public record UserActionEvent : IntegrationEvent
	{
		public int UserId { get; init; }
		public ActionType ActionType { get; init; }
		public int InfluencedId { get; init; }
		public string Message { get; init; }

		public UserActionEvent(int userId, ActionType actionType, string message = "")
		{
			this.UserId = userId;
			this.ActionType = actionType;
			this.Message = message;
		}
		public UserActionEvent(int userId, ActionType actionType, int InfluencedId, string message = "") : this(userId, actionType, message)
		{
			this.InfluencedId = InfluencedId;
		}
	}

	public enum ActionType
	{
		LoggedIn,
		LoggedOut,

		CreatedBoard,
		CompletedBoard,
		DeletedBoard,

		AddedComment,
		UpdatedComment,
		DeletedComment,

		AddedBoardItem,
		UpdatedBoardItem,
		DeletedBoardItem
	}
}
