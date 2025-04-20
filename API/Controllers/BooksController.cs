using Core.DTOs;
using Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{    
    public class BooksController(IBookService bookService) : BaseController
    {
      
        private readonly IBookService bookService = bookService;

        [HttpGet]
        public async Task<ActionResult<BookWithAuthorListDto>> GetAll()
        {
            return Ok(await bookService.GetBooksWithAuthorsAsync());
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<BookWithAuthorListDto>> GetById(int id)
        {
            var result = await bookService.GetBookById(id);
            if (result == null) 
            {
                return NotFound($"Book with ID {id} not found.");
            }
            return Ok(result);
        }

        [HttpPost]
        public async Task<ActionResult<ResultDto>> Create(BookSaveDto item)
        {
            if (item.Id != 0)
                return BadRequest("ID should be zero for a insert.");

            var result = await bookService.SaveBookAsync(item);
            if (result == null || !result.IsSuccess)
            {
                return StatusCode(500, "An error occurred while inserting the book."); ;
            }
            return CreatedAtAction(nameof(GetById), new { id = result.EntityId }, null);
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult<ResultDto>> Update(int id, BookSaveDto item)
        {
            if (item.Id != id)
                return BadRequest("ID in URL does not match ID in body.");

            var exists = await bookService.IsExistsAsync(id);
            if (!exists)
                return NotFound();

            var result = await bookService.SaveBookAsync(item);
            if (result.IsSuccess)
            {
                return NoContent();
            }

            // Log the result.ErrorMessage internally
            // logger.LogError(result.ErrorMessage); // assuming logging

            return StatusCode(500, "An error occurred while updating the item.");
        }
    }
}
