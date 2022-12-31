using Cube.Infrastructure.Repository;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Cube.Board.Domain
{
	public class Project : Entity
	{
		[MaxLength(100)]
		[Required]
		public string Name { get; set; }

		public List<DisscussionBoard> Boards { get; set; }

		public bool IsDeleted { get; set; }
	}
}
