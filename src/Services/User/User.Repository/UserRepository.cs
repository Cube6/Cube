using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Cube.User.Respository
{
	public class UserRepository : Infrastructure.Repository.Repository, IUserRepository
	{
		private UserContext _context;

		public UserRepository(UserContext context)
		{
			_context = context;
			_context.Database.EnsureCreated();
		}

		public Task Save(Domain.User user)
		{
			_context.Users.Add(user);
			return _context.SaveChangesAsync();
		}

		public Task Update(Domain.User user)
		{
			_context.Entry(user).State = EntityState.Modified;
			return _context.SaveChangesAsync();
		}

		public async Task<Domain.User> GetUserByIdAsync(long id)
		{
			var result = await _context.Users.FirstOrDefaultAsync(it => it.Id == id);
			return result;
		}

		public async Task<bool> Exist(string name, string password)
		{
			var result = await _context.Users.AnyAsync(u=>u.Name.Equals(name) && u.Password.Equals(password));
			return result;
		}

		public Task<List<Domain.User>> ListAsync()
		{
			return _context.Users.ToListAsync();
		}
	}
}
