using AutoMapper;

namespace Cube.Board.Application.Configuration
{
	public class MapperFactory
	{
		public static IMapper GetMapper()
		{
			var config = new MapperConfiguration(cfg => cfg.AddProfile<BoardProfile>());
			return new Mapper(config);
		}
	}
}
