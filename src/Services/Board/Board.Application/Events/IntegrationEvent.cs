using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cube.Board.Application.Events
{
	public abstract class IntegrationEvent
	{
		public DateTime DateTime { get; set; }

		public string EventSource { get; set; }
	}
}
