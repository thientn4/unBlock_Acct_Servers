namespace unBlock_Acct_Servers.Models.Object
{
    public class Group
    {
        public Group(string inId, string inName, string inOwnerEmail, bool inIsAdmin)
        {
            Id = inId;
            Name = inName;
            OwnerEmail = inOwnerEmail;
            IsAdmin = inIsAdmin;
        }
        public string Id { get; set; }
        public string? Name { get; set; }
        public string? OwnerEmail { get; set; }
        public bool ? IsAdmin { get; set; }

    }
}
