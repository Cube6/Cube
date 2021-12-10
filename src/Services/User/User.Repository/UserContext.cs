using Microsoft.EntityFrameworkCore;

namespace Cube.User.Respository
{
	public class UserContext : DbContext
	{
		private const string _connectionString = "Server=tcp:10.63.223.58,1433;Initial Catalog=cube_user;Persist Security Info=False;User ID=sa;Password=Welcome1*;MultipleActiveResultSets=False;Connection Timeout=30;";

		public DbSet<Domain.User> Users { get; set; }

		protected override void OnConfiguring(DbContextOptionsBuilder options)
		{
			options.UseSqlServer(_connectionString);
		}

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			// 设置生成的表名
			modelBuilder.Entity<Domain.User>().ToTable("User");
			// 设置主键，可以不设置，会默认把Id字段当成主键
			modelBuilder.Entity<Domain.User>().HasKey(p => p.Id);

			base.OnModelCreating(modelBuilder);
		}
	}
}
