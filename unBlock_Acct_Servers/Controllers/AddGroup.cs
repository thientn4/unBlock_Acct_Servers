using Microsoft.AspNetCore.Mvc;
using unBlock_Acct_Servers.Models;
using unBlock_Acct_Servers.Models.Request;

namespace unBlock_Acct_Servers.Controllers
{
    [ApiController] //mark a class as a web API controller
    public class AddGroup : Controller
    {
        [HttpPost("add/group")]
        public async Task<IActionResult> LoginAzureAsync(
            [FromHeader(Name = "email")] string? email, // request header
            [FromBody] AddGroupBody body // request body
        )
        {
            int effectedRows = await Queries.AddGroup(body.Name,email);
            if(effectedRows <= 0) return StatusCode(200, "failed");
            for(int i=0; i<body.MemberEmails.Count; i++)
            {
                string MemberEmail = body.MemberEmails[i];
                Queries.AddGroupMember(MemberEmail, body.Name + email, false);
            }
            for (int i = 0; i < body.AdminEmails.Count; i++)
            {
                string AdminEmail = body.AdminEmails[i];
                Queries.AddGroupMember(AdminEmail, body.Name + email, true);

            }
            for (int i = 0; i < body.Tags.Count; i++)
            {
                string Tag = body.Tags[i];
                Queries.AddGroupTag(body.Name + email, Tag);
            }
            return StatusCode(200, "success");

        }
    }
}
