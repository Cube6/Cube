using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cube.User.Application.Dtos
{
	public class UpdateOnlineUserDto
	{
		public int Operation { get; set; }
		public long BoardId { get; set; }
		public string Name { get; set; }
	}
}
