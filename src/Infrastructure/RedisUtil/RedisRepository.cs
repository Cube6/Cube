using System;
using System.Threading;
using System.Reactive;
using System.Threading.Tasks;
using StackExchange.Redis;
using Microsoft.VisualStudio.Threading;

namespace Cube.Infrastructure.Redis
{
	internal abstract class RedisRepository : IDisposable
	{
		#region Events

		public event EventHandler<InternalErrorEventArgs> InternalErrorHandler;
		public event EventHandler<RedisErrorEventArgs> ErrorEventHandler;
		public event EventHandler<ConnectionFailedEventArgs> ConnectionFailedHandler;
		public event EventHandler<ConnectionFailedEventArgs> ConnectionRestoredHandler;

		public void OnInternalError(object sender, InternalErrorEventArgs args)
		{
			InternalErrorHandler?.Invoke(sender, args);
		}

		public void OnErrorEvent(object sender, RedisErrorEventArgs args)
		{
			ErrorEventHandler?.Invoke(sender, args);
		}

		public void OnConnectionFailed(object sender, ConnectionFailedEventArgs args)
		{
			ConnectionFailedHandler?.Invoke(sender, args);
		}

		public void OnConnectionRestore(object sender, ConnectionFailedEventArgs args)
		{
			ConnectionRestoredHandler?.Invoke(sender, args);
		}

		#endregion

		#region Properties and Fields

		public bool Disposed => disposed;
		private bool disposed = true;

		//TODO : ReaderWriterLock can not span await statement.
		protected AsyncReaderWriterLock disposeLock;
		protected ConnectionMultiplexer multiplexer;
		protected IDatabase database => multiplexer.GetDatabase();

		#endregion

		public RedisRepository()
		{
			disposeLock = new AsyncReaderWriterLock();
		}

		#region Public Methods
		public async Task InitializeAsync(string connectionString)
		{
			AsyncReaderWriterLock.Releaser upLockReleaser = default, writeLockReleaser = default;
			try
			{
				upLockReleaser = await disposeLock.UpgradeableReadLockAsync();

				if (Disposed)
				{
					writeLockReleaser = await disposeLock.WriteLockAsync();

					multiplexer = await ConnectionMultiplexer.ConnectAsync(connectionString);

					multiplexer.InternalError += OnInternalError;
					multiplexer.ErrorMessage += OnErrorEvent;
					multiplexer.ConnectionFailed += OnConnectionFailed;
					multiplexer.ConnectionRestored += OnConnectionRestore;

					disposed = false;
				}
			}
			finally
			{
				if (disposeLock.IsWriteLockHeld)
					await writeLockReleaser.ReleaseAsync();

				if (disposeLock.IsUpgradeableReadLockHeld)
					await upLockReleaser.ReleaseAsync();
			}
		}

		public void Dispose()
		{
			DisposeAsync().Wait();
		}
		public async Task DisposeAsync()
		{
			AsyncReaderWriterLock.Releaser upLockReleaser = default, writeLockReleaser = default;
			try
			{
				upLockReleaser = await disposeLock.UpgradeableReadLockAsync();
				if (!disposed)
				{
					writeLockReleaser = await disposeLock.WriteLockAsync();

					multiplexer.InternalError -= OnInternalError;
					multiplexer.ErrorMessage -= OnErrorEvent;
					multiplexer.ConnectionFailed -= OnConnectionFailed;
					multiplexer.ConnectionRestored -= OnConnectionRestore;

					multiplexer.Dispose();
					disposed = true;
				}
			}
			finally
			{
				if (disposeLock.IsWriteLockHeld)
					await writeLockReleaser.ReleaseAsync();
				if (disposeLock.IsUpgradeableReadLockHeld)
					await upLockReleaser.ReleaseAsync();
			}
		}

		#endregion

		#region Private Methods
		private async Task CheckDisposeAsync()
		{
			AsyncReaderWriterLock.Releaser readLockReleaser = default;
			try
			{
				readLockReleaser = await disposeLock.ReadLockAsync();
				if (disposed)
					throw new ObjectDisposedException(nameof(multiplexer));
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
