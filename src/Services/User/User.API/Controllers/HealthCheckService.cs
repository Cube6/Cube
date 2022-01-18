using Cube.User.API.Protos;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using System.Threading.Tasks;
using static Cube.User.API.Protos.HealthCheckService;

namespace Cube.User.API.Controllers
{
	public class HealthCheckService : HealthCheckServiceBase
	{
		public override async Task<Result> Get(Empty request, ServerCallContext context)
		{
			return await Task.FromResult(new Result() { Success = true });
		}
	}
}
