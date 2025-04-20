namespace Core.DTOs
{
    public class BookWithAuthorListDto
    {
        public IReadOnlyList<BookWithAuthorsDto> BookWithAuthorList { get; set; } = [];
        public int Count => BookWithAuthorList.Count;
    }
}
