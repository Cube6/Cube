//using Cube.Board.Application;
//using Cube.Board.Application.Events;
//using Microsoft.AspNetCore.Mvc;

//namespace Board.API.EventHandling
//{
//	public class CommentEventHandler
//	{
//		private IBoardAppService _boardAppService;

//		public CommentEventHandler([FromServices]IBoardAppService boardAppService)
//		{
//			_boardAppService = boardAppService;
//		}

//		public async void Process(CommentAddedEvent commentEvent)
//		{
//			switch (commentEvent.Operation)
//			{
//				case EventOperation.Create:
//					await _boardAppService.CreateComment(commentEvent.Comment);
//					break;
//				case EventOperation.Delete:
//					await _boardAppService.DeleteCommentAsync(commentEvent.Comment.Id);
//					break;
//				case EventOperation.Update:
//					await _boardAppService.UpdateComment(commentEvent.Comment);
//					break;
//				default:
//					break;
//			}
//		}
//	}
//}
