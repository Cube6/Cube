using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cube.User.Application.Cache
{
	internal class KeyBuilder
	{
		public static string GetOnlineUserKey(long boardId)
		{
			return "OnlineUsers:" + boardId;
		}
	}
}
