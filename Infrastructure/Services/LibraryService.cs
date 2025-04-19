using AutoMapper;
using Core.DTOs;
using Core.Entities;
using Core.Interfaces;

namespace Infrastructure.Services
{
    public class LibraryService : ILibraryService
    {
        private readonly IMapper mapper;
        private readonly IGenericRepository<Author> authorRepo;
        private readonly IBooksRepository booksRepository;

        public LibraryService(IMapper mapper, IUnitOfWork unitOfWork)
        {
            this.mapper = mapper;
            this.authorRepo = unitOfWork.AuthorRepository;
            this.booksRepository = unitOfWork.BookRepository;
        }

        public async Task<IReadOnlyList<AuthorDto>> GetAuthorsAsync()
        {
            var entities = await authorRepo.ListAllAsync();
            var dtos = mapper.Map<IReadOnlyList<AuthorDto>>(entities);
            return dtos;
        }

        public async Task<BookWithAuthorListDto> GetBooksWithAuthorsAsync()
        {
            var entities = await booksRepository.GetBooksIncludingAuthorsAsync();
            var dtos = mapper.Map<IReadOnlyList<BookWithAuthorsDto>>(entities);
            return new BookWithAuthorListDto { BookWithAuthorList = dtos };
        }
    }
}
