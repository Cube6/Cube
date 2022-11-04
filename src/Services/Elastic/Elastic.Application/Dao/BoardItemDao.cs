using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elastic.Application.Dao
{
	public record BoardItemDao : EntityDao
	{
		public int BoardId { get; set; }
		public string CreateUser { get; set; }
		public string Detail => Keyword;
		public string Action { get; set; }
		public int Type { get; set; }
		public int State { get; set; }
	}
}
