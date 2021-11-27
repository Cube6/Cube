using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Cube.User.Respository
{
	public class UserRepository : Cube.Data.Repository, IUserRepository
	{
		private UserContext _context = new UserContext();

		public UserRepository()
		{
			_context.Database.EnsureCreated();
		}

		public Task Save(Domain.User user)
		{
			_context.Users.Add(user);
			return _context.SaveChangesAsync();
		}

		public async Task<Domain.User> GetUserByIdAsync(long id)
		{
			var result = await _context.Users.FirstOrDefaultAsync(it => it.Id == id);
			return result;
		}

		public Task<List<Domain.User>> ListAsync()
		{
			return _context.Users.ToListAsync();
		}
	}
}
