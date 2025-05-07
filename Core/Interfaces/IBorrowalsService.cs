using Core.DTOs;

namespace Core.Interfaces
{
    public interface IBorrowalsService
    {
        Task<BorrowResultDto> BorrowBook(BookBorrowRequestDto bookFilterDto);
        Task<BorrowalsDisplayListDto> GetAllBorrowalsAsync();
        Task<BorrowalReturnInfoDto> GetBorrowalReturnInfoDto(int borrowalId, decimal perDayLateFeeDollars);
    }
}
