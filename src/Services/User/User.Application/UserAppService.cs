using AutoMapper;
using Cube.User.Application.Configuration;
using Cube.User.Respository;

namespace Cube.User.Application
{
	public class UserAppService : IUserAppService
	{
		private IUserRepository _repository = new UserRepository();
		private IMapper _mapper = MapperFactory.GetMapper();
	}
}
