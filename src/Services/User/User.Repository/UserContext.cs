using Microsoft.EntityFrameworkCore;

namespace Cube.User.Respository
{
	public class UserContext : DbContext, IUserContext
	{
		public DbSet<Domain.User> Users { get; set; }

		public UserContext(DbContextOptions options)
			: base(options)
		{

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
