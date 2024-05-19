using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using InternJohan.Dev.Infrastructure.Configuration;
using InternJohan.Dev.Infrastructure.Models;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;

namespace InternJohan.Dev.Infrastructure.Repository
{
    public class RoleRepository
    {
        private readonly DatabaseSettings _databaseSettings;

        public RoleRepository(IOptions<DatabaseSettings> databaseSettings)
        {
            _databaseSettings = databaseSettings.Value;
        }

        public async Task<IEnumerable<Role>> FindAll()
        {
            using var connection = new SqlConnection(_databaseSettings.DefaultConnection);
            var query = @"
                SELECT r.*, u.*
                FROM Roles r
                LEFT JOIN Users u ON r.Id = u.RoleId";

            var roleUserMapping = new Dictionary<int, Role>();

            // Använd en multi mapping för att mappa roller och användare
            var rolesWithUsers = await connection.QueryAsync<Role, User, Role>(
                query,
                (role, user) =>
                {
                    if (!roleUserMapping.ContainsKey(role.Id))
                    {
                        roleUserMapping[role.Id] = role;
                    }

                    if (user != null)
                    {
                        roleUserMapping[role.Id].Users.Add(user);
                    }

                    return roleUserMapping[role.Id];
                },
                splitOn: "Id"
            );

            return roleUserMapping.Values;
        }
        // Metod för att hämta en roll efter dess namn
        public async Task<Role> FindByName(string name)
        {
            using var connection = new SqlConnection(_databaseSettings.DefaultConnection);
            var query = "SELECT * FROM Roles WHERE Name = @Name";

            // Använd Dapper för att utföra frågan och hämta rollen
            return await connection.QueryFirstOrDefaultAsync<Role>(query, new { Name = name });
        }

        public async Task<Role> FindById(int id)
        {
            using var connection = new SqlConnection(_databaseSettings.DefaultConnection);
            var query = @"
                SELECT r.*, u.*
                FROM Roles r
                LEFT JOIN Users u ON r.Id = u.RoleId
                WHERE r.Id = @Id";

            var roleUserMapping = new Dictionary<int, Role>();

            // Använd multi mapping för att mappa rollen och användare
            var roleWithUsers = await connection.QueryAsync<Role, User, Role>(
                query,
                (role, user) =>
                {
                    if (!roleUserMapping.ContainsKey(role.Id))
                    {
                        roleUserMapping[role.Id] = role;
                    }

                    if (user != null)
                    {
                        roleUserMapping[role.Id].Users.Add(user);
                    }

                    return roleUserMapping[role.Id];
                },
                param: new { Id = id },
                splitOn: "Id"
            );

            return roleUserMapping.Values.FirstOrDefault();
        }
    }
}
