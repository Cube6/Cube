using Cube.Board.Domain;
using System;
using System.Collections.Generic;

namespace Cube.Board.Application.Dtos
{
	public class ProjectDto
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public DateTime DateCreated { get; set; }
		public DateTime DateModified { get; set; }
		public bool IsDeleted { get; set; }
		public List<BoardDto> Boards { get; set; }
	}
}
