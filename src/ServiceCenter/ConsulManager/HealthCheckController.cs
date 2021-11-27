﻿using Microsoft.AspNetCore.Mvc;
using HttpGetAttribute = Microsoft.AspNetCore.Mvc.HttpGetAttribute;
using RouteAttribute = Microsoft.AspNetCore.Mvc.RouteAttribute;

namespace ConsulManager
{
	[Route("[controller]")]
	[ApiController]
	public class HealthCheckController : ControllerBase
	{
		[HttpGet]
		public IActionResult Get()
		{
			return Ok();
		}
	}
}
