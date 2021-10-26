using System.Collections.Generic;

namespace Web.Bff.DiscussionBoard.Responses
{
    public class Response
    {
        public bool Succeed = false;

        public string Message = string.Empty;

        public List<string> Errors = new List<string>();
    }
}
