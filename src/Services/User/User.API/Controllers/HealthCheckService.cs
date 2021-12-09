using AutoMapper;
using Cube.User.API.Protos;
using Cube.User.Application;
using Cube.User.Application.Dtos;
using Cube.User.Respository;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;
using static Cube.User.API.Protos.UserService;
using static Cube.User.API.Protos.HealthCheckService;

namespace Cube.User.API.Controllers
{
    public class HealthCheckService : HealthCheckServiceBase
    {
        public override async Task<Result> Get(Empty request, ServerCallContext context)
        {
            return await Task.FromResult(new Protos.Result() { Success = true });
        }
    }
}
