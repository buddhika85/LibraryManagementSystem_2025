using Core.DTOs;

namespace Core.Interfaces
{
    public interface ILibraryService
    {
        public Task<IReadOnlyList<AuthorDto>> GetAuthorsAsync();
        public Task<BookWithAuthorListDto> GetBooksWithAuthorsAsync();
    }
}
