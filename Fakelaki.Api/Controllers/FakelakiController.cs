using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Fakelaki.Api.Lib.DAL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Fakelaki.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FakelakiController : ControllerBase
    {

        private readonly ILogger<FakelakiController> _logger;
        private readonly FakelakiContext _dataContext;
        public FakelakiController(ILogger<FakelakiController> logger, FakelakiContext dataContext)
        {
            _logger = logger;
            _dataContext = dataContext;
        }

        [HttpGet]
        public IEnumerable<string> Get()
        {
            var rng = new Random();
            return new List<string>();
        }
    }
}
