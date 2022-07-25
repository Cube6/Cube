﻿using Consul;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using System;

namespace Cube.ConsulService
{
	public static class ConsulHelper
	{
		public static IApplicationBuilder RegisterConsul(this IApplicationBuilder app, IConfiguration configuration, IHostApplicationLifetime lifetime)
		{
			try
			{
				var consulClient = new ConsulClient(config =>
				{
					config.Address = new Uri(configuration["Consul:Address"]);
				});

				bool isHttps = false;
				if (configuration["Consul:IsHttps"] != null)
				{
					isHttps = bool.Parse(configuration["Consul:IsHttps"].ToString());
				}

				string uriProtocal = isHttps ? "https" : "http";
				var registration = new AgentServiceRegistration()
				{
					ID = Guid.NewGuid().ToString(),//服务实例唯一标识
					Name = configuration["Consul:Name"], // 服务名
					Address = configuration["Consul:Ip"], // 服务绑定IP
					Port = Convert.ToInt32(configuration["Consul:Port"]), // 服务绑定端口
					Check = new AgentServiceCheck
					{
						DeregisterCriticalServiceAfter = TimeSpan.FromSeconds(5), // 服务启动多久后注册
						Interval = TimeSpan.FromSeconds(10), // 健康检查时间间隔
						HTTP = $"{uriProtocal}://{configuration["Consul:Ip"]}:{configuration["Consul:Port"]}{configuration["Consul:HealthCheck"]}", // 健康检查地址
						Timeout = TimeSpan.FromSeconds(5), // 超时时间
						TLSSkipVerify = true
					}
				};

				//服务注册
				consulClient.Agent.ServiceRegister(registration).Wait();

				//应用程序终止时，取消注册
				lifetime.ApplicationStopping.Register(() =>
				{
					consulClient.Agent.ServiceDeregister(registration.ID).Wait();
				});
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
			}

			return app;
		}
	}
}
