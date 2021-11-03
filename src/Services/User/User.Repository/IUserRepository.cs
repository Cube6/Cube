using System.Threading.Tasks;
using Domain = User.Domain;

namespace Board.Respository
{
	public interface IUserRepository
	{
		Task<User.Domain.User> GetUserByIdAsync(long id);

	}
}
