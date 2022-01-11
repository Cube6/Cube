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

		public string Action { get; set; }

		public int ThumbsUp { get; set; }

		public int ThumbsDown { get; set; }

		public long BoardId { get; set; }
		public virtual DisscussionBoard Board { get; set; }

		[Required]
		public string CreatedUser { get; set; }
	}
}
