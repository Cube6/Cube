using Cube.BuildingBlocks.EventBus.Events;
using System.Runtime.CompilerServices;

namespace Elastic.Application.IntegrationEvents.Events.UserActionEvents
{
	public record DeleteThumbUpActionEvent : CommentActionEvent
	{
		public DeleteThumbUpActionEvent(string userName, long commentId, string description = "") : base(userName, commentId, description)
		{
		}
	}
}
