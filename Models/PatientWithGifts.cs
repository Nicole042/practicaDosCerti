namespace PracticoDos.Models
{
    public class PatientWithGifts
    {
        public required string Name { get; set; }
        public required string LastName { get; set; }
        public required string CI { get; set; }
        public required string BloodType { get; set; }
        public required List<Gift> Gifts { get; set; }
    }
}
