using AutoMapper;
using Cube.Board.Application.Dtos;
using Cube.Board.Domain;

namespace Cube.Board.Application.Configuration
{
	public class BoardProfile : Profile
	{
		public BoardProfile()
		{
			CreateMap<DisscussionBoard, BoardDto>();
			CreateMap<DisscussionBoardItem, BoardItemDto>();
		}
	}
}
