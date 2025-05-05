using System;
using System.Collections.Generic;
using System.Linq;
using PracticoDos.Models;

namespace PracticoDos.Services
{
    
    public class PatientService
    {
        
        // Create
        public Patient AddPatient(Patient patient)
        {
            return patient;
        }

        // Read All
        public List<Patient> GetAllPatients()
        {
            return new List<Patient>();
        }

        // Read by Id
        public bool GetPatientByCI(int id)
        {
            return true;
        }

        // Update
        public bool UpdatePatient(int id, Patient updatedPatient)
        {
            return true;
        }

        // Delete
        public bool DeletePatient(int id)
        {
            return true;
        }
    }
}

