using System;

namespace Board.Domain
{
	public class DisscussionBoardItem
	{
		public long Id { get; set; }
		public BoardItemType Type { set; get; }
		public string Detail { get; set; }
		public DisscussionBoard Board { get; set; }
	}
}
