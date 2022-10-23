using Cube.Board.Domain;
using System;
using System.Collections.Generic;

namespace Cube.Board.Application.Dtos
{
	public class BoardItemStatsDto
	{
		public int Id { get; set; }
		public string CreatedUser { get; set; }
		public int Count 
		{
			get 
			{
				return CountOfWell + CountOfImproved;
			}
		}

		public int CountOfWell { get; set; } = 0;
		public int CountOfImproved { get; set; } = 0;
		public int CountOfComments { get; set; } = 0;
		public int CountOfThumbsup { get; set; } = 0;
	}
}
