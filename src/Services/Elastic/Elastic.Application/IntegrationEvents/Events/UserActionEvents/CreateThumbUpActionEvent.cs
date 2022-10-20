﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elastic.Application.IntegrationEvents.Events.UserActionEvents
{
	public record CreateThumbUpActionEvent : CommentActionEvent
	{
		public CreateThumbUpActionEvent(string userName, int commentId, string description = "") : base(userName, commentId, description)
		{
		}
	}
}
