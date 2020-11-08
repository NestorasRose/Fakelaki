using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Fakelaki.Api.Lib.DAL;
using Fakelaki.Api.Lib.Services.Interfaces;
using Fakelaki.Api.Lib.Models;
using Fakelaki.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Fakelaki.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FakelakiController : ControllerBase
    {

        private readonly ILogger<FakelakiController> _logger;
        private IMapper _mapper;
        private readonly IFakelakiService _fakelakiService;
        public FakelakiController(ILogger<FakelakiController> logger, IMapper mapper, IFakelakiService fakelakiService)
        {
            _logger = logger;
            _mapper = mapper;
            _fakelakiService = fakelakiService;
        }

        [HttpGet("{userId}")]
        public IActionResult GetAllEventFakelakia(int fakelakiId)
        {
            var fakelakia = _fakelakiService.GetByEvent(fakelakiId);
            var model = _mapper.Map<IList<FakelakiModel>>(fakelakia);
            return Ok(model);
        }

        [HttpPost("{eventId}/{emailTemplateId}")]
        public IActionResult Create([FromBody] FakelakiModel model, int emailTemplateId, int eventId)
        {
            // map model to entity
            var fakelaki = _mapper.Map<Lib.Models.Fakelaki>(model);

            try
            {
                // create user
                _fakelakiService.Create(fakelaki, emailTemplateId, eventId);
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
