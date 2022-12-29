﻿using Cube.Board.Domain;
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
		public BoardItemState State { get; set; }
		public string Detail { get; set; }
		public string Action { get; set; }
		public IEnumerable<CommentDto> ThumbsUp { get; set; } = new List<CommentDto>();
		public IEnumerable<CommentDto> Messages { get; set; } = new List<CommentDto>();
		public bool ToggleComment { get; set; } = false;
		public CommentDto Comment { get; set; } = new CommentDto();
		public int BoardId { get; set; }
		public int? AssociatedBoardItemId { get; set; }
		public string Assignee { get; set; }
	}
}
