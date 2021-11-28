using AutoMapper;
using Cube.User.API.Protos;
using Cube.User.Application;
using Cube.User.Respository;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;
using static Cube.User.API.Protos.UserService;

namespace Cube.User.API.Controllers
{
	public class UserService : UserServiceBase
	{
		private readonly IUserRepository _userRepository;
		private readonly IUserAppService _appService;
		private readonly IMapper _mapper;

		public UserService(IUserRepository repository, IUserAppService appService, IMapper mapper)
		{
			_userRepository = repository;
			_appService = appService;
			_mapper = mapper;
		}

		public override async Task<Result> Register(CreateUserRequest request, ServerCallContext context)
		{
			var user = new Domain.User
			{
				Name = request.Name,
				Password = request.Password,
			};

			await _userRepository.Save(user);

			return await Task.FromResult(new Protos.Result() { Success = true });
		}

		public override async Task<AllUsers> GetAllAsync(Empty request, ServerCallContext context)
		{
			var users = new AllUsers();

			for (int i = 0; i < 10; i++)
			{
				users.Users.Add(new Protos.User
				{
					Name = "1234",
					AvatarUrl = "test"
				});
			}

			return await Task.FromResult(users);
		}

		public override async Task<Protos.User> FindUserByIdAsync(Id request, ServerCallContext context)
		{
			var userPo = await _userRepository.GetUserByIdAsync(request.Id_);

			var userDto = _mapper.Map<Protos.User>(userPo);

			return userDto;
		}

		public override async Task<Result> DeleteUserByIdAsync(Id request, ServerCallContext context)
		{
			return await Task.FromResult(new Result() { Success = true });
		}

		[Authorize]
		public override async Task<Result> SecureActionAsync(Empty request, ServerCallContext context)
		{
			return await Task.FromResult(new Protos.Result() { Success = true });
		}
	}
}
