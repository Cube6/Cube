﻿using Cube.BuildingBlocks.EventBus.Events;

namespace Elastic.Application.IntegrationEvents.Events.UserActionEvents
{
	public record DeleteCommentActionEvent : CommentActionEvent
	{
		public DeleteCommentActionEvent(string username, long commentId, string description = "") : base(username, commentId, description)
		{
		}
	}
}
