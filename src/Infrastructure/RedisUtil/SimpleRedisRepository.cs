using Castle.DynamicProxy;
using Microsoft.VisualStudio.Threading;
using Newtonsoft.Json;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Cube.Infrastructure.Redis
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

		public virtual async Task<V> GetAsync<K, V>(K key)
		{
			try
			{
				var stringKey = JsonConvert.SerializeObject(key);
				var value = await database.StringGetAsync(stringKey);
				var objValue = JsonConvert.DeserializeObject<V>(value);
				return objValue;
			}
			catch (Exception)
			{
				return default;
			}
		}

		public virtual async Task<bool> SetAsync<K, V>(K key, V value, int expire)
		{
			try
			{
				var stringKey = JsonConvert.SerializeObject(key);
				var stringValue = JsonConvert.SerializeObject(value);

				TimeSpan? timespan = null;
				if (expire > 0)
				{
					timespan = TimeSpan.FromSeconds(expire);
				}

				return await database.StringSetAsync(stringKey, stringValue, timespan);
			}
			catch (Exception)
			{
				return false;
			}
		}

		public virtual async Task<bool> HashAddAsync<K, F, V>(K key, F field, V value, int expire = 0)
		{
			try
			{
				var stringKey = JsonConvert.SerializeObject(key);
				var stringField = JsonConvert.SerializeObject(field);
				var stringValue = JsonConvert.SerializeObject(value);
				var result = await database.HashSetAsync(stringKey, stringField, stringValue);

				if (expire > 0)
				{
					await database.KeyExpireAsync(stringKey, TimeSpan.FromSeconds(expire));
				}

				return result;
			}
			catch (Exception)
			{
				return false;
			}
		}

		public virtual async Task<bool> HashRemoveAsync<K, F>(K key, F field)
		{
			try
			{
				var stringKey = JsonConvert.SerializeObject(key);
				var stringField = JsonConvert.SerializeObject(field);

				return await database.HashDeleteAsync(stringKey, stringField);
			}
			catch (Exception)
			{
				return false;
			}
		}

		public virtual async Task<V> HashGetAsync<K, F, V>(K key, F field)
		{
			try
			{
				var stringKey = JsonConvert.SerializeObject(key);
				var stringField = JsonConvert.SerializeObject(field);

				var result = await database.HashGetAsync(stringKey, stringField);

				return JsonConvert.DeserializeObject<V>(result);
			}
			catch (Exception)
			{
				return default;
			}
		}

		public virtual async Task<long> ListAddAsync<K, M>(K key, M member, int expire = 0)
		{
			try
			{
				var setting = new JsonSerializerSettings()
				{
					PreserveReferencesHandling = PreserveReferencesHandling.Objects,
					ReferenceLoopHandling = ReferenceLoopHandling.Serialize
				};

				var stringKey = JsonConvert.SerializeObject(key);
				var stringMember = JsonConvert.SerializeObject(member, setting);

				var result = await database.ListRightPushAsync(stringKey, stringMember);

				if (expire > 0)
				{
					await database.KeyExpireAsync(stringKey, TimeSpan.FromSeconds(expire));
				}

				return result;
			}
			catch (Exception)
			{
				return -1;
			}
		}

		public virtual async Task<long> ListRemoveAsync<K, M>(K key, M member)
		{
			try
			{
				var stringKey = JsonConvert.SerializeObject(key);
				var stringMember = JsonConvert.SerializeObject(member);

				var result = await database.ListRemoveAsync(stringKey, stringMember);

				return result;
			}
			catch (Exception)
			{
				return -1;
			}
		}

		public virtual async Task<long> ListLengthAsync<K>(K key)
		{
			try
			{
				var stringKey = JsonConvert.SerializeObject(key);

				var result = await database.ListLengthAsync(stringKey);

				return result;
			}
			catch (Exception)
			{
				return -1;
			}
		}

		public virtual async Task<List<V>> ListRangeAsync<K, V>(K key, int start, int end)
		{
			try
			{
				var stringKey = JsonConvert.SerializeObject(key);
				var response = await database.ListRangeAsync(stringKey, start, end);

				var result = new List<V>();
				foreach (var item in response)
				{
					var temp = JsonConvert.DeserializeObject<V>(item);
					result.Add(temp);
				}
				return result;
			}
			catch (Exception)
			{
				return new List<V>();
			}
		}

		public virtual async Task<bool> SetAddAsync<K, V>(K key, V value, int expire = 0)
		{
			try
			{
				var stringKey = JsonConvert.SerializeObject(key);
				var stringValue = JsonConvert.SerializeObject(value);

				var result = await database.SetAddAsync(stringKey, stringValue);

				if (expire > 0)
				{
					await database.KeyExpireAsync(stringKey, TimeSpan.FromSeconds(expire));
				}

				return result;
			}
			catch (Exception)
			{
				return false;
			}
		}

		public virtual async Task<bool> SetRemoveAsync<K, V>(K key, V value)
		{
			try
			{
				var stringKey = JsonConvert.SerializeObject(key);
				var stringValue = JsonConvert.SerializeObject(value);

				var result = await database.SetRemoveAsync(stringKey, stringValue);
				return result;
			}
			catch (Exception)
			{
				return false;
			}
		}

		public virtual async Task<List<V>> SetAllAsync<K, V>(K key)
		{
			try
			{
				var stringKey = JsonConvert.SerializeObject(key);
				var list = new List<V>();
				var result = await database.SetMembersAsync(stringKey);
				foreach (var item in result)
				{
					var temp = JsonConvert.DeserializeObject<V>(item);
					list.Add(temp);
				}
				return list;
			}
			catch (Exception)
			{
				return new List<V>();
			}
		}

		public virtual async Task<long> SetLengthAsync<K>(K key)
		{
			try
			{
				var stringKey = JsonConvert.SerializeObject(key);
				var result = await database.SetLengthAsync(stringKey);

				return result;
			}
			catch (Exception)
			{
				return -1;
			}
		}

		public virtual async Task<bool> SetContainsValueAsync<K, V>(K key, V value)
		{
			try
			{
				var stringKey = JsonConvert.SerializeObject(key);
				var stringValue = JsonConvert.SerializeObject(value);

				var result = await database.SetContainsAsync(stringKey, stringValue);
				return result;
			}
			catch (Exception)
			{
				return false;
			}
		}

		#endregion

		#region Lock


		private RedisValue LockToken = "Cube";
		public bool LockTake(string lockKey)
		{
			return database.LockTake(lockKey, LockToken, TimeSpan.FromSeconds(30));
		}

		public bool LockRelease(string lockKey)
		{
			return database.LockRelease(lockKey, LockToken);
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
