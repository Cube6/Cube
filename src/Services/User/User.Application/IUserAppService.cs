﻿using Cube.User.Application.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Cube.User.Application
{
	public interface IUserAppService
	{
		Task<ResultDto> Register(CreateUserDto request);
		Task<ResultDto> Validate(ValidateUserDto request);
		Task<List<UserDto>> GetUsersAsync();
		Task<UserDto> FindUserByIdAsync(long id);
		Task<ResultDto> DeleteUserByIdAsync(long id);

		Task<List<OnlineUserNameDto>> GetOnlineUsersAsync(long boardId);
		Task<ResultDto> UpdateOnlineUser(UpdateOnlineUserDto request);
	}
}