
namespace Core.Entities
{
    public class Author : BaseEntity
    {       
        public required string Name { get; set; }
        public required string Country { get; set; }
        public required string Biography { get; set; }
        public DateTime DateOfBirth { get; set; }
        public required ICollection<Book> Books { get; set; } = new List<Book>();
    }
}
