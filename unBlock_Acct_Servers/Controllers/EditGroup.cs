using Microsoft.AspNetCore.Mvc;
using unBlock_Acct_Servers.Models;
using unBlock_Acct_Servers.Models.Request;

namespace unBlock_Acct_Servers.Controllers
{
    [ApiController] //mark a class as a web API controller
    public class EditGroup : Controller
    {
        [HttpPost("edit/group")]
        public async Task<IActionResult> addGroup(
            [FromQuery(Name = "groupId")] int groupId, // request url query parameter
            [FromBody] AddGroupBody body // request body
        )
        {
            int effectedRows = await Queries.EditGroup(body.Name, groupId);
            if (effectedRows <= 0) return StatusCode(200, "failed");
            for (int i = 0; i < body.MemberEmails.Count; i++)
            {
                string MemberEmail = body.MemberEmails[i];
                Queries.AddGroupMember(MemberEmail, groupId, false);
            }
            for (int i = 0; i < body.AdminEmails.Count; i++)
            {
                string AdminEmail = body.AdminEmails[i];
                Queries.AddGroupMember(AdminEmail, groupId, true);

            }
            for (int i = 0; i < body.Tags.Count; i++)
            {
                string Tag = body.Tags[i];
                Queries.AddGroupTag(groupId, Tag);
            }
            return StatusCode(200, "success");

        }
    }
}
