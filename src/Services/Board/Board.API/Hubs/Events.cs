using Cube.Board.Application.Dtos;

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
		public CommentDto Comment { get; set; }
	}
}
