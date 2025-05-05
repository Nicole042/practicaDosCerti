using Microsoft.AspNetCore.Mvc;
using PracticoDos.Models;
using PracticoDos.Services;

namespace PracticoDos.Controllers
{
    [Route("patients")]
    [ApiController]
    public class PatientsController : ControllerBase
    {
        private readonly PatientService _service;

        public PatientsController()
        {
            _service = new PatientService();
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
    }
}
