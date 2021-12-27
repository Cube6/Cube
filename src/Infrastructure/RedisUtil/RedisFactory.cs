using Castle.DynamicProxy;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedisPractice
{
	public static class RedisFactory
	{
		public static async Task<IRedisInstance> GetInstanceAsync(string connection)
		{
			var redisRepository = new SimpleRedisRepository();
			await redisRepository.InitializeAsync(connection);

			var generator = new ProxyGenerator();
			var proxy = generator.CreateInterfaceProxyWithTargetInterface<IRedisInstance>(redisRepository, redisRepository);

			return proxy;
		}
	}
}
