using Microsoft.AspNetCore.Mvc;
using unBlock_Acct_Servers.Models;
using unBlock_Acct_Servers.Models.Request;

namespace unBlock_Acct_Servers.Controllers
{
    [ApiController] //mark a class as a web API controller
    public class RemoveGroup : Controller
    {
        [HttpPost("remove/group")]
        public async Task<IActionResult> removeGroup(
            [FromHeader(Name = "email")] string email, // request header
            [FromQuery(Name = "groupId")] int groupId // request url query parameter
        )
        {

            int effectedRows = await Queries.DeleteGroup(groupId, email);
            if (effectedRows > 0) { return StatusCode(200, "success"); }
            effectedRows = await Queries.DeleteGroupMember(groupId, email);
            if (effectedRows > 0) { return StatusCode(200, "success"); }
            return StatusCode(200, "failed");
        }
    }
}
