using System;
using System.Threading;
using StackExchange.Redis;

namespace RedisRepository
{
	public class RedisRepository : IDisposable
	{
		protected bool dispose;
		public bool Disposed => dispose;

		protected ConnectionMultiplexer multiplexer;

		protected ReaderWriterLockSlim readerWriterLock;

		public RedisRepository(string connectionString)
		{
			readerWriterLock = new ReaderWriterLockSlim();
			multiplexer = ConnectionMultiplexer.Connect(connectionString);
			dispose = false;
		}

		public void SetString(string key, string value)
		{			
		}

		/// <summary>
		/// Dispose
		/// </summary>
		/// <exception cref="ObjectDisposedException"></exception>
		public void Dispose()
		{
			try
			{
				readerWriterLock.EnterUpgradeableReadLock();

				if (Disposed)
				{
					throw new ObjectDisposedException(nameof(multiplexer));
				}
				else
				{
					readerWriterLock.EnterWriteLock();
					multiplexer.Dispose();
				}
			}
			finally
			{
				if (readerWriterLock.IsWriteLockHeld)
					readerWriterLock.ExitWriteLock();

				if (readerWriterLock.IsUpgradeableReadLockHeld)
					readerWriterLock.ExitUpgradeableReadLock();
			}
		}
	}
}
