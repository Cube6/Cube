using Cube.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Board.Domain
{
	public class DisscussionBoard: Entity
	{
		[MaxLength(30)]
		public string Name { get; set; }
		public string CreatedUser { get; set; }
		public DateTime DateCreated { get; set; }
		public List<DisscussionBoardItem> BoardItems { get; set; }
	}
}
