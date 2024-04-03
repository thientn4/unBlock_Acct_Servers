using Microsoft.AspNetCore.Mvc;
using unBlock_Acct_Servers.Models;
using unBlock_Acct_Servers.Models.Response;

namespace unBlock_Acct_Servers.Controllers
{
    [ApiController] //mark a class as a web API controller
    public class GetGroupMembers : Controller
    {
        [HttpGet("group/members")]
        public async Task<IActionResult> LoginAzureAsync(
            [FromQuery(Name = "groupId")] int groupId // request url query parameter
        )
        {
            return StatusCode(200, new GetGroupMembersRes("success", await Queries.GetGroupMembers(groupId)));
        }
    }
}
