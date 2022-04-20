using Cube.Board.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cube.Board.Application.Events
{
	public class CommentEvent
	{
		public EventOperation Operation { get; set; }

		public Comment Comment { get; set; }

		public DateTime DateTime { get; set; }

		public string EventSource { get; set; }
	}

	public enum EventOperation
	{
		Create,
		Update,
		Delete
	}
}
