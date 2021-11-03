using System.ComponentModel.DataAnnotations;

namespace Identity.API.Models
{
	public class LoginViewModel
	{
		[Required]
		public string User {  get; set; }

		[Required]
		public string Password { get; set; }
	}
}
