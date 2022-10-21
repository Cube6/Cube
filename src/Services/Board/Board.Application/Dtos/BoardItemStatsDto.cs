using Cube.Board.Domain;
using System;
using System.Collections.Generic;

namespace Cube.Board.Application.Dtos
{
	public class BoardItemStatsDto
	{
		public int Id { get; set; }
		public string CreatedUser { get; set; }
		public int Count { get; set; }
	}
}
