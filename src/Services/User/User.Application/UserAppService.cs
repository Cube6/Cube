using AutoMapper;
using Cube.User.Application.Cache;
using Cube.User.Application.Configuration;
using Cube.User.Application.Dtos;
using Cube.User.Respository;
using Cube.Infrastructure.Redis;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Cube.User.Application
{
	public class UserAppService : IUserAppService
	{
		private IUserRepository _repository;
		private IMapper _mapper = MapperFactory.GetMapper();
		private IRedisInstance _redis;

		public UserAppService(IUserRepository repository, IRedisInstance redis)
		{
			_repository = repository;
			_redis = redis;
		}

		public async Task<ResultDto> Register(CreateUserDto request)
		{
			var user = _mapper.Map<Domain.User>(request);

			var result = user.Validate();
			if (result)
			{
				await _repository.Save(user);
			}

			return await Task.FromResult(new ResultDto() { Success = result });
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

		public async Task<List<OnlineUserNameDto>> GetOnlineUsersAsync(long boardId)
		{
			var setKey = KeyBuilder.GetOnlineUserKey(boardId);
			var userNames = await _redis.SetAllAsync<string, string>(setKey);
			var allUsers = new List<OnlineUserNameDto>();
			foreach (var user in userNames)
			{
				allUsers.Add(new OnlineUserNameDto() { Name = user});
			}

			return await Task.FromResult(allUsers);
		}

		public async Task<ResultDto> UpdateOnlineUser(UpdateOnlineUserDto request)
		{
			var setKey = KeyBuilder.GetOnlineUserKey(request.BoardId);
			if (request.Operation == 1)
			{
				if (!await _redis.SetContainsValueAsync(setKey, request.Name))
				{
					await _redis.SetAddAsync(setKey, request.Name, CacheSettings.DefaultExpiryInSecondsForUser);
				}
			}
			else
			{
				await _redis.SetRemoveAsync(setKey, request.Name);
			}

			return await Task.FromResult(new ResultDto() { Success = true });
		}
	}
}
