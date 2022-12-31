using Cube.Board.Domain;
using Microsoft.EntityFrameworkCore;

namespace Cube.Board.Respository
{
	public class BoardContext : DbContext, IBoardContext
	{
		public DbSet<Project> Projects { get; set; }
		public DbSet<DisscussionBoard> DisscussionBoards { get; set; }
		public DbSet<DisscussionBoardItem> DisscussionBoardItems { get; set; }
		public DbSet<IntegrationEvent> IntegrationEvents { get; set; }
		public DbSet<Comment> Comments { get; set; }

		public BoardContext(DbContextOptions options)
			: base(options)
		{

		}

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			var project = new Project
			{
				Id = 1,
				Name = "Cube Lab",
			};

			modelBuilder.Entity<Project>().HasData(project);

			var board = new DisscussionBoard
			{
				Id = 1,
				Name = "SprintX Retro",
				CreatedUser = "michael",
			};

			modelBuilder.Entity<DisscussionBoard>().HasData(board);

			var boardItem1 = new DisscussionBoardItem 
			{ 
				Id = 1, 
				Detail = "Completed All Stores", 
				Type = BoardItemType.WentWell, 
				Action = "Action1", 
				CreatedUser = "Michael" 
			};
			var boardItem2 = new DisscussionBoardItem 
			{ 
				Id = 2, 
				Detail = "Quality is not good", 
				Type = BoardItemType.NeedsImproved, 
				Action = "Action2", 
				CreatedUser = "Tony" 
			};
			var boardItem3 = new DisscussionBoardItem 
			{ 
				Id = 3, 
				Detail = "Do monthly story discussions", 
				Type = BoardItemType.Action, 
				Action = "Action3", 
				CreatedUser = "Lebron" 
			};
			modelBuilder.Entity<DisscussionBoardItem>().HasData(
				boardItem1,
				boardItem2,
				boardItem3
			);

			var comment1 = new Comment()
			{
				Id =1,
				Type = CommentType.ThumbsUp,
				CreatedUser = "Michael"
			};

			var comment2 = new Comment()
			{
				Id = 2,
				Type = CommentType.ThumbsUp,
				CreatedUser = "Michael"
			};

			var comment3 = new Comment()
			{
				Id = 3,
				Type = CommentType.ThumbsUp,
				CreatedUser = "Michael"
			};
			modelBuilder.Entity<Comment>().HasData(
				comment1,
				comment2,
				comment3
			);

			base.OnModelCreating(modelBuilder);
		}
	}
}
