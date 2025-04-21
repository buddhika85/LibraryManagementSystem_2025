using Core.Enums;

namespace Core.DTOs
{
    public class BookSaveDto : BaseDto
    {
        public required string Title { get; set; }
        public required BookGenre Genre { get; set; }
        public required List<int> AuthorIds { get; set; }
        public required DateTime PublishedDate { get; set; }
        public required string PictureUrl { get; set; }
    }
}
