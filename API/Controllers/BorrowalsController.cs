
using API.Helpes;
using Core.DTOs;
using Core.Enums;
using Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Security.Claims;

namespace API.Controllers
{

    [Authorize]
    public class BorrowalsController : BaseApiController
    {
        private readonly IBorrowalsService borrowalsService;
        private readonly ILibraryService libraryService;
        private readonly IUserService userService;        
        private readonly AppSettingsReader customSettings;

        public BorrowalsController(IBorrowalsService borrowalsService, ILibraryService libraryService, IUserService userService,
            IOptions<AppSettingsReader> customSettings)
        {
            this.borrowalsService = borrowalsService;
            this.libraryService = libraryService;
            this.userService = userService;
            this.customSettings = customSettings.Value;
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
            var dto = new BorrowFormDto
            {
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


        [Authorize(Roles = "Admin,Staff")]
        [HttpPost("borrow-book")]
        public async Task<ActionResult<BorrowResultDto>> BorrowBook(BookBorrowRequestDto bookFilterDto)
        {
            bookFilterDto.StartDate = bookFilterDto.StartDate.ToLocalTime();
            bookFilterDto.EndDate = bookFilterDto.EndDate.ToLocalTime();
            var result = await borrowalsService.BorrowBook(bookFilterDto);
            return Ok(result);
        }

        [Authorize(Roles = "Admin,Staff")]
        [HttpGet("borrowal-return-info/{borrowalId:int}")]
        public async Task<ActionResult<BorrowalReturnInfoDto>> GetBorrowalReturnInfoDto(int borrowalId)
        {
            var dto = await borrowalsService.GetBorrowalReturnInfoDto(borrowalId, customSettings.PerDayLateFeeDollars);
            return Ok(dto);
        }

        [Authorize(Roles = "Admin,Staff")]
        [HttpPost("return-book")]
        public async Task<ActionResult<ReturnResultDto>> ReturnBook(ReturnsAcceptDto returnsAcceptDto)
        {
            var dto = await borrowalsService.ReturnBookAsync(returnsAcceptDto);
            return Ok(dto);
        }

        [Authorize(Roles = "Admin,Staff,Member")]
        [HttpGet("borrowal-summary-member")]
        public async Task<ActionResult<BorrowalSummaryListDto>> GetBorrowalSummaryForMember(string memberEmail)
        {
            var dto = await borrowalsService.GetBorrowalSummaryForMemberAsync(memberEmail);
            return Ok(dto);
        }

        [Authorize(Roles = "Member")]                       // only member users allowed
        [HttpGet("borrowal-summary-logged-member")]
        public async Task<ActionResult<BorrowalSummaryListDto>> GetBorrowalSummaryForMember()
        {
            var loggedInMemberEmail = User.FindFirst(ClaimTypes.Name)?.Value;           
            if (loggedInMemberEmail == null)
                return Unauthorized();
 
            var dto = await borrowalsService.GetBorrowalSummaryForMemberAsync(loggedInMemberEmail);
            return Ok(dto);
        }
    }
}
