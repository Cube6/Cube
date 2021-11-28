using AutoMapper;
using Cube.Board.Application.Dtos;
using Cube.Board.Domain;

namespace Cube.Board.Application.Util
{
	public class MapperFactory
	{
		public static IMapper GetMapper()
		{
			//Initialize the mapper
			var config = new MapperConfiguration(cfg => cfg.AddProfile<BoardApiProfile>());
			return new Mapper(config);
		}
	}
}
