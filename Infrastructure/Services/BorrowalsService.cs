using AutoMapper;
using Core.DTOs;
using Core.Entities;
using Core.Interfaces;

namespace Infrastructure.Services
{
    public class BorrowalsService : IBorrowalsService
    {
        private readonly IBorrowalsRepository repository;
        private readonly IMapper mapper;

        public BorrowalsService(IBorrowalsRepository repository, IMapper mapper)
        {
            this.repository = repository;
            this.mapper = mapper;
        }

        public async Task<BorrowalsDisplayListDto> GetAllBorrowalsAsync()
        {
            var allBrrowals = await repository.GetAllBorrowalsWithNavPropsAsync();
            return new BorrowalsDisplayListDto
            {
                BorrowalsList = mapper.Map<IReadOnlyList<BorrowalsDisplayDto>>(allBrrowals)
            };
        }
    }
}
