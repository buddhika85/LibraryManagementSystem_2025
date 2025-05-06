
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
        private readonly IUserService userService;

        public BorrowalsController(IBorrowalsService borrowalsService, ILibraryService libraryService, IUserService userService)
        {
            this.borrowalsService = borrowalsService;
            this.libraryService = libraryService;
            this.userService = userService;
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
                Genres = Enum.GetValues(typeof(BookGenre)).Cast<BookGenre>().ToList(),
                Members = (await userService.GetMembersAsync()).UsersList,
            };
            return Ok(dto);
        }

        [Authorize(Roles = "Admin,Staff")]
        [HttpPost("filter-books")]
        public async Task<ActionResult<BookWithAuthorListDto>> FilterBooks(BookFilterDto bookFilterDto)
        {
            if (bookFilterDto == null)
            {
                return BadRequest("Invalid filter parameters.");
            }

            var dto = new BookWithAuthorListDto
            {
                BookWithAuthorList = await libraryService.FindBooksAsync(bookFilterDto, isAvailable: true)
            };
            return Ok(dto);
        }
    }
}
