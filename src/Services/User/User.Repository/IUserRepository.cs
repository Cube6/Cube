using System.Threading.Tasks;
using Domain = Cube.User.Domain;

namespace Cube.User.Respository
{
	public interface IUserRepository
	{
		Task<User.Domain.User> GetUserByIdAsync(long id);

	}
}
