﻿using Board.API.Models;
using Cube.Board.Respository;
using Cube.ConsulService;
using Cube.Infrastructure.Redis;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cube.Board.Application;
using Microsoft.EntityFrameworkCore;
using Board.API.Hubs;
using RabbitMq;
using Quartz;
using Board.API.QuartzJobs;
using Board.API.Extensions;

namespace Board.API
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
			services.AddSignalR(options =>
			{
				options.EnableDetailedErrors = true;
			}).AddJsonProtocol(options =>
			{
				//Prevent SignalR from converting the first letters of property to lower case
				//when it serializes the object and sends it to clients
				options.PayloadSerializerOptions.PropertyNamingPolicy = null;
			});

			services.AddControllers();
			services.AddSwaggerGen(c =>
			{
				c.SwaggerDoc("v1", new OpenApiInfo { Title = "Board.API", Version = "v1" });
			});


			//services.AddCors(option => option.AddPolicy("cors", policy => policy.AllowAnyHeader().AllowAnyMethod().AllowCredentials().AllowAnyOrigin()));

			services.AddCors(options =>
			{
				options.AddPolicy(name: "cors",
								  builder =>
								  {
									  builder.WithOrigins("http://localhost:5050",
														  "http://localhost:81",
														  "http://cube");
								  });
			});

			services.AddDbContext<BoardContext>(
						options =>
						{
							string connectionString = Configuration.GetConnectionString("DefaultConnection");
							Console.WriteLine(connectionString);
							options.UseSqlServer(connectionString);
						});

			services.AddScoped<IBoardRepository, BoardRepository>();
			services.AddScoped<IBoardAppService, BoardAppService>();
			services.AddSingleton<IRedisInstance>(RedisFactory.GetInstanceAsync(Configuration.GetConnectionString("RedisConnection")).GetAwaiter().GetResult());
			services.AddSingleton<IMessageQueue>(new RabbitMQService(Configuration.GetSection("RabbitMq")["ConnectionString"]));

			services.AddJWTAuth(Configuration);

			services.AddControllers().AddNewtonsoftJson(options => {
				options.SerializerSettings.ContractResolver = new DefaultContractResolver();
			});

			services.RegisterQuartz();
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IHostApplicationLifetime lifetime)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}

			app.UseSwagger();
			app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Board.API v1"));

			//app.UseHttpsRedirection();

			app.UseRouting();

			app.UseCors("cors");

			app.UseAuthentication();

			app.UseAuthorization();

			app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllers();
				endpoints.MapHub<BoardHub>("/BoardHub")
						.RequireCors(t => t.WithOrigins(new string[] { "http://localhost:5050", "http://localhost:81", "http://cube" })
						.AllowAnyMethod()
						.AllowAnyHeader()
						.AllowCredentials());
			});

#if RELEASE
			//服务注册
			app.RegisterConsul(Configuration, lifetime);
#endif
		}
	}
}
