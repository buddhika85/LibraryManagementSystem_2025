using AutoMapper;
using Core.DTOs;
using Core.Entities;
using Core.Interfaces;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Infrastructure.Services
{
    public class BookService : IBookService
    {
        private readonly IMapper mapper;
        private readonly IUnitOfWork unitOfWork;
        private readonly IBooksRepository booksRepository;
        private readonly IAuthorRepository authorRepository;

        public BookService(IMapper mapper, IUnitOfWork unitOfWork)
        {
            this.mapper = mapper;
            this.unitOfWork = unitOfWork;
            booksRepository = unitOfWork.BookRepository;
            authorRepository = unitOfWork.AuthorRepository;
        }

        /// <summary>
        /// Retunrs all books awith their authors for display purposes
        /// </summary>
        /// <returns>All books along with author information</returns>
        public async Task<BookWithAuthorListDto> GetBooksWithAuthorsAsync()
        {
            var entities = await booksRepository.GetBooksIncludingAuthorsAsync();
            var dtos = mapper.Map<IReadOnlyList<BookWithAuthorsDto>>(entities);
            return new BookWithAuthorListDto { BookWithAuthorList = dtos };
        }

        /// <summary>
        /// Retunrs a book with its authors for display purposes filtered by Id provided
        /// </summary>
        /// <param name="id">ID of book</param>
        /// <returns>A single book with authors infor if found, or else null</returns>
        public async Task<BookWithAuthorsDto?> GetBookById(int id)
        {
            var entity = await booksRepository.GetBookByIdAsync(id);
            if (entity == null)
                return null;
            return mapper.Map<BookWithAuthorsDto>(entity);
        }


        /// <summary>
        /// Creates or updates a book along with its related authors.
        /// </summary>
        /// <param name="bookSaveDto">The data transfer object containing book details and associated author information.</param>
        /// <returns>A result object indicating success or failure of the operation.</returns>
        public async Task<InsertUpdateResultDto> SaveBookAsync(BookSaveDto bookSaveDto)
        {
            var result = new InsertUpdateResultDto();

            var authorEntities = await authorRepository.GetAuthorsByIdsAsync(bookSaveDto.AuthorIds);
            if (authorEntities == null || authorEntities.Count == 0)
            {
                result.ErrorMessage = "Invalid author IDs.";
                return result;                
            }


            Book? model;
            if (bookSaveDto.Id == 0)
            {
                // add mode
                model = mapper.Map<Book>(bookSaveDto);
                booksRepository.Add(model);  
                model.Authors = authorEntities;
            }
            else
            {
                // edit mode
                model = await booksRepository.GetBookByIdAsync(bookSaveDto.Id);
                if (model == null)
                {
                    result.ErrorMessage = $"Book with such ID {bookSaveDto.Id} unavailable in DB for editing";
                    return result;
                }
                model = mapper.Map(bookSaveDto, model);
                model.Authors.Clear();                      // Clear existing authors
                foreach (var author in authorEntities)
                {
                    model.Authors.Add(author);              // Add tracked authors
                }               
            }            
           
            if (!await unitOfWork.SaveAllAsync())
            {
                result.ErrorMessage = "Could not save book.";                
            }

            result.EntityId = model.Id;
            return result;
        }

        /// <summary>
        /// Returns true / false if a book with id exists in database
        /// </summary>
        /// <param name="id">ID value of the book we are searching for</param>
        /// <returns>True if book with ID exists, elase false</returns>
        public async Task<bool> IsExistsAsync(int id)
        {
            return await booksRepository.GetByIdAsync(id) != null;
        }
    }
}
