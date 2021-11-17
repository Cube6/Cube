using System.Threading.Tasks;
using Domain = User.Domain;

namespace User.Respository
{
	public interface IUserRepository
	{
		Task<User.Domain.User> GetUserByIdAsync(long id);

	}
}
