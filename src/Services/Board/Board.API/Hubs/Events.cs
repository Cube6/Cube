using Cube.Board.Application.Dtos;
using Cube.Board.Domain;

namespace Board.API.Hubs
{
	public class BoardItemEvent
	{
		public string Operation { get; set; }
		public BoardItemDto BoardItem { get; set; }
	}

	public class CommentEvent
	{
		public string Operation { get; set; }
		public int BoardId { get; set; }
		public BoardItemType Type { set; get; }
		public CommentDto Comment { get; set; }
	}

	public class UserEvent
	{
		public string UserName { get; set; }
		public string Operation { get; set; }
		public int BoardId { get; set; }
	}
}
