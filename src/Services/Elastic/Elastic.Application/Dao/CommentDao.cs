using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elastic.Application.Dao
{
	public record CommentDao : EntityDao
	{
		public int BoardItemId { get; set; }
		public CommentType Type { get; set; }
		public string Detail { get; set; }
	}

	public enum CommentType
	{
		ThumbsUp,
		Message
	}
}
