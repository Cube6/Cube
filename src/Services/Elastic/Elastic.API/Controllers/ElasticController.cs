using Elastic.Application;
using Microsoft.AspNetCore.Mvc;

namespace Elastic.API.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class ElasticController : ControllerBase
	{


		private readonly ILogger<ElasticController> _logger;
		private readonly IElasticService _service;

		public ElasticController(ILogger<ElasticController> logger, IElasticService service)
		{
			_logger = logger;
			_service = service;
		}

		[HttpGet]
		[Route("Test")]
		public string Test()
		{
			try
			{
				_service.Test();
				return "good";
			}catch(Exception es)
			{
				return es.ToString();
			}
		}
	}
}