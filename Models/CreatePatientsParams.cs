namespace PracticoDos.Models
{
    public class CreatePatientParams
    {
        public required string Name { get; set; }
        public required string LastName { get; set; }
        public required string CI { get; set; }
    }
}
