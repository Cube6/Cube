using System.Collections.Generic;

namespace Cube.GatewayService.Responses
{
    public class Response
    {
        public bool Succeed = false;

        public string Message = string.Empty;

        public List<string> Errors = new List<string>();
    }
}
