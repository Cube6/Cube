using Elastic.Application.IntegrationEvents.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elastic.Application.Dao
{
	public record UserActionDao : BaseDao
	{
		public int UserId { get; set; }
		public string OperationType { get; set; }
		public UserActionEvent ExtraInfo { get; set; }

		public UserActionDao(UserActionEvent @event)
		{
			this.UserId = @event.UserId;
			this.OperationType = @event.GetType().Name;
			this.ExtraInfo = @event;
		}
	}
}
