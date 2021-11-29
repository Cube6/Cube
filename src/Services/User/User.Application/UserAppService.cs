﻿using AutoMapper;
using Cube.User.Application.Configuration;
using Cube.User.Application.Dtos;
using Cube.User.Respository;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Cube.User.Application
{
	public class UserAppService : IUserAppService
	{
		private IUserRepository _repository = new UserRepository();
		private IMapper _mapper = MapperFactory.GetMapper();


		public async Task<ResultDto> Register(CreateUserDto request)
		{
			var user = _mapper.Map<Domain.User>(request); 

			await _repository.Save(user);

			return await Task.FromResult(new ResultDto() { Success = true });
		}

		public async Task<List<UserDto>> GetAllAsync()
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