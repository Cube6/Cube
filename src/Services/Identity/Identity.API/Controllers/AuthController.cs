using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Identity.API
{
	[Route("api/[controller]")]
	[Authorize]
	[ApiController]
	public class AuthController : ControllerBase
	{
		private readonly ILogger<AuthController> _logger;

		public AuthController(ILogger<AuthController> logger)
		{
			_logger = logger;
		}

		[HttpGet]
		[Authorize(Roles = "User")]
		public ActionResult Index()
		{
			return new ViewResult();
		}

		[HttpGet("{id}")]
		[Authorize(Roles = "Managers")]
		public ActionResult Manager(int id)
		{
			return new ViewResult();
		}
	}
}
