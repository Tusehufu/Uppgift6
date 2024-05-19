using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using InternJohan.Dev.Infrastructure.Models;
using InternJohan.Dev.Infrastructure.Repository;
using InternJohan.Dev.Infrastructure.ViewModel;
using System.Security.Claims;

namespace InternJohan.Dev.App.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AttendeeController : ControllerBase
    {
        private readonly AttendeeService _attendeeService;
        public AttendeeController(AttendeeService attendeeService)
        {
            _attendeeService = attendeeService;
        }


    
        [HttpGet("{eventId}")]
        public async Task<ActionResult<IEnumerable<Attendees>>> FindAttendees(int eventId)
        {
            var attendees = await _attendeeService.FindAttendees(eventId);
            return Ok(attendees);
        }
    }
}
