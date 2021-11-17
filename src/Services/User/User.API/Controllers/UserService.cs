using AutoMapper;
using Cube.User.API.Protos;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using System.Threading.Tasks;
using User.Respository;
using static Cube.User.API.Protos.UserService;

namespace Cube.User.API.Controllers
{
	public class UserService : UserServiceBase
	{
		private readonly IUserRepository _userRepository;
		private readonly IMapper _mapper;

		public UserService(IUserRepository userRepository, IMapper mapper)
		{
			this._userRepository = userRepository;
			this._mapper = mapper;
		}

		public override async Task<AllUsers> GetAllAsync(Empty request, ServerCallContext context)
		{
			var users = new AllUsers();

			for(int i = 0; i < 10; i++)
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
			return new Result() { Success = true };
		}
	}
}
