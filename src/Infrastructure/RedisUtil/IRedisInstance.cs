using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Castle.DynamicProxy;

namespace RedisPractice
{
	public interface IRedisInstance
	{
		public event EventHandler<InternalErrorEventArgs> InternalErrorHandler;
		public event EventHandler<RedisErrorEventArgs> ErrorEventHandler;
		public event EventHandler<ConnectionFailedEventArgs> ConnectionFailedHandler;
		public event EventHandler<ConnectionFailedEventArgs> ConnectionRestoredHandler;
		public void BeginTransaction();
		public Task CommitAsync();
		public Task<V> GetAsync<K, V>(K key);
		public Task<V> GetAsync<V>(string key);
		public Task<bool> SetAsync<V>(string key, string value, int expire = 0);
		public Task<bool> SetAsync<K, V>(K key, V value, int expire = 0);
	}
}
