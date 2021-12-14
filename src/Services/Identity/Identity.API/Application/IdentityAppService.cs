using Grpc.Net.Client;
using Cube.User.API.Protos;
using Microsoft.Extensions.Configuration;

namespace Cube.Identity.API.Application
{
	public class IdentityAppService : IIdentityAppService
	{
		public IdentityAppService(IConfiguration configuration)
		{
			Configuration = configuration;
		}

		public IConfiguration Configuration { get; }

		public bool Validate(string user, string password)
		{
			var address = Configuration["UserService:Address"];
			var channel = GrpcChannel.ForAddress(address);
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
