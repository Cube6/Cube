using Microsoft.EntityFrameworkCore;

namespace Cube.User.Respository
{
	public interface IUserContext
	{
		DbSet<Domain.User> Users { get; set; }
	}
}