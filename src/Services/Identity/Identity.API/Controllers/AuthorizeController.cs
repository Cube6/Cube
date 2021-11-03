using Identity.API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Identity.API.Controllers
{
	[Route("api/[controller]")]
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
				DateTime.Now.AddMinutes(30),
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

		//[HttpPost(Name = "Token")]
		//public IActionResult Token([FromBody]LoginViewModel login)
		//{
		//	if(ModelState.IsValid)
		//	{
		//		if(!(login.User=="michael" && login.Password== "123456"))
		//		{
		//			return BadRequest();
		//		}

		//		var claims = new Claim[]
		//		{
		//			new Claim("name","michael"),
		//			new Claim("role","admin")
		//		};

		//		var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.SecretKey));
		//		var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

		//		//Option1
		//		var token = new JwtSecurityToken(
		//			_jwtSettings.Issuer,
		//			_jwtSettings.Audience,
		//			claims,
		//			DateTime.Now,
		//			DateTime.Now.AddMinutes(30),
		//			creds);

		//		//Option2
		//		//var descriptor = new SecurityTokenDescriptor
		//		//{
		//		//	Issuer = _jwtSettings.Issuer,
		//		//	Audience = _jwtSettings.Audience,// Audience Token的作用对象，也就是被访问的资源服务器授权标识
		//		//	Subject = new ClaimsIdentity(claims),
		//		//	NotBefore = DateTime.Now, //Token生效时间，在此之前不可用
		//		//	Expires = DateTime.Now.AddMinutes(30), //Token过期时间，在此之后不可用
		//		//	SigningCredentials = creds,
		//		//	IssuedAt = DateTime.Now //Token颁发时间
		//		//};

		////var handler = new JwtSecurityTokenHandler();
		////JwtSecurityToken token1 = handler.CreateJwtSecurityToken(descriptor);


		//		return Ok(new {
		//			token = new JwtSecurityTokenHandler().WriteToken(token),
		//			//token1 = handler.WriteToken(token1)
		//		});
		//	}

		//	return BadRequest();
		//}
	}
}
