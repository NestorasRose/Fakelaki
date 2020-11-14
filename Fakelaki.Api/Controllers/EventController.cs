using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Fakelaki.Api.Lib.DAL;
using Fakelaki.Api.Lib.Models;
using Fakelaki.Api.Lib.Services.Interfaces;
using Fakelaki.Api.Models;
using System.Linq;
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
        private IQRCoderService _qRCoderService;
        private readonly ILogger<EventController> _logger;


        public EventController(ILogger<EventController> logger, IMapper mapper, IEventService eventService, IQRCoderService qRCoderService)
        {
            _logger = logger;
            _eventService = eventService;
            _mapper = mapper;
            _qRCoderService = qRCoderService;
        }


        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var model = _mapper.Map<EventModel>(_eventService.Get(id));
            model.QRCodeBase64 = _qRCoderService.Create(model.Id.ToString());
            return Ok(model);
        }


        [HttpGet("{userId}")]
        public IActionResult GetAllUserEvents(int userId)
        {
            var model = _mapper.Map<IList<EventModel>>(_eventService.GetByUser(userId)).ToList();
            model.ForEach(x => x.QRCodeBase64 = _qRCoderService.Create(x.Id.ToString()));
            return Ok(model);
        }

        [HttpPost("{userId}")]
        public IActionResult Create([FromBody] EventModel eventModel, int userId)
        {
            try
            {
                // create event
                var model = _mapper.Map<EventModel>(_eventService.Create(_mapper.Map<Event>(eventModel), userId));
                model.QRCodeBase64 = _qRCoderService.Create(model.Id.ToString());
                return Ok(model);
            }
            catch (Exception ex)
            {
                // return error message if there was an exception
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPut]
        public IActionResult Update([FromBody] EventModel model)
        {
            try
            {
                // update event 
                _eventService.Update(_mapper.Map<Event>(model));
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
