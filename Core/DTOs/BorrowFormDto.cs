using Core.Enums;

namespace Core.DTOs
{
    public class BorrowFormDto
    {
        public IReadOnlyList<AuthorDto> Authors { get; set; } = new List<AuthorDto>();
        public IReadOnlyList<BookGenre> Genres { get; set; } = new List<BookGenre>();
        public IReadOnlyList<UserDisplayDto> Members { get; set; } = new List<UserDisplayDto>();
    }
}
