
using Core.DTOs;
using Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{

   
    public class BorrowalsController : BaseApiController
    {
        private readonly IBorrowalsService borrowalsService;

        public BorrowalsController(IBorrowalsService borrowalsService)
        {
            this.borrowalsService = borrowalsService;
        }

        public async Task<ActionResult<BorrowalsDisplayListDto>> GetAllBorrowals()
        {
            BorrowalsDisplayListDto dto = await borrowalsService.GetAllBorrowalsAsync();
            return Ok(dto);
        }
    }
}
