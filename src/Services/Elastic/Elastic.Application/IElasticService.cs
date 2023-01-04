using Elastic.Application.IntegrationEvents.Events.SearchEvents.Request;
using Elastic.Application.IntegrationEvents.Events.SearchEvents.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elastic.Application
{
	public interface IElasticService
	{
		public void Test();

		public Task<GlobalInfoSearchResponseEvent> GlobalInfoSearchAsync(GlobalInfoSearchRequestEvent request);
	}
}
