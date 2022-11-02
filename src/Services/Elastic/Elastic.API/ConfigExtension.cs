using Elastic.Application.Dao;
using Elastic.Application;
using Elasticsearch.Net;
using Nest;
using Autofac;
using Cube.BuildingBlocks.EventBus.Abstractions;
using Cube.BuildingBlocks.EventBus.EventBusRabbitMQ;
using Cube.BuildingBlocks.EventBus;
using Elastic.Application.IntegrationEvents.EventHandling;
using Elastic.Application.IntegrationEvents.Events.UserActionEvents;
using RabbitMq;
using Elastic.Application.Helper;

namespace Elastic.API
{
	public static class ConfigExtension
	{
		public static IServiceCollection AddElastic(this IServiceCollection services, ConfigurationManager configuration)
		{
			IndexHelper.BoardIndex = "cube.entity.board";
			IndexHelper.BoardItemIndex = "cube.entity.boarditem";
			IndexHelper.CommentIndex = "cube.entity.comment";
			IndexHelper.UserActionIndex = "cube.action.default";

			services.AddScoped<IElasticService, ElasticService>();

			var nestConfig = configuration.GetSection("Nest");
			var nodes = nestConfig.GetSection("Connections")
				.Get<string[]>()
				.AsEnumerable()
				.Select(node => new Uri(node));
			var pool = new StaticConnectionPool(nodes);
			var settings = new ConnectionSettings(pool)
				.DefaultIndex("Default")
				.EnableDebugMode()
				//.BasicAuthentication(nestConfig["Username"], nestConfig["Password"])
				.ServerCertificateValidationCallback((a, b, c, d) => true)//TODO: Configure the service certificate
				.RequestTimeout(TimeSpan.FromSeconds(nestConfig.GetSection("RequestTimeout").Get<int>()))
				.ThrowExceptions()
				.DefaultMappingFor<UserActionDao>(m => m.IndexName(IndexHelper.UserActionIndex))
				.DefaultMappingFor<BoardItemDao>(m => m.IndexName(IndexHelper.BoardItemIndex).IdProperty(d => d.EntityId))
				.DefaultMappingFor<BoardDao>(m => m.IndexName(IndexHelper.BoardIndex).IdProperty(d => d.EntityId))
				.DefaultMappingFor<CommentDao>(m => m.IndexName(IndexHelper.CommentIndex).IdProperty(d => d.EntityId));

			services.AddScoped<ElasticClient>(sp =>
			{
				var client = new ElasticClient(settings);

				return client;
			});

			return services;
		}

		public static IServiceCollection AddMessageQueue(this IServiceCollection services, ConfigurationManager configuration)
		{
			string rabbitMQConnStr = configuration.GetSection("RabbitMq")["ConnectionString"];
			string rabbitMQTopic = configuration.GetSection("RabbitMq")["QueueName"];
			bool firstInitialize = false;

			services.AddSingleton<IEventBusSubscriptionsManager, InMemoryEventBusSubscriptionsManager>();
			services.AddSingleton<IEventBus, EventBusRabbitMQ>(sp =>
			{
				IMessageQueue messageQueue = new RabbitMQService(rabbitMQConnStr, rabbitMQTopic);
				var eventBusSubcriptionsManager = sp.GetRequiredService<IEventBusSubscriptionsManager>();
				var iLifetimeScope = sp.GetRequiredService<ILifetimeScope>();
				var eventBus = new EventBusRabbitMQ(messageQueue, eventBusSubcriptionsManager, iLifetimeScope);
				if (!firstInitialize)
				{
					eventBus.Subscribe<CreateBoardActionEvent, BoardActionEventHandler>();
					eventBus.Subscribe<DeleteBoardActionEvent, BoardActionEventHandler>();
					eventBus.Subscribe<UpdateBoardActionEvent, BoardActionEventHandler>();
					eventBus.Subscribe<BoardActionEvent, BoardActionEventHandler>();

					eventBus.Subscribe<CreateBoardItemActionEvent, BoardItemActionEventHandler>();
					eventBus.Subscribe<DeleteBoardItemActionEvent, BoardItemActionEventHandler>();
					eventBus.Subscribe<UpdateBoardItemActionEvent, BoardItemActionEventHandler>();
					eventBus.Subscribe<BoardItemActionEvent, BoardItemActionEventHandler>();

					eventBus.Subscribe<CreateCommentActionEvent, CommentActionEventHandler>();
					eventBus.Subscribe<DeleteCommentActionEvent, CommentActionEventHandler>();
					eventBus.Subscribe<UpdateCommentActionEvent, CommentActionEventHandler>();
					eventBus.Subscribe<CommentActionEvent, CommentActionEventHandler>();

					eventBus.Subscribe<LogInActionEvent, MiscellaneousActionEventHandler>();
					eventBus.Subscribe<LogOutActionEvent, MiscellaneousActionEventHandler>();
					eventBus.Subscribe<MiscellaneousActionEvent, MiscellaneousActionEventHandler>();

					firstInitialize = true;
				}

				return eventBus;
			});

			services.AddTransient<BoardActionEventHandler>();
			services.AddTransient<BoardItemActionEventHandler>();
			services.AddTransient<CommentActionEventHandler>();
			services.AddTransient<MiscellaneousActionEventHandler>();

			return services;
		}
	}
}
