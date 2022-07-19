using Autofac;
using Autofac.Extensions.DependencyInjection;
using Board.API.Extensions;
using Board.API.Hubs;
using Cube.Board.Application;
using Cube.Board.Application.IntegrationEvents.EventHandling;
using Cube.Board.Application.IntegrationEvents.Events;
using Cube.Board.Respository;
using Cube.BuildingBlocks.EventBus.Abstractions;
using Cube.ConsulService;
using Cube.Infrastructure.Redis;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json.Serialization;
using Quartz;
using System;

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
		public virtual IServiceProvider ConfigureServices(IServiceCollection services)
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

			services.RegisterEventBus(Configuration);

			services.AddJWTAuth(Configuration);

			services.AddControllers().AddNewtonsoftJson(options =>
			{
				options.SerializerSettings.ContractResolver = new DefaultContractResolver();
			});

			services.RegisterQuartz();

			var container = new ContainerBuilder();
			container.Populate(services);

			return new AutofacServiceProvider(container.Build());
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

			ConfigureEventBus(app);
		}

		private void ConfigureEventBus(IApplicationBuilder app)
		{
			var eventBus = app.ApplicationServices.GetRequiredService<IEventBus>();

			eventBus.Subscribe<CommentAddedEvent, CommentAddedEventHandler>();
			eventBus.Subscribe<CommentUpdatedEvent, CommentUpdatedEventHandler>();
			eventBus.Subscribe<CommentDeletedEvent, CommentDeletedEventHandler>();
		}
	}
}
