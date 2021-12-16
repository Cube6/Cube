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

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			var board = new DisscussionBoard
			{
				Id = 1,
				Name = "SprintX Retro",
				CreatedUser = "michael",
			};

			modelBuilder.Entity<DisscussionBoard>().HasData(board);

			modelBuilder.Entity<DisscussionBoardItem>().HasData(
				new DisscussionBoardItem { Id = 1, Detail = "Completed All Stores", Type = BoardItemType.WentWell, BoardId = board.Id },
				new DisscussionBoardItem { Id = 2, Detail = "Quality is not good", Type = BoardItemType.NeedsImproved, BoardId = board.Id },
				new DisscussionBoardItem { Id = 3, Detail = "Do monthly story discussions", Type = BoardItemType.Action, BoardId = board.Id }
			);

			base.OnModelCreating(modelBuilder);
		}
	}
}
