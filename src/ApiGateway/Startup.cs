using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Ocelot.Provider.Consul;
using Ocelot.Cache.CacheManager;
using Ocelot.Provider.Polly;
using Cube.GatewayService.Hubs;

namespace Cube.GatewayService
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
			});

			services.AddOcelot()
					.AddConsul()
					.AddCacheManager(x =>
					{
						x.WithDictionaryHandle();
					})
					.AddPolly();
			services.AddControllers();
			services.AddSwaggerGen(c =>
			{
				c.SwaggerDoc("v1", new OpenApiInfo { Title = "Web.Bff.DiscussionBoard", Version = "v1" });
			});

			services.AddCors(options =>
			{
				options.AddPolicy(name: "cors",
								  builder =>
								  {
									  builder.WithOrigins("http://localhost:81",
														  "http://cube");
								  });
			});
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
				app.UseSwagger();
				app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Web.Bff.DiscussionBoard v1"));
			}

			app.UseRouting();

			app.UseCors("cors");

			app.UseAuthorization();

			app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllers();
				endpoints.MapHub<BoardHub>("/BoardHub")
						.RequireCors(t => t.WithOrigins(new string[] { "http://localhost:81", "http://cube" })
						.AllowAnyMethod()
						.AllowAnyHeader()
						.AllowCredentials());
			});

			app.UseOcelot().Wait();
		}
	}
}
