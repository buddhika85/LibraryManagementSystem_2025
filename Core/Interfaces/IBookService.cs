using Core.DTOs;

namespace Core.Interfaces
{
    public interface IBookService
    {
        // get
        public Task<BookWithAuthorListDto> GetBooksWithAuthorsAsync();
        public Task<BookWithAuthorsDto?> GetBookById(int id);

        public Task<InsertUpdateResultDto> SaveBookAsync(BookSaveDto bookSaveDto);          // insert / update
        public Task<bool> IsExistsAsync(int id);
    }
}
