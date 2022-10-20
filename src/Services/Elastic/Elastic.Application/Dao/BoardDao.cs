using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elastic.Application.Dao
{
	public record BoardDao : EntityDao
	{
		public string CreateUser { get; set; }
		public string Name => Keyword;
		public int State { get; set; }
		public bool IsDeleted { get; set; }
	}
}
