using Grpc.Net.Client;
using Cube.User.API.Protos;
using Microsoft.Extensions.Configuration;
using System;
using System.Net.Http;

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
			var address = $"{Configuration["UserService:Scheme"]}://{Configuration["UserService:Host"]}:{Configuration["UserService:Port"]}";

			Console.WriteLine($"Try to connect to the user service: {address}");

			var httpHandler = new HttpClientHandler();
			// Return `true` to allow certificates that are untrusted/invalid
			httpHandler.ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator;

			var channel = GrpcChannel.ForAddress(address, new GrpcChannelOptions { HttpHandler = httpHandler });
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
