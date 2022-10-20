using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elastic.Application.Dao
{
	public record BoardItemDao : EntityDao
	{
		public int BorderId { get; set; }
		public string CreateUser { get; set; }
		public string Detail => Keyword;
		public string Action { get; set; }
		public BoardItemType Type { get; set; }
		public BoardItemState State { get; set; }
	}

	public enum BoardItemType
	{
		WentWell,
		NeedsImprovded,
		Action
	}

	public enum BoardItemState
	{
		None,
		InProgress,
		Done,
	}
}
