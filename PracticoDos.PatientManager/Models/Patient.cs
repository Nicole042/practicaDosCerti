namespace PracticoDos.Models
{
    public class Patient
    {
        

        public required string Name { get; set; }
        public required string LastName { get; set; }
        public required string CI { get; set; }
        public string? BloodType { get; set; }
       
    }
}