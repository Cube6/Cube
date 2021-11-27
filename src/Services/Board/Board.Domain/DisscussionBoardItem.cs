using Cube.Infrastructure.Repository;
using System.ComponentModel.DataAnnotations;

namespace Cube.Board.Domain
{
	public class DisscussionBoardItem : Entity
	{
		[Required]
		public BoardItemType Type { set; get; }

		[Required]
		public string Detail { get; set; }

		public DisscussionBoard Board { get; set; }
	}
}
