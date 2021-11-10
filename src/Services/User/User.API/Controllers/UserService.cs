using Cube.User.API.Protos;
using Grpc.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static Cube.User.API.Protos.UserService;

namespace Cube.User.API.Controllers
{
	public class UserService : UserServiceBase
	{
		public override Task<Protos.User> FindUserById(Id request, ServerCallContext context)
		{
			return Task.FromResult(new Protos.User()
			{
				Username = "testUser",
				PhoneNumber = "1234"
			});
		}
	}
}
