using Cube.Board.Application;
using Quartz;
using System;
using System.Threading.Tasks;

namespace Board.API.QuartzJobs
{
	public class PersistCommentJob : IJob
	{
		public PersistCommentJob()
		{
		}

		public async Task Execute(IJobExecutionContext context)
		{
			Console.WriteLine("quartz is invoked " + DateTime.Now);
		}
	}
}
