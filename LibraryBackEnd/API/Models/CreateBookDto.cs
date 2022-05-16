namespace API.Models
{
    public class CreateBookDto
    {
        public string Isbn { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
        public int AmountOfCopies { get; set; }
    }
}