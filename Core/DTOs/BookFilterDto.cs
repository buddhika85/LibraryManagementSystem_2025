namespace Core.DTOs
{
    public class BookFilterDto
    {
        public int[] BookGenres { get; set; } = [];
        public int[] AuthorIds { get; set; } = [];
    }
}
