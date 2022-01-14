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

			CreateMap<DisscussionBoardItem, BoardItemDto>()
				.ForMember(dest => dest.BoardId, opt => opt.MapFrom(src => src.Board.Id));

			CreateMap<Comment, CommentDto>()
				.ForMember(dest => dest.BoardItemId, opt => opt.MapFrom(src => src.BoardItem.Id));
		}
	}
}
