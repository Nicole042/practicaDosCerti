using Microsoft.AspNetCore.Mvc;
using PracticoDos.Models;

namespace PracticoDos.Controllers
{


    [Route("patients")]
    [ApiController]
    public class PatientsController : ControllerBase
    {


        public PatientsController() { }

        [HttpPost]
        [Route("")]
        public Patient CreatePatient([FromBody]Patient patient)
        {
            string[] bloodTypes = { "A+", "A-", "B+", "B-", "AB+", "AB-", "O+", "O-" };
            Random random = new Random();
            string randomBloodType = bloodTypes[random.Next(bloodTypes.Length)];
            var newPatient = new Patient
        {
            Name = patient.Name,
            LastName = patient.LastName,
            CI = patient.CI,
            BloodType = randomBloodType
        };
            return newPatient;
        }
    }
}