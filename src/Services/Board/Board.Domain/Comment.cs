using Cube.Infrastructure.Repository;
using System.ComponentModel.DataAnnotations;

namespace Cube.Board.Domain
{
	public class Comment : Entity
	{
		[Required]
		public CommentType Type { get; set; }

		[Required]
		public string CreatedUser { get; set; }

		public string Detail { get; set; }

		public virtual DisscussionBoardItem BoardItem { get; set; }
	}
}
