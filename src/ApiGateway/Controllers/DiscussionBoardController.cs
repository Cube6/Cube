using Microsoft.AspNetCore.Mvc;
using Web.Bff.DiscussionBoard.Responses;

namespace Web.Bff.DiscussionBoard.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DiscussionBoardController : Controller
    {
        public DiscussionBoardController()
        {

        }

        [HttpPost]
        public Response Create(string id, string name)
        {
            return new Response()
            {
                Succeed = true,
            };
        }

        [HttpGet]
        public JsonResult Get(string id)
        {
            return new JsonResult("");
        }
    }
}
