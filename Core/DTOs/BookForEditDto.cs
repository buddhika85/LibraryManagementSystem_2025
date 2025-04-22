namespace Core.DTOs
{
    public class BookForEditDto : ResultDto
    {
        public BookSaveDto? Book { get; set; }
        public IReadOnlyList<AuthorDto> AllAuthors { get; set; } = new List<AuthorDto>();
    }
}
