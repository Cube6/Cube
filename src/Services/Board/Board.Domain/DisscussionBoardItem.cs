using Cube.Data;
using System;

namespace Board.Domain
{
	public class DisscussionBoardItem : Entity
	{
		public BoardItemType Type { set; get; }
		public string Detail { get; set; }
		public DisscussionBoard Board { get; set; }
	}
}
