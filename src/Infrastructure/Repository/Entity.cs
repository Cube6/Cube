using System;
using System.ComponentModel.DataAnnotations;

namespace Cube.Infrastructure.Repository
{
	public class Entity
	{
		[Key]
		public int Id { get; set; }
		public DateTime DateCreated { get; set; }
		public DateTime DateModified { get; set; }
	}
}
