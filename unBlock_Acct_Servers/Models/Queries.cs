using unBlock_Acct_Servers.Models.Object;
using MySqlConnector;
using System.Data;

namespace unBlock_Acct_Servers.Models
{
    public class Queries
    {
        public static async Task<List<Group>> GetUserGroups(string Email)
        {
            List<Group> results = new List<Group>();
            using (var connection = new MySqlConnection("Server=localhost;Port=3306;Uid=root;Pwd=Ntmtrung1973@;Database=UNBLOCK"))
            {
                await connection.OpenAsync();

                using (var command = new MySqlCommand($"CALL UNBLOCK.GET_USER_GROUPS(\"{Email}\")", connection))
                {
                    var reader = await command.ExecuteReaderAsync();

                    while (await reader.ReadAsync())
                    {
                        int id = reader.GetInt32(0);
                        string name = reader.GetString(1);
                        string ownerEmail = reader.GetString(2);
                        bool isAdmin = reader.GetBoolean(3);

                        results.Add(new Group(id, name, ownerEmail, isAdmin));
                    }
                }
            }
            return results;
        }
        public static async Task<List<Member>> GetGroupMembers(int groupId)
        {
            List<Member> results = new List<Member>();
            using (var connection = new MySqlConnection("Server=localhost;Port=3306;Uid=root;Pwd=Ntmtrung1973@;Database=UNBLOCK"))
            {
                await connection.OpenAsync();

                using (var command = new MySqlCommand($"SELECT * FROM UNBLOCK.MEMBERSHIP WHERE GROUP_ID = {groupId}", connection))
                {
                    var reader = await command.ExecuteReaderAsync();

                    while (await reader.ReadAsync())
                    {
                        int id = reader.GetInt32(1);
                        string email = reader.GetString(0);
                        bool isAdmin = reader.GetBoolean(2);
                        results.Add(new Member(id,email,isAdmin));
                    }
                }
            }
            return results;
        }
        public static async Task<int> AddGroup(string Name, string OwnerEmail)
        {
            using (var connection = new MySqlConnection("Server=localhost;Port=3306;Uid=root;Pwd=Ntmtrung1973@;Database=UNBLOCK"))
            {
                await connection.OpenAsync();
                using (var command = new MySqlCommand($"CALL UNBLOCK.ADD_GROUP(\"{Name}\",\"{OwnerEmail}\")", connection))
                {
                    try
                    {
                        var reader = await command.ExecuteReaderAsync();
                        await reader.ReadAsync();
                        return reader.GetInt32(0);
                    }
                    catch (Exception)
                    {
                        return -1;
                    }
                }
            }
        }
        public static async Task<int> EditGroup(string Name, int GroupId)
        {
            using (var connection = new MySqlConnection("Server=localhost;Port=3306;Uid=root;Pwd=Ntmtrung1973@;Database=UNBLOCK"))
            {
                await connection.OpenAsync();
                using (var command = new MySqlCommand($"CALL UNBLOCK.EDIT_GROUP(\"{Name}\",{GroupId})", connection))
                {
                    try
                    {
                        return command.ExecuteNonQuery();
                    }
                    catch (Exception)
                    {
                        return -1;
                    }
                }
            }
        }
        public static async void AddGroupMember(string MemberEmail, int GroupID, bool IsAdmin)
        {
            using (var connection = new MySqlConnection("Server=localhost;Port=3306;Uid=root;Pwd=Ntmtrung1973@;Database=UNBLOCK"))
            {
                await connection.OpenAsync();
                using (var command = new MySqlCommand($"CALL UNBLOCK.ADD_GROUP_MEMBER(\"{MemberEmail}\",\"{GroupID}\",{IsAdmin})", connection))
                {
                    try
                    {
                        command.ExecuteNonQuery();
                    }
                    catch (Exception)
                    {

                    }
                }
            }
        }
        public static async void AddGroupTag( int GroupID, string Tag)
        {
            using (var connection = new MySqlConnection("Server=localhost;Port=3306;Uid=root;Pwd=Ntmtrung1973@;Database=UNBLOCK"))
            {
                await connection.OpenAsync();
                using (var command = new MySqlCommand($"CALL UNBLOCK.ADD_GROUP_TAG(\"{Tag}\",\"{GroupID}\")", connection))
                {
                    try
                    {
                        command.ExecuteNonQuery();
                    }
                    catch (Exception)
                    {

                    }
                }
            }
        }
        public static async Task<int> DeleteGroup(int GroupId, string OwnerEmail)
        {
            var effectedRows = 0;
            using (var connection = new MySqlConnection("Server=localhost;Port=3306;Uid=root;Pwd=Ntmtrung1973@;Database=UNBLOCK"))
            {
                await connection.OpenAsync();
                using (var command = new MySqlCommand($"CALL UNBLOCK.DELETE_GROUP(\"{OwnerEmail}\",\"{GroupId}\")", connection))
                {
                    try
                    {
                        effectedRows = command.ExecuteNonQuery();
                    }
                    catch (Exception)
                    {
                        effectedRows = 0;
                    }
                }
            }
            return effectedRows;
        }
        public static async Task<int> DeleteGroupMember(int GroupId, string Email)
        {
            var effectedRows = 0;
            using (var connection = new MySqlConnection("Server=localhost;Port=3306;Uid=root;Pwd=Ntmtrung1973@;Database=UNBLOCK"))
            {
                await connection.OpenAsync();
                using (var command = new MySqlCommand($"CALL UNBLOCK.DELETE_GROUP_MEMBER(\"{Email}\",\"{GroupId}\")", connection))
                {
                    try
                    {
                        effectedRows = command.ExecuteNonQuery();
                    }
                    catch (Exception)
                    {
                        effectedRows = 0;
                    }
                }
            }
            return effectedRows;
        }
    }
}
