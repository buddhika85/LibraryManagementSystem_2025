using Core.DTOs;
using Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class AuthorsController(ILibraryService libraryService) : BaseApiController
    {
        private readonly ILibraryService libraryService = libraryService;

        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<AuthorDto>>> GetAll()
        {
            var result = await libraryService.GetAuthorsAsync();
            return Ok(result);
        }
    }
}
