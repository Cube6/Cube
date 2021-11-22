namespace Cube.Identity.API.Models
{
	public class JwtSettings
	{
		public string Issuer { get; set; }

		public string Audience { get; set; }

		public string SecretKey { get; set; }
	}
}
