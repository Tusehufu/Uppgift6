using InternJohan.Dev.Infrastructure.Configuration;
using InternJohan.Dev.Infrastructure.Models;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;
using Dapper;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace InternJohan.Dev.Infrastructure.Repository
{
    public class AttendeeRepository
    {
        private readonly DatabaseSettings _databaseSettings;

        public AttendeeRepository(IOptions<DatabaseSettings> databaseSettings)
        {
            _databaseSettings = databaseSettings.Value;
        }
        public async Task<IEnumerable<Attendees>> FindAttendees(int eventId)
        {
            IEnumerable<Attendees> items;

            using (var connection = new SqlConnection(_databaseSettings.DefaultConnection))
                items = await connection.QueryAsync<Attendees>(@"
                    SELECT 
                        se.UserId,
                        se.EventId,
                        Users.Username AS Username
                    FROM 
                        EventParticipants se 
                    JOIN Users on Users.Id = se.UserId
                    WHERE se.EventId = @EventId
                " ,new
                {
                EventId = eventId
                } );

            return items;
        }
    }
}
