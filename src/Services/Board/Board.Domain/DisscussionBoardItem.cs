using Data;
using System;

namespace Board.Domain
{
	public class DisscussionBoardItem : DataModel
	{
		public long Id { get; set; }
		public BoardItemType Type { set; get; }
		public string Detail { get; set; }
		public DisscussionBoard Board { get; set; }
	}
}
