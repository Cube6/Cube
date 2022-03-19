using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cube.User.API.Protos;
using Domain = Cube.User.Domain;
using Cube.User.Application.Dtos;

namespace Cube.User.API.Util
{
	public class UserApiProfile : Profile
	{
		public UserApiProfile()
		{
			CreateMap<Domain.User, Protos.User>();
			CreateMap<UserDto, Protos.User>();
			CreateMap<OnlineUserNameDto,OnlineUserName>();
			CreateMap<ResultDto, Result>();
			CreateMap<CreateUserRequest, CreateUserDto>();
			CreateMap<ValidateUserRequest, ValidateUserDto>();
			CreateMap<UpdateOnlineUserRequest, UpdateOnlineUserDto>();
		}
	}
}
