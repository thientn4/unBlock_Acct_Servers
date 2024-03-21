using unBlock_Acct_Servers.Models.Object;

namespace unBlock_Acct_Servers.Models.Response
{
    public class GetUserGroupsRes
    {
        public GetUserGroupsRes()
        {
            this.Status = "failed";
        }
        public GetUserGroupsRes(string inStatus, List<Group> inGroups)
        {
            this.Status = inStatus;
            this.Groups = inGroups;
        }
        public string? Status { get; set; }
        public List<Group>? Groups { get; set; }
    }
}
