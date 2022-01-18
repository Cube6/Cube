using Cube.Board.Domain;
using System;
using System.Collections.Generic;

namespace Cube.Board.Application.Dtos
{
	public class BoardDto
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public string CreatedUser { get; set; }
		public DateTime DateCreated { get; set; }
		public DateTime DateModified { get; set; }
		public bool IsDeleted { get; set; }
		public BoardState State { get; set; }
		public List<BoardItemDto> BoardItems { get; set; }
	}
}
