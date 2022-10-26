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
	.DefaultMappingFor<EntityDao>(m => m.IndexName("cube.entity.default"))
	.DefaultMappingFor<BoardItemDao>(m => m.IndexName("cube.entity.boardItem"))
	.DefaultMappingFor<BoardDao>(m => m.IndexName("cube.entity.board"))
	.DefaultMappingFor<CommentDao>(m => m.IndexName("cube.entity.comment"));

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

builder.Services.AddSingleton<IEventBusSubscriptionsManager, InMemoryEventBusSubscriptionsManager>();
builder.Services.AddSingleton<IEventBus, EventBusRabbitMQ>(sp =>
{
	IMessageQueue messageQueue = new RabbitMQService(rabbitMQConnStr, rabbitMQTopic);
	var eventBusSubcriptionsManager = sp.GetRequiredService<IEventBusSubscriptionsManager>();
	var iLifetimeScope = sp.GetRequiredService<ILifetimeScope>();
	var eventBus = new EventBusRabbitMQ(messageQueue, eventBusSubcriptionsManager, iLifetimeScope);
	eventBus.Subscribe<CreateBoardActionEvent, BoardActionEventHandler>();
	eventBus.Subscribe<DeleteBoardActionEvent, BoardActionEventHandler>();
	eventBus.Subscribe<UpdateBoardActionEvent, BoardActionEventHandler>();
	eventBus.Subscribe<BoardActionEvent, BoardActionEventHandler>();
	return eventBus;
});

builder.Services.AddTransient<BoardActionEventHandler>();

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
