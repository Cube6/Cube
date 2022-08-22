using Cube.Infrastructure.Redis;
using Cube.User.API.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Cube.User.API.Extensions
{
	public static class ServiceCollectionExtensions
	{
		public static void AddJWTAuth(this IServiceCollection services, IConfiguration configuration)
		{
			services.Configure<JwtSettings>(configuration.GetSection("JwtSettings"));
			var jwtSettings = new JwtSettings();
			configuration.Bind("JwtSettings", jwtSettings);

			services.AddAuthentication("OAuth")
			.AddJwtBearer("OAuth", options =>
			{
				var secretBytes = Encoding.UTF8.GetBytes(jwtSettings.SecretKey);
				var key = new SymmetricSecurityKey(secretBytes);
				options.TokenValidationParameters = new TokenValidationParameters
				{
					ValidIssuer = jwtSettings.Issuer,
					ValidAudience = jwtSettings.Audience,
					IssuerSigningKey = key
				};
			});
		}

		public static void AddRedis(this IServiceCollection services, IConfiguration configuration)
		{
			string connectionStr = configuration.GetConnectionString("RedisConnection");
			var redisInstance = RedisFactory.GetInstanceAsync(connectionStr).Result;

			services.AddSingleton(redisInstance);
		}
	}
}
