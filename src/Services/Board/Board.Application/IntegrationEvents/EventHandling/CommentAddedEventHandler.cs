using Cube.Board.Application.IntegrationEvents.Events;
using Cube.Board.Domain;
using Cube.Board.Respository;
using Cube.BuildingBlocks.EventBus.Abstractions;
using System;
using System.Threading.Tasks;

namespace Cube.Board.Application.IntegrationEvents.EventHandling;

public class CommentAddedEventHandler : IIntegrationEventHandler<CommentAddedEvent>
{
	private IBoardRepository _repository;
	public CommentAddedEventHandler(IBoardRepository repository)
	{
		_repository = repository;
	}

	public async Task Handle(CommentAddedEvent @event)
	{
		var commentDto = @event.Comment;

		var comment = new Comment()
		{
			BoardItem = await _repository.GetBoardItemByIdAsync(commentDto.BoardItemId),
			Detail = commentDto.Detail,
			CreatedUser = commentDto.CreatedUser,
			DateCreated = DateTime.Now,
			DateModified = DateTime.Now,
			Type = commentDto.Type,
		};

		await _repository.CreateCommentAsync(comment);
	}
}
