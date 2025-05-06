
using Core.DTOs;
using Core.Enums;
using Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{

    [Authorize]
    public class BorrowalsController : BaseApiController
    {
        private readonly IBorrowalsService borrowalsService;
        private readonly ILibraryService libraryService;

        public BorrowalsController(IBorrowalsService borrowalsService, ILibraryService libraryService)
        {
            this.borrowalsService = borrowalsService;
            this.libraryService = libraryService;
        }

        [Authorize(Roles = "Admin,Staff")]
        [HttpGet("all-borrowals")]
        public async Task<ActionResult<BorrowalsDisplayListDto>> GetAllBorrowals()
        {
            var dto = await borrowalsService.GetAllBorrowalsAsync();
            return Ok(dto);
        }

        [Authorize(Roles = "Admin,Staff")]
        [HttpGet("borrow-form-data")]
        public async Task<ActionResult<BorrowFormDto>> GetBorrowFormData()
        {
            var dto = new BorrowFormDto {
                Authors = await libraryService.GetAuthorsAsync(),
                Genres = Enum.GetValues(typeof(BookGenre)).Cast<BookGenre>().ToList()
            };
            return Ok(dto);
        }

        [Authorize(Roles = "Admin,Staff")]
        [HttpGet("filter-books")]
        public async Task<ActionResult<BookWithAuthorListDto>> FilterBooks(BookFilterDto bookFilterDto)
        {
            var dto = new BookWithAuthorListDto
            {
                BookWithAuthorList = await libraryService.FindBooksAsync(bookFilterDto, isAvailable: true)
            };
            return Ok(dto);
        }
    }
}
