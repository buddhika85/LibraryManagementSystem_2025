using AutoMapper;
using Core.DTOs;
using Core.Entities;
using Core.Interfaces;

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
        /// Returns book with its author info and all authors list for editing purposes
        /// </summary>
        /// <param name="id">book id</param>
        /// <returns>A DTO containing Book info, all authors for update purposes</returns>
        public async Task<BookForEditDto> GetBookForEditingById(int id)
        {
            var dto = new BookForEditDto();
            if (id > 0)
            {
                // edit mode request
                var entity = await booksRepository.GetBookByIdAsync(id);
                if (entity == null)
                {
                    dto.ErrorMessage = $"Book with ID {id} unavailable for editing purposes.";
                    return dto;
                }
                dto.Book = mapper.Map<BookSaveDto>(entity);
            }

            // add mode
            dto.Book = null;

            // add / edit both
            var authorEntities = await authorRepository.ListAllAsync();
            dto.AllAuthors = mapper.Map<IReadOnlyList<AuthorDto>>(authorEntities);
            return dto;
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
                model = AddBook(bookSaveDto, authorEntities);
            }
            else
            {
                // edit mode
                model = await booksRepository.GetBookByIdAsync(bookSaveDto.Id);     // get tracked book object
                if (model == null)
                {
                    result.ErrorMessage = $"Book with such ID {bookSaveDto.Id} unavailable in DB for editing";
                    return result;
                }

                model = UpdateBook(bookSaveDto, authorEntities, model);
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

        /// <summary>
        /// Delete a book by Id
        /// </summary>
        /// <param name="id">Id of the book to delete</param>
        /// <returns>The data transfer object containing info on book deleted or not</returns>
        public async Task<ResultDto> DeleteAsync(int id)
        {
            var result = new ResultDto();
            var entity = await booksRepository.GetByIdAsync(id);
            if (entity == null)
            {
                result.ErrorMessage = $"Book with such ID {id} unavailable in DB for deletion";
                return result;
            }
            booksRepository.Remove(entity);
            if (!await unitOfWork.SaveAllAsync())
            {
                result.ErrorMessage = "Could not delete book.";
            }
            return result;
        }

        #region Helpers
        private Book AddBook(BookSaveDto bookSaveDto, IList<Author> authorEntities)
        {
            Book? model = mapper.Map<Book>(bookSaveDto);
            booksRepository.Add(model);
            model.Authors = authorEntities;
            return model;
        }

        private Book UpdateBook(BookSaveDto bookSaveDto, IList<Author> authorEntities, Book trackedModel)
        {
            trackedModel = mapper.Map(bookSaveDto, trackedModel);
            trackedModel.Authors.Clear();                      // Clear existing authors
            foreach (var author in authorEntities)
            {
                trackedModel.Authors.Add(author);              // Add tracked authors
            }
            return trackedModel;
        }       
        #endregion
    }
}
