using Core.DTOs;

namespace Core.Interfaces
{
    public interface IBorrowalsService
    {
        Task<BorrowResultDto> BorrowBook(BookBorrowRequestDto bookFilterDto);
        Task<BorrowalsDisplayListDto> GetAllBorrowalsAsync();
    }
}
