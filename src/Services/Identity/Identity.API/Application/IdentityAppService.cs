using Grpc.Net.Client;
using Cube.User.API.Protos;

namespace Cube.Identity.API.Application
{
	public class IdentityAppService : IIdentityAppService
	{
		public bool Validate(string user, string password)
		{
			var channel = GrpcChannel.ForAddress("https://localhost:4000");
			var client = new UserService.UserServiceClient(channel);

			var result = client.Validate(new ValidateUserRequest()
			{
				Name = user,
				Password = password
			});

			return result.Success;
		}
	}
}
