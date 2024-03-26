using Microsoft.AspNetCore.Mvc;
using unBlock_Acct_Servers.Models;
using unBlock_Acct_Servers.Models.Response;

namespace unBlock_Acct_Servers.Controllers
{
    [ApiController] //mark a class as a web API controller
    public class GetUserGroups : ControllerBase
    {
        [HttpGet("user/groups")]
        public async Task<IActionResult> LoginAzureAsync(
            [FromHeader(Name = "email")] string email // request header
        ){
            return StatusCode(200, new GetUserGroupsRes("success", await Queries.GetUserGroups(email)));
        }
    }
}
