using Microsoft.AspNetCore.Mvc;

namespace unBlock_Acct_Servers.Controllers
{
    [ApiController] //mark a class as a web API controller
    public class GetUserGroups : ControllerBase
    {
        [HttpGet("user/groups")]
        public IActionResult LoginAzure(){
            return StatusCode(200, "hello");
        }
    }
}
