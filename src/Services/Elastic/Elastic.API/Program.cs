using Autofac;
using Autofac.Extensions.DependencyInjection;
using Cube.BuildingBlocks.EventBus;
using Cube.BuildingBlocks.EventBus.Abstractions;
using Cube.BuildingBlocks.EventBus.EventBusRabbitMQ;
using Cube.Infrastructure.Redis;
using Elastic.Application;
using Elastic.Application.Configuration;
using Elastic.Application.Dao;
using Elastic.Application.IntegrationEvents.EventHandling;
using Elastic.Application.IntegrationEvents.Events.UserActionEvents;
using Elasticsearch.Net;
using Nest;
using Quartz;
using RabbitMq;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IElasticService, ElasticService>();

var nestConfig = builder.Configuration.GetSection("Nest");
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
	.DefaultMappingFor<BaseDao>(m => m.IndexName("cube.default"))
	.DefaultMappingFor<UserActionDao>(m => m.IndexName("cube.action.default"))
	.DefaultMappingFor<EntityDao>(m => m.IndexName("cube.entity.default").IdProperty(d => d.EntityId))
	.DefaultMappingFor<BoardItemDao>(m => m.IndexName("cube.entity.boarditem").IdProperty(d => d.EntityId))
	.DefaultMappingFor<BoardDao>(m => m.IndexName("cube.entity.board").IdProperty(d => d.EntityId))
	.DefaultMappingFor<CommentDao>(m => m.IndexName("cube.entity.comment").IdProperty(d => d.EntityId));

builder.Services.AddScoped<ElasticClient>(sp =>
{
	var client = new ElasticClient(settings);

	return client;
});

#region Register Automapper, Quartz, Redis, and JWT
builder.Services.AddAutoMapper(config => config.AddProfile<ElasticProfile>());

builder.Services.AddSingleton<IRedisInstance>(RedisFactory.GetInstanceAsync(builder.Configuration.GetConnectionString("RedisConnection")).GetAwaiter().GetResult());

builder.Services.AddQuartz(config =>
{
	config.UseMicrosoftDependencyInjectionJobFactory();
	//TODO: Add some jobs to do the statistic work and store results in redis
});
//TODO: JWT OAUTH authentication
#endregion

#region Register Message Queue
string rabbitMQConnStr = builder.Configuration.GetSection("RabbitMq")["ConnectionString"];
string rabbitMQTopic = builder.Configuration.GetSection("RabbitMq")["QueueName"];
bool firstInitialize = false;

builder.Services.AddSingleton<IEventBusSubscriptionsManager, InMemoryEventBusSubscriptionsManager>();
builder.Services.AddSingleton<IEventBus, EventBusRabbitMQ>(sp =>
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

builder.Services.AddTransient<BoardActionEventHandler>();
builder.Services.AddTransient<BoardItemActionEventHandler>();
builder.Services.AddTransient<CommentActionEventHandler>();
builder.Services.AddTransient<MiscellaneousActionEventHandler>();

builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());

#endregion

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
