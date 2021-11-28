using AutoMapper;
using Cube.User.Application.Dtos;

namespace Cube.User.Application.Configuration
{
	public class UserProfile : Profile
	{
		public UserProfile()
		{
			CreateMap<Domain.User, UserDto>();
		}
	}
}
