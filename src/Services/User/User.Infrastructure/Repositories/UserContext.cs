using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

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
			//// 设置主键，可以不设置，会默认把Id字段当成主键
			//modelBuilder.Entity<Domain.User>().HasKey(p => p.Id);
			//modelBuilder.Entity<Domain.User>().Property(p => p.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

			modelBuilder.Entity<Domain.User>().HasData(
					new Domain.User 
					{ 
						Id = 1,
						Name ="michael",
						Password ="123456"
					},
					new Domain.User
					{
						Id = 2,
						Name = "alisa",
						Password = "123456"
					},
					new Domain.User
					{
						Id = 3,
						Name = "tony",
						Password = "123456"
					},
					new Domain.User
					{
						Id = 4,
						Name = "lebron",
						Password = "123456"
					},
					new Domain.User
					{
						Id = 5,
						Name = "charles",
						Password = "123456"
					},
					new Domain.User
					{
						Id = 6,
						Name = "elaine",
						Password = "123456"
					}
				);

			base.OnModelCreating(modelBuilder);
		}
	}
}
