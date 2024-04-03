using unBlock_Acct_Servers.Models.Object;

namespace unBlock_Acct_Servers.Models.Response
{
    public class GetGroupMembersRes
    {
        public GetGroupMembersRes()
        {
            this.Status = "failed";
        }
        public GetGroupMembersRes(string inStatus, List<Member> inMembers)
        {
            this.Status = inStatus;
            this.Members = inMembers;
        }
        public List<Member>? Members { get; set; }
        public string? Status { get; set; }
    }
}
