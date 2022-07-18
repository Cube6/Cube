using Board.API.Models;
using Board.API.QuartzJobs;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Quartz;
using System;
using System.Configuration;
using System.Text;
using Microsoft.Extensions.Configuration;

namespace Board.API.Extensions
{
	public static class ServiceCollectionExtensions
	{
		public static void RegisterQuartz(this IServiceCollection services)
		{
			services.AddQuartz(config =>
			{
				//支持DI，默认Ijob 实现不支持有参构造函数
				config.UseMicrosoftDependencyInjectionJobFactory();

				config.ScheduleJob<PersistCommentJob>(trigger => trigger
								.WithIdentity("CommitCommentToDBJobTrigger")
								.StartAt(DateBuilder.EvenSecondDate(DateTimeOffset.UtcNow.AddSeconds(7)))
								.WithDailyTimeIntervalSchedule(x => x.WithInterval(5, IntervalUnit.Minute))
								.WithDescription("Test: Commit Comment To DB Periodically")
				);
			});

			services.AddQuartzServer(options =>
			{
				// when shutting down we want jobs to complete gracefully
				options.WaitForJobsToComplete = true;
			});
		}

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
	}
}
