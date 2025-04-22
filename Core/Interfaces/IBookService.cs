using Core.DTOs;

namespace Core.Interfaces
{
    public interface IBookService
    {
        // get
        public Task<BookWithAuthorListDto> GetBooksWithAuthorsAsync();
        public Task<BookWithAuthorsDto?> GetBookById(int id);
        public Task<BookForEditDto> GetBookForEditingById(int id);

        // insert / update
        public Task<InsertUpdateResultDto> SaveBookAsync(BookSaveDto bookSaveDto);          
        // deletion
        Task<ResultDto> DeleteAsync(int id);

        // available in DB?
        public Task<bool> IsExistsAsync(int id);
       
    }
}
