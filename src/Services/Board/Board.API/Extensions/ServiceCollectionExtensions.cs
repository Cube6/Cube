using Autofac;
using Board.API.Models;
using Board.API.QuartzJobs;
using Cube.Board.Application.IntegrationEvents.EventHandling;
using Cube.BuildingBlocks.EventBus;
using Cube.BuildingBlocks.EventBus.Abstractions;
using Cube.BuildingBlocks.EventBus.EventBusRabbitMQ;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Quartz;
using RabbitMq;
using System;
using System.Text;

namespace Board.API.Extensions
{
	public static class ServiceCollectionExtensions
	{
		public static void RegisterEventBus(this IServiceCollection services, IConfiguration configuration)
		{
			services.AddSingleton<IEventBus, EventBusRabbitMQ>(sp =>
			{
				string rabbitMQConnStr = configuration.GetSection("RabbitMq")["ConnectionString"];
				string rabbitMQTopic = configuration.GetSection("RabbitMq")["QueueName"];

				IMessageQueue messageQueue = new RabbitMQService(rabbitMQConnStr, rabbitMQTopic);
				var eventBusSubcriptionsManager = sp.GetRequiredService<IEventBusSubscriptionsManager>();

				var iLifetimeScope = sp.GetRequiredService<ILifetimeScope>();

				return new EventBusRabbitMQ(messageQueue, eventBusSubcriptionsManager, iLifetimeScope);
			});

			services.AddSingleton<IEventBusSubscriptionsManager, InMemoryEventBusSubscriptionsManager>();

			services.AddTransient<CommentAddedEventHandler>();
			services.AddTransient<CommentUpdatedEventHandler>();
			services.AddTransient<CommentDeletedEventHandler>();
			services.AddTransient<ThumbUpDeletedEventHandler>();
		}

		public static void RegisterQuartz(this IServiceCollection services)
		{
			services.AddQuartz(config =>
			{
				//支持DI，默认Ijob 实现不支持有参构造函数
				config.UseMicrosoftDependencyInjectionJobFactory();

				config.ScheduleJob<PublishEventJob>(trigger => trigger
								.WithIdentity("PublishEventToMQJobTrigger")
								.StartAt(DateBuilder.EvenSecondDate(DateTimeOffset.UtcNow.AddSeconds(7)))
								.WithDailyTimeIntervalSchedule(x => x.WithInterval(3, IntervalUnit.Second))
								.WithDescription("Test: Publish Event to MQ Periodically")
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
