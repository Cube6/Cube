using Cube.Board.Domain;
using System;
using System.Collections.Generic;

namespace Cube.Board.Application.Dtos
{
	public class BoardItemDto
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public string CreatedUser { get; set; }
		public DateTime DateCreated { get; set; }
		public DateTime DateModified { get; set; }
		public BoardItemType Type { set; get; }
		public string Detail { get; set; }
		public string Action { get; set; }
		public IEnumerable<CommentDto> ThumbsUp { get; set; } = new List<CommentDto>();
		public int BoardId { get; set; }
	}
}
