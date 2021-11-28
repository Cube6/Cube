using AutoMapper;

namespace Cube.User.Application.Configuration
{
	public class MapperFactory
	{
		public static IMapper GetMapper()
		{
			var config = new MapperConfiguration(cfg => cfg.AddProfile<UserProfile>());
			return new Mapper(config);
		}
	}
}
