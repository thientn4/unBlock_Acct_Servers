namespace unBlock_Acct_Servers.Models.Object
{
    public class Member
    {
        public Member(int inGroupId, string inEmail, bool inIsAdmin)
        {
            GroupId = inGroupId;
            Email = inEmail;
            IsAdmin = inIsAdmin;
        }

        public int GroupId { get; set; }
        public string Email { get; set; }

        public bool IsAdmin { get; set; }

    }
}
