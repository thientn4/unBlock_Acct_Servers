namespace unBlock_Acct_Servers.Models.Request
{
    public class AddGroupBody
    {
        public required string Name { get; set; }
        public required List<string> MemberEmails { get; set; }
        public required List<string> AdminEmails { get; set; }
        public required List<string> Tags { get; set; }

    }
}
