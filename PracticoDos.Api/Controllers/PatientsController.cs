using Microsoft.AspNetCore.Mvc;
using PracticoDos.Models;
using PracticoDos.Services;
using System.Text.Json;

namespace PracticoDos.Controllers
{
    [Route("patients")]
    [ApiController]
    public class PatientsController : ControllerBase
    {
        private readonly PatientService _service;
        private readonly ILogger<PatientsController> _logger;

        public PatientsController(ILogger<PatientsController> logger)
        {
            _service = new PatientService();
            _logger = logger;
        }

        // Create
        [HttpPost]
        [Route("")]
        public ActionResult<Patient> CreatePatient([FromBody] CreatePatientParams request)
        {
            if (string.IsNullOrWhiteSpace(request.Name) ||
                string.IsNullOrWhiteSpace(request.LastName) ||
                string.IsNullOrWhiteSpace(request.CI))
            {
                return BadRequest("Name, LastName, and CI are required.");
            }

            string[] bloodTypes = { "A+", "A-", "B+", "B-", "AB+", "AB-", "O+", "O-" };
            Random random = new Random();
            string randomBloodType = bloodTypes[random.Next(bloodTypes.Length)];

            var newPatient = new Patient
            {
                Name = request.Name,
                LastName = request.LastName,
                CI = request.CI,
                BloodType = randomBloodType
            };

            _service.AddPatient(newPatient);
            return Ok(newPatient);
        }

        // Read All
        [HttpGet]
        [Route("")]
        public ActionResult<List<Patient>> GetAll()
        {
            return Ok(_service.GetAllPatients());
        }

        // Read by CI
        [HttpGet]
        [Route("{ci}")]
        public ActionResult<Patient> GetByCI(string ci)
        {
            var patient = _service.GetPatientByCI(ci);
            if (patient == null)
                return NotFound("Patient not found");

            return Ok(patient);
        }

        // Update
        [HttpPut]
        [Route("{ci}")]
        public ActionResult UpdatePatient(string ci, [FromBody] UpdatePatientParams updated)
        {
            if (string.IsNullOrWhiteSpace(updated.Name) || string.IsNullOrWhiteSpace(updated.LastName))
            {
                return BadRequest("Name and LastName are required.");
            }

            var existingPatient = _service.GetPatientByCI(ci);
            if (existingPatient == null)
            {
                return NotFound("Patient not found");
            }

            var updatedPatient = new Patient
            {
                Name = updated.Name,
                LastName = updated.LastName,
                CI = existingPatient.CI,
                BloodType = existingPatient.BloodType
            };

            var success = _service.UpdatePatient(ci, updatedPatient);
            return success ? Ok("Patient updated") : StatusCode(500, "Error updating patient");
        }


        // Delete
        [HttpDelete]
        [Route("{ci}")]
        public ActionResult DeletePatient(string ci)
        {
            var success = _service.DeletePatient(ci);
            if (!success)
                return NotFound("Patient not found");

            return Ok("Patient deleted");
        }

        [HttpGet]
        [Route("{ci}/gifts")]
        public async Task<ActionResult<PatientWithGifts>> GetPatientWithGifts(string ci)
        {
            var patient = _service.GetPatientByCI(ci);
            if (patient == null) {
                _logger.LogWarning("Patient with CI {CI} not found when retrieving gifts", ci);
                return NotFound("Patient not found");
            }

            using var client = new HttpClient();

            try
            {
                var response = await client.GetAsync("https://api.restful-api.dev/objects");
                response.EnsureSuccessStatusCode();

                var json = await response.Content.ReadAsStringAsync();
                var gifts = JsonSerializer.Deserialize<List<Gift>>(json, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                _logger.LogInformation("Successfully retrieved {Count} gifts for patient {CI}", gifts?.Count ?? 0, ci);


                var result = new PatientWithGifts
                {
                    Name = patient.Name,
                    LastName = patient.LastName,
                    CI = patient.CI,
                    BloodType = patient.BloodType?? string.Empty,
                    Gifts = gifts ?? new List<Gift>()
                };

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving gifts for patient with CI: {CI}", ci);
                return StatusCode(500, $"Error retrieving gifts: {ex.Message}");
            }
        }

    }
}
