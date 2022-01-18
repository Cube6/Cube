using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace Cube.GatewayService
{
	public class Program
	{
		public static void Main(string[] args)
		{
			CreateHostBuilder(args).Build().Run();
		}

		public static IHostBuilder CreateHostBuilder(string[] args)
		{
			return Host.CreateDefaultBuilder(args)
						.ConfigureAppConfiguration((hostingContext, config) =>
						{
						config.AddJsonFile("ocelot.json");
						})
						.ConfigureWebHostDefaults(webBuilder =>
						{
						webBuilder.UseStartup<Startup>();
						});
		}
	}
}
