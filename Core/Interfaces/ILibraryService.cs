using Core.DTOs;

namespace Core.Interfaces
{
    public interface ILibraryService
    {
        Task<IReadOnlyList<BookWithAuthorsDto>> FindBooksAsync(BookFilterDto bookFilterDto, bool isAvailable);
        public Task<IReadOnlyList<AuthorDto>> GetAuthorsAsync();
    }
}
