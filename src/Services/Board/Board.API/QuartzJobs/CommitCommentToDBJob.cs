using Cube.Board.Application;
using Microsoft.Extensions.Options;
using Quartz;
using System.Threading.Tasks;

namespace Board.API.QuartzJobs
{
	public class CommitCommentToDBJob : IJob
	{
		private readonly IBoardAppService _boardAppService;
		public CommitCommentToDBJob(IBoardAppService boardAppService)
		{
			_boardAppService = boardAppService;
		}

		public async Task Execute(IJobExecutionContext context)
		{
			await _boardAppService.CommitCommentToDB();
		}

	}
}
