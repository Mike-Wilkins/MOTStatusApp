using Microsoft.AspNetCore.Mvc;
using MOTStatusWebApi.Data;
using MOTStatusWebApi.Interfaces;

namespace MOTStatusWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MOTStatusDetailsController : Controller
    {
        private readonly IMOTStatusDetailsRepository _statusDetailsRepository;

        public MOTStatusDetailsController(IMOTStatusDetailsRepository statusDetailsRepository)
        {
            _statusDetailsRepository = statusDetailsRepository;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<MOTStatusDetails>))]
        public IActionResult GetStatusDetails()
        {
            var statusDetails = _statusDetailsRepository.GetStatusDetails();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(statusDetails);
        }

        [HttpGet("{Id}")]
        [ProducesResponseType(200, Type = typeof(MOTStatusDetails))]
        [ProducesResponseType(400)]
        public IActionResult GetStatusDetail(int id)
        {
            if(!_statusDetailsRepository.StatusDetailExists(id))
                return NotFound();

            var statusDetail = _statusDetailsRepository.GetStatusDetail(id);

            if(!ModelState.IsValid)
                return BadRequest();

            return Ok(statusDetail);
        }

        [HttpGet("{Id}/registration")]
        [ProducesResponseType(200, Type = typeof(string))]
        [ProducesResponseType(400)]
        public IActionResult GetStatusDetailRegistration(string registration)
        {
            if(!_statusDetailsRepository.StatusDetailExists(registration))
                return NotFound();

            var statusDetail = _statusDetailsRepository.GetRegistrationNumber(registration);

            if (!ModelState.IsValid)
                return BadRequest();

            return Ok(statusDetail);
        }
    }
}
