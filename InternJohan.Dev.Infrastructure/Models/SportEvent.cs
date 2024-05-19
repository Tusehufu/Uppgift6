using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InternJohan.Dev.Infrastructure.Models
{
    public class SportEvent
    {
        public int Id { get; set; }
        public string Sport { get; set; }
        public int NeededParticipants { get; set; }
        public int Participants { get; set; } = 0;
        public DateTime DateTime { get; set; }
        public string Location { get; set; }
        public User UserHost { get; set; }
    }
    public class Attendees
    {
        public int EventId { get; set; }
        public int UserId { get; set; }
        public string Username { get; set; }    
    }
}
