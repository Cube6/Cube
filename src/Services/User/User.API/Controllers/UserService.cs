using AutoMapper;
using Cube.User.API.Protos;
using Cube.User.Application;
using Cube.User.Application.Dtos;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;
using static Cube.User.API.Protos.UserService;

namespace Cube.User.API.Controllers
{
	public class UserService : UserServiceBase
	{
		private readonly IUserAppService _appService;
		private readonly IMapper _mapper;

		public UserService(IUserAppService appService, IMapper mapper)
		{
			_appService = appService;
			_mapper = mapper;
		}

		public override async Task<Result> Register(CreateUserRequest request, ServerCallContext context)
		{
			var dto = _mapper.Map<CreateUserDto>(request);

			var result = await _appService.Register(dto);

			return await Task.FromResult(_mapper.Map<Result>(result));
		}

		public override async Task<Result> Validate(ValidateUserRequest request, ServerCallContext context)
		{
			var dto = _mapper.Map<ValidateUserDto>(request);

			var result = await _appService.Validate(dto);

			return await Task.FromResult(_mapper.Map<Result>(result));
		}

		public override async Task<AllUsers> GetAllAsync(Empty request, ServerCallContext context)
		{
			var users = await _appService.GetUsersAsync();

			var allUsers = new AllUsers();
			foreach (var user in users)
			{
				allUsers.Users.Add(_mapper.Map<Protos.User>(user));
			}

			return await Task.FromResult(allUsers);
		}

		public override async Task<Protos.User> FindUserByIdAsync(Id request, ServerCallContext context)
		{
			var userPo = await _appService.FindUserByIdAsync(request.Id_);

			var userDto = _mapper.Map<Protos.User>(userPo);

			return userDto;
		}

		public override async Task<Result> DeleteUserByIdAsync(Id request, ServerCallContext context)
		{
			var result = await _appService.DeleteUserByIdAsync(request.Id_);
			return await Task.FromResult(_mapper.Map<Result>(result));
		}

		[Authorize]
		public override async Task<Result> SecureActionAsync(Empty request, ServerCallContext context)
		{
			return await Task.FromResult(new Protos.Result() { Success = true });
		}
	}
}
