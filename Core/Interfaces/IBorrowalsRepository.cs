using Core.DTOs;
using Core.Entities;

namespace Core.Interfaces
{
    public interface IBorrowalsRepository : IGenericRepository<Borrowals>
    {
        Task<IReadOnlyList<Borrowals>> GetAllBorrowalsWithNavPropsAsync();

        Task<IReadOnlyList<Borrowals>> SearchBorrowalsWithNavPropsAsync(BorrowalsSearchDto searchDto);

        Task<Borrowals?> GetAllBorrowalWithNavPropsAsync(int borrowalId);
        
        Task<IReadOnlyCollection<BorrowalSummaryDto>> GetBorrowalSummaryForMemberAsync(string memberEmail);        
    }
}
