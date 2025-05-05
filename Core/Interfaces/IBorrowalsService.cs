using Core.DTOs;

namespace Core.Interfaces
{
    public interface IBorrowalsService
    {
        Task<BorrowalsDisplayListDto> GetAllBorrowalsAsync();
    }
}
