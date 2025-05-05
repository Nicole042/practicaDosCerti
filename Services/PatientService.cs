using PracticoDos.Models;

namespace PracticoDos.Services
{
    public class PatientService
    {
        private readonly string _filePath = "patients.txt";

        // Create
        public Patient AddPatient(Patient patient)
        {
            var line = $"{patient.Name},{patient.LastName},{patient.CI},{patient.BloodType}";
            File.AppendAllLines(_filePath, new[] { line });
            return patient;
        }

        // Read All
        public List<Patient> GetAllPatients()
        {
            if (!File.Exists(_filePath))
                return new List<Patient>();

            var lines = File.ReadAllLines(_filePath);
            return lines.Select(line =>
            {
                var parts = line.Split(',');
                return new Patient
                {
                    Name = parts[0],
                    LastName = parts[1],
                    CI = parts[2],
                    BloodType = parts[3]
                };
            }).ToList();
        }

        // Read by CI
        public Patient? GetPatientByCI(string ci)
        {
            return GetAllPatients().FirstOrDefault(p => p.CI == ci);
        }

        // Update
        public bool UpdatePatient(string ci, Patient updatedPatient)
        {
            var patients = GetAllPatients();
            var index = patients.FindIndex(p => p.CI == ci);
            if (index == -1)
                return false;

            patients[index].Name = updatedPatient.Name;
            patients[index].LastName = updatedPatient.LastName;

            SaveAll(patients);
            return true;
        }

        // Delete
        public bool DeletePatient(string ci)
        {
            var patients = GetAllPatients();
            var updatedList = patients.Where(p => p.CI != ci).ToList();
            if (updatedList.Count == patients.Count)
                return false;

            SaveAll(updatedList);
            return true;
        }

        // Helper: sobrescribe todo el archivo
        private void SaveAll(List<Patient> patients)
        {
            var lines = patients.Select(p => $"{p.Name},{p.LastName},{p.CI},{p.BloodType}");
            File.WriteAllLines(_filePath, lines);
        }
    }
}