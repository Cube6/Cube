using System.Collections.Generic;
using System.Threading.Tasks;

namespace Cube.User.Respository
{
	public interface IUserRepository
	{
		Task Save(Domain.User user);
		Task<User.Domain.User> GetUserByIdAsync(long id);
		Task<List<Domain.User>> ListAsync();

	}
}
