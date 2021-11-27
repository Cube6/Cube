using Cube.Infrastructure.Repository;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Cube.Board.Domain
{
	public class DisscussionBoard: Entity
	{
		[MaxLength(30)]
		[Required]
		public string Name { get; set; }

		[Required]
		public string CreatedUser { get; set; }
		public List<DisscussionBoardItem> BoardItems { get; set; }
	}
}
