using User.Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Data;

namespace User.Respository
{
	public class UserRepository : Repository, IUserRepository
	{
		UserContext _context = new UserContext();
		public UserRepository()
		{

		}


		public async Task<User.Domain.User> GetUserByIdAsync(long id)
		{
			var result = await _context.Users.FirstAsync(it => it.Id == id);
			return result;
		}
	}
}
