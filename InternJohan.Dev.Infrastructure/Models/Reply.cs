using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InternJohan.Dev.Infrastructure.Models
{
    public class Reply
    {
        public int ReplyId { get; set; }
        public int UserId { get; set; }
        public string Author { get; set; }
        public string Content { get; set; }
        public DateTime Timestamp { get; set; }
        public int PostId { get; set; }

        //// Navigationsegenskaper
        //public virtual User User { get; set; }
    }
}
