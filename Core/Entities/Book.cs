using Core.Enums;

namespace Core.Entities
{
    public class Book : BaseEntity
    {      
        public required string Title { get; set; }
        public required BookGenre Genre { get; set; }
        public required ICollection<Author> Authors { get; set; } = new List<Author>();
        public required DateTime PublishedDate { get; set; }
        public required string PictureUrl { get; set; }
    }
}
