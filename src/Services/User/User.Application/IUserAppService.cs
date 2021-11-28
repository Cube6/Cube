using Cube.User.Application.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Cube.User.Application
{
	public interface IUserAppService
	{
		Task<ResultDto> Register(CreateUserDto request);
		Task<List<UserDto>> GetAllAsync();
		Task<UserDto> FindUserByIdAsync(long id);
		Task<ResultDto> DeleteUserByIdAsync(long id);
	}
}