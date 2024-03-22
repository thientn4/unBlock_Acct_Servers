namespace unBlock_Acct_Servers.Models.Request
{
    public class AddGroupBody
    {
        public string? Name { get; set; }
        public List<string>? MemberEmails { get; set; }
        public List<string>? AdminEmails { get; set; }
        public List<string>? Tags { get; set; }

    }
}
