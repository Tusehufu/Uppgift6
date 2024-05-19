using InternJohan.Dev.Infrastructure.Configuration;
using InternJohan.Dev.Infrastructure.Models;
using Microsoft.Extensions.Options;
using Dapper;
using Microsoft.Data.SqlClient;

namespace InternJohan.Dev.Infrastructure.Repository
{
    public class AttendeeService
    {
        private readonly AttendeeRepository _attendeeRepository;
        public AttendeeService(AttendeeRepository attendeeRepository)
        {
            _attendeeRepository = attendeeRepository;
        }
        public async Task<IEnumerable<Attendees>> FindAttendees(int eventId)
        {
            return await _attendeeRepository.FindAttendees(eventId);
        }
    }
}
