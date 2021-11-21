using Cube.Identity.API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Cube.Identity.API.Controllers
{
	[Route("[controller]")]
	[ApiController]
	public class AuthorizeController : Controller
	{
		private JwtSettings _jwtSettings;

		public AuthorizeController(IOptions<JwtSettings> jwtSetting)
		{
			_jwtSettings = jwtSetting.Value;
		}

		[HttpPost]
		public IActionResult Login(string userName, string pwd)
		{
			if (!(userName == "cube" && pwd == "123456"))
			{
				return BadRequest();
			}

			var claims = new Claim[]
			{
				new Claim(JwtRegisteredClaimNames.UniqueName,userName),
				new Claim(JwtRegisteredClaimNames.Website, "http://www.cube.com")
			};

			var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.SecretKey));
			var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

			var token = new JwtSecurityToken(
				_jwtSettings.Issuer,
				_jwtSettings.Audience,
				claims,
				DateTime.Now,
				DateTime.Now.AddMinutes(1),
				creds);

			return Ok(new {
				access_token = new JwtSecurityTokenHandler().WriteToken(token)
			});
		}

		[HttpGet]
		[Authorize]
		public IActionResult Secret()
		{
			return Ok();
		}
	}
}
