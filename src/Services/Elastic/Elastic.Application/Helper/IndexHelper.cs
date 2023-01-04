using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elastic.Application.Helper
{
	public static class IndexHelper
	{
		public static string BoardIndex { get; set; }
		public static string BoardItemIndex { get; set; }
		public static string CommentIndex { get; set; }
		public static string UserActionIndex { get; set; }
	}
}
