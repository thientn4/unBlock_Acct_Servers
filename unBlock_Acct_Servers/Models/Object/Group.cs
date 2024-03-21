namespace unBlock_Acct_Servers.Models.Object
{
    public class Group
    {
        public Group(int inId, string inName, string inOwnerEmail, bool inIsAdmin)
        {
            Id = inId;
            Name = inName;
            OwnerEmail = inOwnerEmail;
            IsAdmin = inIsAdmin;
        }
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? OwnerEmail { get; set; }
        public bool ? IsAdmin { get; set; }

    }
}
