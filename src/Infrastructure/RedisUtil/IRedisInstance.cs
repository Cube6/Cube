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
		public Task<bool> SetAsync<K, V>(K key, V value, int expire = 0);
		public Task<bool> HashAddAsync<K, F, V>(K key, F field, V value);
		public Task<bool> HashRemoveAsync<K, F>(K key, F field);
		public Task<V> HashGetAsync<K, F, V>(K key, F field);
		public Task<long> ListAddAsync<K, M>(K key, M member);
		public Task<long> ListRemoveAsync<K, M>(K key, M member);
		public Task<long> ListLengthAsync<K>(K key);
		public Task<List<V>> ListRangeAsync<K, V>(K key, int start, int end);
		public Task<bool> SetAddAsync<K, V>(K key, V value);
		public Task<bool> SetRemoveAsync<K, V>(K key, V value);
		public Task<List<V>> SetAllAsync<K, V>(K key);
		public Task<long> SetLengthAsync<K>(K key);
		public Task<bool> SetContainsValueAsync<K, V>(K key, V value);
	}
}
