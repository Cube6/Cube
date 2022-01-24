using Cube.Board.Application.Dtos;
using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace Board.API.Hubs
{
	public class BoardHub : Hub
	{
		public async Task SendBoardMessage()
		{
			await Clients.All.SendAsync("ReceiveBoardMessage");
		}

		public async Task SendBoardItemMessage(BoardItemEvent boardItemEvent)
		{
			await Clients.All.SendAsync("ReceiveBoardItemMessage", boardItemEvent);
		}

		public async Task SendCommentMessage(CommentEvent commentEvent)
		{
			await Clients.All.SendAsync("ReceiveCommentMessage", commentEvent);
		}

		//// 常用方法
		//// 给所有人发送消息
		//await Clients.All.SendAsync("ReceiveMessage", data);
		//// 给组里所有人发消息
		//await Clients.Group("Users").SendAsync("ReceiveMsg", data);
		//// 给调用方法的那个人发消息
		//await Clients.Caller.SendAsync("ReceiveMessage", data);
		//// 给除了调用方法的以外所有人发消息
		//await Clients.Others.SendAsync("ReceiveMessage", data);
		//// 给指定connectionId的人发消息
		//await Clients.User(connectionId).SendAsync("ReceiveMessage", data);
		//// 给指定connectionId的人发消息
		//await Clients.Client(connectionId).SendAsync("ReceiveMessage", data);
		//// 给指定connectionId的人发消息，同时指定多个connectionId
		//await Clients.Clients(IReadOnlyList <> connectionIds).SendAsync("ReceiveMessage", data);
	}
}
