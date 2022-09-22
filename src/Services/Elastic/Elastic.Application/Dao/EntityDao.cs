using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elastic.Application.Dao
{
	public record EntityDao : BaseDao
	{
		//Like Board Id, BorderItem Id, and 
		public int EntityId { get; set; }
		public string Creator { get; set; }

	}
}
