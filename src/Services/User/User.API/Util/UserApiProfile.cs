using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cube.User.API.Protos;
using Domain = User.Domain;

namespace Cube.User.API.Util
{
	public class UserApiProfile : Profile
	{
		public UserApiProfile()
		{
			CreateMap<Domain.User, Protos.User>();
		}
	}
}
