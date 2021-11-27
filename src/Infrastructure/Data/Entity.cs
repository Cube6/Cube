using System;
using System.ComponentModel.DataAnnotations;

namespace Cube.Data
{
	public class Entity
	{
		[Key]
		public int Id { get; set; }
		public DateTime CreateTime { get; set; }
		public DateTime ModifyTime { get; set; }
	}
}
