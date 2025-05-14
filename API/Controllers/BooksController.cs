using Core.DTOs;
using Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{    
    public class BooksController(IBookService bookService) : BaseApiController
    {
      
        private readonly IBookService bookService = bookService;

        [HttpGet]
        public async Task<ActionResult<BookWithAuthorListDto>> GetAll()
        {
            return Ok(await bookService.GetBooksWithAuthorsAsync());
        }

        [HttpGet("{id:int}")]           // GET /api/books/5
        public async Task<ActionResult<BookWithAuthorListDto>> GetById(int id)
        {
            var result = await bookService.GetBookById(id);
            if (result == null) 
            {
                return NotFound($"Book with ID {id} not found.");
            }
            return Ok(result);
        }

        [HttpGet("edit/{id:int}")]       // GET /api/books/edit/5
        public async Task<ActionResult<BookForEditDto>> GetForEdit(int id)
        {
            BookForEditDto result = await bookService.GetBookForEditingById(id);
            if (!result.IsSuccess)
            {
                return NotFound($"{result.ErrorMessage}");
            }
            return Ok(result);
        }

        [HttpPost]
        public async Task<ActionResult<InsertUpdateResultDto>> Create(BookSaveDto item)
        {
            if (item.Id != 0)
                return BadRequest("ID should be zero for a insert.");

            item.PublishedDate = item.PublishedDate.ToLocalTime();
            var result = await bookService.SaveBookAsync(item);
            if (result == null || !result.IsSuccess)
            {
                return StatusCode(500, "An error occurred while inserting the book."); ;
            }
            return CreatedAtAction(nameof(GetById), new { id = result.EntityId }, result);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, BookSaveDto item)
        {
            if (item.Id != id)
                return BadRequest("ID in URL does not match ID in body.");
            
            var exists = await bookService.IsExistsAsync(id);
            if (!exists)
                return NotFound($"Book with ID {id} not found.");

            item.PublishedDate = item.PublishedDate.ToLocalTime();
            var result = await bookService.SaveBookAsync(item);
            if (result.IsSuccess)
            {
                return NoContent();
            }

            // Log the result.ErrorMessage internally
            // logger.LogError(result.ErrorMessage); // assuming logging

            return StatusCode(500, "An error occurred while updating the item.");
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var exists = await bookService.IsExistsAsync(id);
            if (!exists)
                return NotFound($"Book with ID {id} not found.");

            var result = await bookService.DeleteAsync(id);
            if (result.IsSuccess)
            {
                return NoContent();
            }
            return StatusCode(500, $"An error occurred while deleting the item with Id {id}.");
        }

        /// <summary>
        /// This will update the received image into - wwwroot/images/booksImgs folder - of the API server
        /// </summary>
        /// <param name="file">Image uploaded</param>
        /// <returns>If successful the image path will be returned</returns>
        [HttpPost("uploadBookImage")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> UploadBookImage([FromForm] UploadBookImageRequestDto requestDto)
        {
            if (requestDto == null || requestDto.File == null || requestDto.File.Length == 0)
            {
                return BadRequest("No file uploaded.");
            }

            var uploadPath = Path.Combine("wwwroot", "apiImages", "booksImgs");
            Directory.CreateDirectory(uploadPath);

            var fileName = Guid.NewGuid() + Path.GetExtension(requestDto.File.FileName);
            var filePath = Path.Combine(uploadPath, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await requestDto.File.CopyToAsync(stream);
            }

            return Ok(new { path = $"{fileName}" });
        }

    }
}
