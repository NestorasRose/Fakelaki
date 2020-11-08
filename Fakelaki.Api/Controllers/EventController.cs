using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Fakelaki.Api.Lib.DAL;
using Fakelaki.Api.Lib.Models;
using Fakelaki.Api.Lib.Services.Interfaces;
using Fakelaki.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Fakelaki.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EventController : ControllerBase
    {

        private IEventService _eventService;
        private IMapper _mapper;
        private readonly ILogger<EventController> _logger;
        public EventController(ILogger<EventController> logger, IMapper mapper, IEventService eventService)
        {
            _logger = logger;
            _eventService = eventService;
            _mapper = mapper;
        }


        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var eventt = _eventService.Get(id);
            var model = _mapper.Map<EventModel>(eventt);
            return Ok(model);
        }


        [HttpGet("{userId}")]
        public IActionResult GetAllUserEvents(int userId)
        {
            var events = _eventService.GetByUser(userId);
            var model = _mapper.Map<IList<EventModel>>(events);
            return Ok(model);
        }

        [HttpPost("{userId}")]
        public IActionResult Create([FromBody] EventModel eventModel, int userId)
        {
            // map model to entity
            var eventt = _mapper.Map<Event>(eventModel);

            try
            {
                // create event
                _eventService.Create(eventt, userId);
                return Ok();
            }
            catch (Exception ex)
            {
                // return error message if there was an exception
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPatch]
        public IActionResult Update([FromBody] EventModel model)
        {
            // map model to entity and set id
            var eventt = _mapper.Map<Event>(model);

            try
            {
                // update event 
                _eventService.Update(eventt);
                return Ok();
            }
            catch (Exception ex)
            {
                // return error message if there was an exception
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
