using InternJohan.Dev.Infrastructure.Configuration;
using InternJohan.Dev.Infrastructure.Models;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;
using Dapper;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using Microsoft.Extensions.Logging;


namespace InternJohan.Dev.Infrastructure.Repository
{
    public class SportEventRepository
    {
        private readonly DatabaseSettings _databaseSettings;

        public SportEventRepository(IOptions<DatabaseSettings> databaseSettings)
        {
            _databaseSettings = databaseSettings.Value;
        }

        public async Task<IEnumerable<SportEvent>> FindAll()
        {
            IEnumerable<SportEvent> items;
            using (var connection = new SqlConnection(_databaseSettings.DefaultConnection))
                items = await connection.QueryAsync<SportEvent>(@"
                    SELECT 
                        se.Id,
                        se.Sport,
                        se.NeededParticipants,
                        se.Participants,
                        se.DateTime,
                        se.Location,
                        se.UserHostId,
                        u.Username
                    FROM 
                        SportEvents se 
                    JOIN 
                        Users u ON se.UserHostId = u.Id
                ");

            return items;
        }

        public async Task<SportEvent> FindById(int id)
        {
            using (var connection = new SqlConnection(_databaseSettings.DefaultConnection))
                return await connection.QueryFirstOrDefaultAsync<SportEvent>(@"
                    SELECT 
                        se.* 
                         
                    FROM 
                        SportEvents se
                    WHERE 
                        se.Id = @Id
                ", new { Id = id });
        }

        public async Task<bool> Insert(SportEvent sportEvent)
        {
            //try
            //{

            using var connection = new SqlConnection(_databaseSettings.DefaultConnection);
            sportEvent.Id = await connection.ExecuteScalarAsync<int>(@"
                    DECLARE @InsertedSportEvent TABLE (Id INT);

                    INSERT INTO SportEvents (
                        Sport,
                        NeededParticipants,
                        Participants,
                        DateTime,
                        Location,
                        UserHostId
                    )
                    OUTPUT INSERTED.Id INTO @InsertedSportEvent
                    VALUES (
                        @Sport,
                        @NeededParticipants,
                        @Participants,
                        @DateTime,
                        @Location,
                        @UserHostId
                    );

                    SELECT Id FROM @InsertedSportEvent;
                ", new
            {
                sportEvent.Sport,
                sportEvent.NeededParticipants,
                sportEvent.Participants,
                DateTime = sportEvent.DateTime, // Om datumet formateras, hantera det här
                sportEvent.Location,
                UserHostId = sportEvent.UserHost.Id
            });

            return true;
            //}
            //catch
            //{
            //    return false;
            //}
        }

        public async Task<bool> Update(SportEvent sportEvent)
        {
            //Console.WriteLine(sportEvent.Sport);
            //Console.WriteLine(sportEvent.NeededParticipants);
            //Console.WriteLine(sportEvent.Participants);
            //Console.WriteLine(sportEvent.DateTime);
            //Console.WriteLine(sportEvent.Location);
            //Console.WriteLine(sportEvent.Id);


            //try
            {
                using var connection = new SqlConnection(_databaseSettings.DefaultConnection);
                var query = @"
             UPDATE SportEvents
 SET
     Sport = @Sport,
     NeededParticipants = @NeededParticipants,
     Participants = @Participants,
    DateTime = @DateTime,
     SportEvents.Location = @Location
 WHERE Id = @Id
        ";

                await connection.ExecuteAsync(query, new
                {
                    Sport = sportEvent.Sport,
                    NeededParticipants = sportEvent.NeededParticipants,
                    Participants = sportEvent.Participants,
                    DateTime = sportEvent.DateTime,
                    Location = sportEvent.Location,
                    //UserHostId = sportEvent.UserHost.Id,
                    Id = sportEvent.Id
                });

                return true;
            }
            //catch (Exception ex)
            {
                //Console.WriteLine($"Error: /*{ex.Message}*/");
                return false;
            }
        }


        public async Task<bool> IsAdminOrModerator(int userId)
        {
            using var connection = new SqlConnection(_databaseSettings.DefaultConnection);
            var roleId = await connection.ExecuteScalarAsync<int?>(@"
        SELECT RoleId FROM Users WHERE Id = @UserId", new { UserId = userId });

            if (roleId.HasValue && (roleId == 2 || roleId == 3))
            {
                Console.WriteLine("Användaren är antingen administratör eller moderator");
                return true;
            }
            else
            {
                Console.WriteLine("Användaren är inte administratör eller moderator");
                return false;
            }
        }

        public async Task<bool> DeleteEvent(int userId, int eventId)
        {
            //try 
            {
                // Kontrollera om användaren är ägaren till evenemanget
                var isOwner = await IsUserEventHost(userId, eventId);

                if (!isOwner)
                {
                    var isAdminOrModerator = await IsAdminOrModerator(userId);

                    if (!isAdminOrModerator)
                    {
                        return false; // Användaren är inte ägaren, returnera false
                    }
                }

                using var connection = new SqlConnection(_databaseSettings.DefaultConnection);

                var removedRows = await connection.ExecuteAsync(@"
                DELETE FROM EventParticipants
                WHERE EventId = @Id 
                ", new { Id = eventId });

                var affectedRows = await connection.ExecuteAsync(@"
            DELETE FROM SportEvents
            WHERE Id = @Id
        ", new { Id = eventId });

                return affectedRows > 0;
            }
            //catch
            {
                return false;
            }
        }
        public async Task<bool> IsUserEventHost(int userId, int eventId)
        {
            using (var connection = new SqlConnection(_databaseSettings.DefaultConnection))
            {
                var ownerId = await connection.ExecuteScalarAsync<int?>(@"
         SELECT UserHostId
         FROM SportEvents
         WHERE Id = @EventId
     ", new { EventId = eventId });

                return ownerId.HasValue && ownerId.Value == userId;
            }
        }


        public async Task<bool> JoinEvent(int userId, int eventId)
        {
            //try
            {
                // Kontrollera om användaren redan är en deltagare i evenemanget
                var isUserParticipant = await IsUserParticipant(userId, eventId);
                if (isUserParticipant)
                {
                    return false; // Användaren är redan en deltagare, returnera false
                }

                using (var connection = new SqlConnection(_databaseSettings.DefaultConnection))
                {
                    await connection.OpenAsync();

                    // Uppdatera antalet deltagare i SportEvents
                    var affectedRows = await connection.ExecuteAsync(@"
                UPDATE SportEvents
                SET Participants = Participants + 1
                WHERE Id = @Id
            ", new { Id = eventId });

                    if (affectedRows > 0)
                    {
                        // Lägg till deltagaren i EventParticipants
                        await AddParticipant(userId, eventId);

                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            //catch (Exception ex)
            {
                // Hantera undantag och returnera false
                Console.WriteLine($"Error:");
                return false;
            }
        }


        public async Task<bool> IsUserParticipant(int userId, int eventId)
        {
            using (var connection = new SqlConnection(_databaseSettings.DefaultConnection))
            {
                var result = await connection.ExecuteScalarAsync<int>(@"
            SELECT COUNT(*)
            FROM EventParticipants
            WHERE UserId = @UserId AND EventId = @EventId
        ", new { UserId = userId, EventId = eventId });

                return result > 0;
            }
        }

        public async Task AddParticipant(int userId, int eventId)
        {
            //try
            {
                using (var connection = new SqlConnection(_databaseSettings.DefaultConnection))
                {
                    await connection.OpenAsync();

                    // Lägg till användaren som deltagare i EventParticipants
                    var affectedRows = await connection.ExecuteAsync(@"
                INSERT INTO EventParticipants (UserId, EventId)
                VALUES (@UserId, @EventId)
            ", new { UserId = userId, EventId = eventId });

                    if (affectedRows == 0)
                    {
                        Console.WriteLine("Failed to add participant to EventParticipants.");
                    }
                }
            }
            //catch (Exception ex)
            {
                // Hantera undantag och logga felmeddelandet
                Console.WriteLine($"Error:");
            }
        }
        public async Task RemoveParticipant(int userId, int eventId)
        {
            using (var connection = new SqlConnection(_databaseSettings.DefaultConnection))
            {
                await connection.OpenAsync();

                // Start a transaction to ensure both operations succeed or fail together
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        // Ta bort användaren som deltagare i EventParticipants
                        var affectedRows = await connection.ExecuteAsync(@"
                DELETE FROM EventParticipants
                WHERE UserId = @UserId AND EventId = @EventId
                ", new { UserId = userId, EventId = eventId }, transaction: transaction);

                        if (affectedRows == 0)
                        {
                            Console.WriteLine("Failed to remove participant from EventParticipants.");
                            transaction.Rollback();
                            return;
                        }

                        // Decrementera antalet deltagare i SportEvents
                        affectedRows = await connection.ExecuteAsync(@"
                UPDATE SportEvents
                SET Participants = Participants - 1
                WHERE Id = @Id
                ", new { Id = eventId }, transaction: transaction);

                        if (affectedRows == 0)
                        {
                            Console.WriteLine("Failed to decrement participants in SportEvents.");
                            transaction.Rollback();
                        }
                        else
                        {
                            transaction.Commit();
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error: {ex.Message}");
                        transaction.Rollback();
                    }
                }
            }
        }
    }
}


