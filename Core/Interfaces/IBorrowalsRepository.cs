using Core.Entities;

namespace Core.Interfaces
{
    public interface IBorrowalsRepository : IGenericRepository<Borrowals>
    {
        Task<IReadOnlyList<Borrowals>> GetAllBorrowalsWithNavPropsAsync();
    }
}
