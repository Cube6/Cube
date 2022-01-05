using Castle.DynamicProxy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Threading;
using StackExchange.Redis;
using Microsoft.VisualStudio.Threading;

namespace RedisPractice
{
	internal class SimpleRedisRepository : RedisRepository, IAsyncInterceptor, IRedisInstance
	{
		private ThreadLocal<ITransaction> transaction = new ThreadLocal<ITransaction>();

		public SimpleRedisRepository()
		{
			
		}

		public SimpleRedisRepository(string connectionString) : this()
		{
			InitializeAsync(connectionString).Wait();
		}

		#region Operations

		public void BeginTransaction()
		{
			var tran = database.CreateTransaction();
			transaction.Value = tran;
		}

		public async Task CommitAsync()
		{
			if (transaction.Value == null)
				throw new InvalidOperationException(nameof(transaction));

			//TODO: What to do with the return value?
			var result = await transaction.Value.ExecuteAsync();

			transaction.Value = null;
		}
		public virtual async Task<V> GetAsync<V>(string key)
		{
			return await GetAsync<string, V>(key);
		}
		public virtual async Task<V> GetAsync<K, V>(K key)
		{
			try
			{
				var stringKey = JsonConvert.SerializeObject(key);
				var value = await database.StringGetAsync(stringKey);
				var objValue = JsonConvert.DeserializeObject<V>(value);
				return objValue;
			}catch(Exception)
			{
				return default;
			}
		}

		public virtual async Task<bool> SetAsync<V>(string key, string value, int expire)
		{
			return await SetAsync<string, string>(key, value, expire);
		}
		public virtual async Task<bool> SetAsync<K,V>(K key, V value, int expire)
		{
			try
			{
				var stringKey = JsonConvert.SerializeObject(key);
				var stringValue = JsonConvert.SerializeObject(value);

				TimeSpan? timespan = null;
				if(expire > 0)
					timespan = TimeSpan.FromSeconds(expire);

				return await database.StringSetAsync(stringKey, stringValue, timespan);
			}
			catch (Exception)
			{
				return false;
			}
		}
		#endregion

		#region Dynamic Proxy
		/// <summary>
		/// Called for methods that return Task but not generic Task<T>
		/// </summary>
		/// <param name="invocation"></param>
		/// <exception cref="NotImplementedException"></exception>
		public async void InterceptAsynchronous(IInvocation invocation)
		{
			AsyncReaderWriterLock.Releaser readLockReleaser = default;
			try
			{
				readLockReleaser = await disposeLock.ReadLockAsync();

				invocation.Proceed();
				var task = (Task)invocation.ReturnValue;
				await task;

			}
			finally
			{
				if (disposeLock.IsReadLockHeld)
					await readLockReleaser.ReleaseAsync();
			}
		}

		public async void InterceptAsynchronous<TResult>(IInvocation invocation)
		{
			AsyncReaderWriterLock.Releaser readLockReleaser = default;
			try
			{
				readLockReleaser = await disposeLock.ReadLockAsync();

				invocation.Proceed();
				var task = (Task<TResult>)invocation.ReturnValue;
				TResult result = await task;
			}
			finally
			{
				if (disposeLock.IsReadLockHeld)
					await readLockReleaser.ReleaseAsync();
			}
		}

		/// <summary>
		/// Only called for synchronous methods which do not return task or TASK<T>
		/// </summary>
		/// <param name="invocation"></param>
		/// <exception cref="NotImplementedException"></exception>
		public async void InterceptSynchronous(IInvocation invocation)
		{
			AsyncReaderWriterLock.Releaser readLockReleaser = default;
			try
			{
				readLockReleaser = await disposeLock.ReadLockAsync();

				invocation.Proceed();
			}
			finally
			{
				if (disposeLock.IsReadLockHeld)
					await readLockReleaser.ReleaseAsync();
			}
		}

		#endregion
	}
}
