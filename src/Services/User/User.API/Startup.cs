using Cube.ConsulService;
using Cube.User.API.Controllers;
using Cube.User.API.Models;
using Cube.User.API.Util;
using Cube.User.Application;
using Cube.User.Respository;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Cube.Infrastructure.Redis;
using System;
using System.Text;
using Cube.User.API.Extensions;

namespace Cube.User.API
{
	public class Startup
	{
		public Startup(IConfiguration configuration)
		{
			Configuration = configuration;
		}

		public IConfiguration Configuration { get; }

		// This method gets called by the runtime. Use this method to add services to the container.
		public void ConfigureServices(IServiceCollection services)
		{
			services.AddAutoMapper(typeof(UserApiProfile));

			services.AddGrpc();

			services.AddGrpcHttpApi();

			services.AddSwaggerGen(c =>
			{
				c.SwaggerDoc("v1", new OpenApiInfo { Title = "Cube.User.API", Version = "v1" });
			});

			services.AddDbContext<UserContext>(
				options =>
				{
					string connectionString = Configuration.GetConnectionString("DefaultConnection");
					Console.WriteLine(connectionString);
					options.UseSqlServer(connectionString);
				});

			services.AddScoped<IUserRepository, UserRepository>();
			services.AddScoped<IUserAppService, UserAppService>();
			services.AddSingleton<IRedisInstance>(RedisFactory.GetInstanceAsync(Configuration.GetConnectionString("RedisConnection")).GetAwaiter().GetResult());

			services.AddJWTAuth(Configuration);

			services.AddAuthorization();

			services.AddGrpcSwagger();
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IHostApplicationLifetime lifetime)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}

			app.UseSwagger();
			app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Cube.User.API v1"));

			app.UseRouting();

			app.UseAuthentication();

			app.UseAuthorization();

			app.UseEndpoints(endpoints =>
			{
				endpoints.MapGrpcService<UserService>();
				endpoints.MapGrpcService<HealthCheckService>();
			});

#if RELEASE
			//服务注册
			app.RegisterConsul(Configuration, lifetime);		
#endif
		}
	}
}
