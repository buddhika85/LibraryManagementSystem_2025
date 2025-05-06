using Core.Enums;

namespace Core.DTOs
{
    public class BookSearchDto
    {
        public BookGenre SelectedGenre { get; set; } = BookGenre.None;
        public int SelectedAuthorId { get; set; }        
        public string BookTitleSearchStr { get; set; } = string.Empty;        
    }
}
