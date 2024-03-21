using unBlock_Acct_Servers.Models.Object;
using MySqlConnector;

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
    }
}
