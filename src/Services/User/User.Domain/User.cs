using Cube.Data;

namespace Cube.User.Domain
{
	public class User : Entity
	{
		public string Name { get; set; }
		public string Password { get; set; }
		public string NickName { get; set; }
		public string AvatarUrl { get; set; }
	}
}
