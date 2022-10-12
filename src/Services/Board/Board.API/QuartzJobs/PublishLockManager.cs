using System.Collections.Generic;

namespace Board.API.QuartzJobs
{
	public class PublishLockManager
	{
		public static Dictionary<string, bool> Locks = new Dictionary<string, bool>();

		public static bool AcquareLock(string key)
		{
			lock(Locks)
			{
				if(Locks.TryGetValue(key, out bool result))
				{
					if(result)
					{
						Locks[key] = false;
						return true;
					}
					else
					{
						return false;
					}
				}
				else
				{
					Locks[key] = false;
					return true;
				}
			}
		}

		public static void ReleaseLock(string key)
		{
			lock (Locks)
			{
				Locks[key] = true;
			}
		}
	}
}
