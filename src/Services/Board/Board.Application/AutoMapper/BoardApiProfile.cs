using AutoMapper;
using Cube.Board.Application.Dtos;
using Cube.Board.Domain;

namespace Cube.Board.Application.Util
{
	public class BoardApiProfile : Profile
	{
		public BoardApiProfile()
		{
			CreateMap<DisscussionBoard, BoardDto>();
			CreateMap<DisscussionBoardItem, BoardItemDto>();
		}
	}
}
