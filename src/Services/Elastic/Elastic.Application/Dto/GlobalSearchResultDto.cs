using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elastic.Application.Dto
{
	public record GlobalSearchResultDto
	{
		public int	EntityId { get; set; }
		public EntityType EntityType { get; set; }
		public int BoardId { get; set; }
		public string Content { get; set; }
		public string CreationUser { get; set; }
	}

	public enum EntityType
	{
		Board,
		BoardItem,
		Comment
	}
}
