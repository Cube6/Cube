using Cube.Board.Domain;
using Microsoft.EntityFrameworkCore;

namespace Cube.Board.Respository
{
	public class BoardContext : DbContext
	{
		private const string _connectionString = "Server=cn-wushuai-1;Initial Catalog=cube_board;Persist Security Info=False;User ID=sa;Password=Welcome1;";

		public DbSet<DisscussionBoard> DisscussionBoards { get; set; }
		public DbSet<DisscussionBoardItem> DisscussionBoardItems { get; set; }

		protected override void OnConfiguring(DbContextOptionsBuilder options)
		{
			options.UseSqlServer(_connectionString);
		}
	}
}
