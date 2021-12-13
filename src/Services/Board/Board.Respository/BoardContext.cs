using Cube.Board.Domain;
using Microsoft.EntityFrameworkCore;

namespace Cube.Board.Respository
{
	public class BoardContext : DbContext, IBoardContext
	{
		public DbSet<DisscussionBoard> DisscussionBoards { get; set; }
		public DbSet<DisscussionBoardItem> DisscussionBoardItems { get; set; }

		public BoardContext(DbContextOptions options)
			: base(options)
		{

		}
	}
}
