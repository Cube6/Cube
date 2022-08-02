using Elastic.Application;
using Microsoft.AspNetCore.Mvc;
using Elastic.Application.IntegrationEvents.Events.SearchEvents.Response;
using Elastic.Application.IntegrationEvents.Events.SearchEvents.Request;

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

		[HttpPut]
		[Route(nameof(GlobalInfoSearch))]
		public async Task<GlobalInfoSearchResponseEvent> GlobalInfoSearch(GlobalInfoSearchRequestEvent request)
		{
			return await _service.GlobalInfoSearchAsync(request);
		}

	}
}