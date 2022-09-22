﻿using Elastic.Application.IntegrationEvents.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elastic.Application.Dao
{
	public record UserActionDao : BaseDao
	{
		public string Username { get; set; }
		public DateTime CreationTime { get; set; }
		public string OperationType { get; set; }
		public UserActionEvent ExtraInfo { get; set; }

		public UserActionDao(UserActionEvent @event)
		{
			this.Username = @event.UserName;
			this.OperationType = @event.GetType().Name;
			this.ExtraInfo = @event;
		}
	}
}
