
using Core.DTOs;
using Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{

    [Authorize]
    public class BorrowalsController : BaseApiController
    {
        private readonly IBorrowalsService borrowalsService;

        public BorrowalsController(IBorrowalsService borrowalsService)
        {
            this.borrowalsService = borrowalsService;
        }

        [Authorize(Roles = "Admin,Staff")]
        [HttpGet("all-borrowals")]
        public async Task<ActionResult<BorrowalsDisplayListDto>> GetAllBorrowals()
        {
            var dto = await borrowalsService.GetAllBorrowalsAsync();
            return Ok(dto);
        }
    }
}
