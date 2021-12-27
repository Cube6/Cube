using System.Collections.Generic;
using System.Threading.Tasks;

namespace Cube.User.Respository
{
	public interface IUserRepository
	{
		Task Save(Domain.User user);
		Task Update(Domain.User user);
		Task<User.Domain.User> GetUserByIdAsync(long id);
		Task<bool> Exist(string name, string password);
		Task<List<Domain.User>> ListAsync();
	}
}
