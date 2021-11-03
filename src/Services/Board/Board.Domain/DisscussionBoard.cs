using Data;
using System;
using System.ComponentModel.DataAnnotations;

namespace Board.Domain
{
	public class DisscussionBoard: DataModel
	{
		public int Id { get; set; }
		[MaxLength(30)]
		public string Name { get; set; }
		public DateTime DateCreated { get; set; }
	}
}
