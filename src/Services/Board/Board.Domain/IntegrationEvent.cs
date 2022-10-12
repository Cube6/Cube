using Cube.Infrastructure.Repository;
using System.ComponentModel.DataAnnotations;

namespace Cube.Board.Domain
{
	public class IntegrationEvent: Entity
	{
		[Required]
		public string EventType { set; get; }

		[Required]
		public string EventBody { set; get; }

		public bool Published { get; set; }
	}
}
