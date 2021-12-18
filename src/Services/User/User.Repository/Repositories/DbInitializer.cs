using Cube.User.Respository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cube.User.Repository
{
	internal class DbInitializer
	{
		public async static Task InitializeAsync(UserContext context)
		{
			await context.Database.MigrateAsync();
		}
	}
}
