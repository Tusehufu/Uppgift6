using System.Collections.Generic;
using System.Threading.Tasks;
using Dapper;
using InternJohan.Dev.Infrastructure.Configuration;
using InternJohan.Dev.Infrastructure.Models;
using Isopoh.Cryptography.Argon2;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;

namespace InternJohan.Dev.Infrastructure.Repository
{
    public class UserRepository
    {
        private readonly DatabaseSettings _databaseSettings;

        public UserRepository(IOptions<DatabaseSettings> databaseSettings)
        {
            _databaseSettings = databaseSettings.Value;
        }

        // Metod för att hämta alla användare och deras roller
        public async Task<IEnumerable<User>> FindAllWithRoles()
        {
            using var connection = new SqlConnection(_databaseSettings.DefaultConnection);
            var query = @"
                SELECT u.*, r.*
                FROM Users u
                JOIN Roles r ON u.RoleId = r.Id";

            var userRoleMapping = await connection.QueryAsync<User, Role, User>(
                query,
                (user, role) =>
                {
                    user.Role = role;
                    return user;
                }
            );

            return userRoleMapping;
        }

        // Metod för att hämta en användare efter ID med tillhörande roll
        public async Task<User> FindByIdWithRole(int id)
        {
            using var connection = new SqlConnection(_databaseSettings.DefaultConnection);
            var query = @"
                SELECT u.*, r.*
                FROM Users u
                JOIN Roles r ON u.RoleId = r.Id
                WHERE u.Id = @Id";

            var userRoleMapping = await connection.QueryAsync<User, Role, User>(
                query,
                (user, role) =>
                {
                    user.Role = role;
                    return user;
                },
                new { Id = id }
            );

            // Returnera den första matchningen
            return userRoleMapping.FirstOrDefault();
        }

        // Övriga metoder är oförändrade, men inkluderar dem för komplett kod
        public async Task<int> Add(User user)
        {
            // Hasha användarens lösenord
            string hashedPassword = Argon2.Hash(user.Password);
            user.PasswordHash = hashedPassword;

            using var connection = new SqlConnection(_databaseSettings.DefaultConnection);
            var query = @"
                INSERT INTO Users (Username, Email, PasswordHash, RoleId, CreatedAt)
                OUTPUT INSERTED.Id
                VALUES (@Username, @Email, @PasswordHash, @RoleId, @CreatedAt)";

            int newUserId = await connection.ExecuteScalarAsync<int>(query, user);
            return newUserId;
        }

        public async Task<bool> Update(User user)
        {
            using var connection = new SqlConnection(_databaseSettings.DefaultConnection);
            var query = @"
                UPDATE Users
                SET Username = @Username, Email = @Email,
                    PasswordHash = @PasswordHash, RoleId = @RoleId, CreatedAt = @CreatedAt
                WHERE Id = @Id";

            int affectedRows = await connection.ExecuteAsync(query, user);
            return affectedRows > 0;
        }

        public async Task<bool> Delete(int id)
        {
            using var connection = new SqlConnection(_databaseSettings.DefaultConnection);
            var query = @"
                DELETE FROM Users
                WHERE Id = @Id";

            int affectedRows = await connection.ExecuteAsync(query, new { Id = id });
            return affectedRows > 0;
        }
        // Metod för att hitta en användare baserat på användarnamn
        public async Task<User> FindByUsername(string username)
        {
            using var connection = new SqlConnection(_databaseSettings.DefaultConnection);
            var query = @"
                SELECT u.*, r.*
                FROM Users u
                JOIN Roles r ON u.RoleId = r.Id
                WHERE u.Username = @Username";

            var userRoleMapping = await connection.QueryAsync<User, Role, User>(
                query,
                (user, role) =>
                {
                    user.Role = role;
                    return user;
                },
                new { Username = username }
            );

            // Returnera den första matchande användaren
            return userRoleMapping.FirstOrDefault();
        }

    }
}
