using Cube.Infrastructure.Repository;
using System.ComponentModel.DataAnnotations;

namespace Cube.User.Domain
{
	public class User : Entity
	{
		[Required]
		public string Name { get; set; }

		[Required]
		public string Password { get; set; }

		public string NickName { get; set; }
		public string AvatarUrl { get; set; }

		public bool Validate()
		{
			return !string.IsNullOrEmpty(Name) && !string.IsNullOrEmpty(Password);
		}
	}
}
