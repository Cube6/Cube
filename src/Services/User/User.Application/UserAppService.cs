using AutoMapper;
using Cube.User.Application.Configuration;
using Cube.User.Application.Dtos;
using Cube.User.Respository;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Cube.User.Application
{
	public class UserAppService : IUserAppService
	{
		private IUserRepository _repository;
		private IMapper _mapper = MapperFactory.GetMapper();

		public UserAppService(IUserRepository repository)
		{
			_repository = repository;
		}

		public async Task<ResultDto> Register(CreateUserDto request)
		{
			var user = _mapper.Map<Domain.User>(request); 

			await _repository.Save(user);

			return await Task.FromResult(new ResultDto() { Success = true });
		}

		public async Task<ResultDto> Validate(ValidateUserDto request)
		{
			var result = await _repository.Exist(request.Name, request.Password);

			return await Task.FromResult(new ResultDto() { Success = result });
		}

		public async Task<List<UserDto>> GetUsersAsync()
		{
			var users = await _repository.ListAsync();

			var allUsers = new List<UserDto>();
			foreach (var user in users)
			{
				allUsers.Add(_mapper.Map<UserDto>(user));
			}

			return await Task.FromResult(allUsers);
		}

		public async Task<UserDto> FindUserByIdAsync(long id)
		{
			var userPo = await _repository.GetUserByIdAsync(id);

			var userDto = _mapper.Map<UserDto>(userPo);

			return userDto;
		}

		public async Task<ResultDto> DeleteUserByIdAsync(long id)
		{
			return await Task.FromResult(new ResultDto() { Success = true });
		}
	}
}
