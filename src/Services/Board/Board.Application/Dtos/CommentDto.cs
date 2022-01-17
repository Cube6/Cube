using Cube.Board.Domain;
using System;

namespace Cube.Board.Application.Dtos
{
	public class CommentDto
	{
		public int Id { get; set; }
		public string CreatedUser { get; set; }
		public DateTime DateCreated { get; set; }
		public DateTime DateModified { get; set; }
		public CommentType Type { set; get; }
		public string Detail { get; set; }

		public int BoardItemId { get; set; }
	}
}
