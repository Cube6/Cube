using Autofac;
using Autofac.Extensions.DependencyInjection;
using Cube.BuildingBlocks.EventBus;
using Cube.BuildingBlocks.EventBus.Abstractions;
using Cube.BuildingBlocks.EventBus.EventBusRabbitMQ;
using Cube.Infrastructure.Redis;
using Elastic.Application;
using Elastic.Application.Configuration;
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

builder.Services.AddSingleton<ElasticClient>(sp =>
{
	var nestConfig = builder.Configuration.GetSection("Nest");
	var nodes = nestConfig.GetSection("Connections")
		.Get<string[]>()
		.AsEnumerable()
		.Select(node => new Uri(node));
	var pool = new StaticConnectionPool(nodes);
	var settings = new ConnectionSettings(pool)
		.DefaultIndex("UnknownIndex")
		.BasicAuthentication(nestConfig["Username"], nestConfig["Password"])
		.ServerCertificateValidationCallback((a,b,c,d) => true)
		.RequestTimeout(TimeSpan.FromSeconds(nestConfig.GetSection("RequestTimeout").Get<int>()))
		.ThrowExceptions();
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
	return new EventBusRabbitMQ(messageQueue, eventBusSubcriptionsManager, iLifetimeScope);
});

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
