using Autofac;
using Autofac.Extensions.DependencyInjection;
using Cube.BuildingBlocks.EventBus;
using Cube.BuildingBlocks.EventBus.Abstractions;
using Cube.BuildingBlocks.EventBus.EventBusRabbitMQ;
using Cube.ConsulService;
using Cube.Infrastructure.Redis;
using Elastic.API;
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

builder.Services.AddElastic(builder.Configuration);

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

builder.Services.AddMessageQueue(builder.Configuration);

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

#if RELEASE
	//ЗўЮёзЂВс
	app.RegisterConsul(builder.Configuration, app.Lifetime);
#endif

app.Run();
