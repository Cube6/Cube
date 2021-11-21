using Cube.Data;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Cube.User.Respository
{
	public class UserRepository : Repository, IUserRepository
	{
		UserContext _context = new UserContext();

		public UserRepository()
		{

		}

		public async Task<Domain.User> GetUserByIdAsync(long id)
		{
			var result = await _context.Users.FirstAsync(it => it.Id == id);
			return result;
		}
	}
}
