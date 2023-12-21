using Microsoft.AspNetCore.Mvc;
using MOTStatusWebApi.Interfaces;
using MOTStatusWebApi.Models;

namespace MOTStatusWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MOTTestCertificateDetailsController : Controller
    {
        private readonly IMOTTestCertificateDetailsRepository _testCertificateRepository;

        public MOTTestCertificateDetailsController (IMOTTestCertificateDetailsRepository testCertificateDetailsRepository)
        {
            _testCertificateRepository = testCertificateDetailsRepository;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<MOTTestCertificateDetails>))]
        public IActionResult GetTestCertificateDetails()
        {
            var testDetails = _testCertificateRepository.GetTestCertificateDetails();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(testDetails);

        }

        [HttpGet("{customerId}")]
        [ProducesResponseType(200, Type = typeof(MOTTestCertificateDetails))]
        [ProducesResponseType(400)]
        public IActionResult GetTestDetail(int Id)
        {
            if (!_testCertificateRepository.TestDetailExists(Id)) 
                return NotFound();

            var testDetail = _testCertificateRepository.GetTestCertificateDetail(Id);

            if (!ModelState.IsValid)
                return BadRequest();

            return Ok(testDetail);
        }

        [HttpGet("{Id}/registration")]
        [ProducesResponseType(200, Type = typeof(string))]
        [ProducesResponseType(400)]
        public IActionResult GetStatusDetailRegistration(string registration)
        {
            if (!_testCertificateRepository.TestDetailExists(registration))
                return NotFound();

            var statusDetail = _testCertificateRepository.GetRegistrationNumber(registration);

            if (!ModelState.IsValid)
                return BadRequest();

            return Ok(statusDetail);
        }

        [HttpPost("create")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult CreateMOTTestDetail(MOTTestCertificateDetails detailCreate)
        {
            if (detailCreate == null)
                return BadRequest(ModelState);

            var details = _testCertificateRepository.GetTestCertificateDetails().
                Where(d => d.RegistrationNumber == detailCreate.RegistrationNumber).FirstOrDefault();

            if (details != null)
            {
                ModelState.AddModelError("", "Registration already exists.");
                return StatusCode(422, ModelState);
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            _testCertificateRepository.Add(detailCreate);
            _testCertificateRepository.Save();

            return Ok("Successfully created");
        }

        [HttpPut("customerId")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult UpdateMOTTestDetail(int customerId, MOTTestCertificateDetails detailsUpdate)
        {
            if (detailsUpdate == null)
                return BadRequest(ModelState);

            if (customerId != detailsUpdate.Id)
                return BadRequest(ModelState);

            if (!_testCertificateRepository.TestDetailExists(customerId))
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest();

            _testCertificateRepository.Update(detailsUpdate);

            return NoContent();
        }

        [HttpDelete("delete")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult DeleteMOTStatusDetail(int customerId)
        {
            if (!_testCertificateRepository.TestDetailExists(customerId))
                return NotFound();

            var statusDetailToDelete = _testCertificateRepository.GetTestCertificateDetail(customerId);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            _testCertificateRepository.Delete(statusDetailToDelete);

            return NoContent();
        }
    }
}
