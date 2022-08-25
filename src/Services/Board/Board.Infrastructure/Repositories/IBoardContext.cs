using Cube.Board.Domain;
using Microsoft.EntityFrameworkCore;

namespace Cube.Board.Respository
{
	public interface IBoardContext
	{
		DbSet<DisscussionBoardItem> DisscussionBoardItems { get; set; }
		DbSet<DisscussionBoard> DisscussionBoards { get; set; }
		DbSet<IntegrationEvent> IntegrationEvents { get; set; }
		public DbSet<Comment> Comments { get; set; }
	}
}