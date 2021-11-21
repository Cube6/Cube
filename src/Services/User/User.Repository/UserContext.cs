using Microsoft.EntityFrameworkCore;

namespace Cube.User.Respository
{
	public class UserContext : DbContext
	{
		private const string _connectionString = "Server=tcp:techgroup.database.windows.net,1433;Initial Catalog=cube;Persist Security Info=False;User ID=techgroupdba;Password=Welcome1*;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";

		public DbSet<Domain.User> Users { get; set; }

		protected override void OnConfiguring(DbContextOptionsBuilder options)
		{
			options.UseSqlServer(_connectionString);
		}
	}
}
