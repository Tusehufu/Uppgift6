using InternJohan.Dev.Infrastructure.Models;
using System;

namespace InternJohan.Dev.Infrastructure.ViewModel
{
    public class SportEventViewModel
    {
        public string Sport { get; set; }
        public int NeededParticipants { get; set; }
        public int Participants { get; set; }
        public DateTime DateTime { get; set; }
        public string Location { get; set; }

    }
}

