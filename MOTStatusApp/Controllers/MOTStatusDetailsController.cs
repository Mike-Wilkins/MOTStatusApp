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

        [HttpGet("{customerId}")]
        [ProducesResponseType(200, Type = typeof(MOTStatusDetails))]
        [ProducesResponseType(400)]
        public IActionResult GetStatusDetail(int customerId)
        {
            if (!_statusDetailsRepository.StatusDetailExists(customerId))
                return NotFound();

            var statusDetail = _statusDetailsRepository.GetStatusDetail(customerId);

            if (!ModelState.IsValid)
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

        [HttpPost("create")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult CreateMOTStatusDetail(MOTStatusDetails detailsCreate)
        {
            if(detailsCreate == null)
                return BadRequest(ModelState);

            var details = _statusDetailsRepository.GetStatusDetails().
                Where(d => d.RegistrationNumber == detailsCreate.RegistrationNumber).FirstOrDefault();

            if (details != null)
            {
                ModelState.AddModelError("","Registration already exists.");
                return StatusCode(422, ModelState);
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var statusDetails =_statusDetailsRepository.Add(detailsCreate);
            _statusDetailsRepository.Save();

            return Ok("Successfully created");
        }
        [HttpPut("customerId")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult UpdateMOTStatusDetail(int customerId, MOTStatusDetails detailsUpdate)
        {
            if (detailsUpdate == null)
                return BadRequest(ModelState);

            if(customerId != detailsUpdate.Id)
                return BadRequest(ModelState);

            if (!_statusDetailsRepository.StatusDetailExists(customerId))
                return NotFound();

            if(!ModelState.IsValid)
                return BadRequest();         

            _statusDetailsRepository.Update(detailsUpdate);   

            return NoContent();
        }

        [HttpDelete("delete")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult DeleteMOTStatusDetail(int customerId)
        {
            if (!_statusDetailsRepository.StatusDetailExists(customerId))
                return NotFound();

            var statusDetailToDelete = _statusDetailsRepository.GetStatusDetail(customerId);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            _statusDetailsRepository.Delete(statusDetailToDelete);

            return NoContent();
        }
    }
}
