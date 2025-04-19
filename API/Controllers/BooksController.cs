using Core.DTOs;
using Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{    
    public class BooksController(ILibraryService libraryService) : BaseController
    {
        private readonly ILibraryService libraryService = libraryService;

        [HttpGet]
        public async Task<ActionResult<BookWithAuthorListDto>> GetAll()
        {
            return await libraryService.GetBooksWithAuthorsAsync();
        }

        //private readonly AppDbContext context = context;

        //[HttpGet]
        //public async Task<ActionResult<IEnumerable<Book>>> GetAll()
        //{
        //    return await context.Books.ToListAsync();
        //}


        //[HttpGet("{id:int}")]
        //public async Task<ActionResult<Book>> GetById(int id)
        //{
        //    var item = await context.Books.FindAsync(id);
        //    if (item == null)
        //        return NotFound();
        //    return item;
        //}

        //[HttpPost]
        //public async Task<ActionResult<Book>> Create(Book item)
        //{ 
        //    context.Books.Add(item);
        //    await context.SaveChangesAsync();
        //    return item;
        //}

        //[HttpPut("{id:int}")]
        //public async Task<ActionResult> Update(int id, Book item)
        //{
        //    if (!await ItemExistsAsync(id) || item.Id != id)
        //        return BadRequest("Cannot update this item");

        //    context.Entry(item).State = EntityState.Modified;
        //    await context.SaveChangesAsync();
        //    return NoContent();
        //}

        //[HttpDelete("{id:int}")]
        //public async Task<ActionResult<Book>> Delete(int id)
        //{
        //    var item = await GetItemAsync(id);
        //    if (item == null)
        //        return NotFound($"Item not found for deletion");


        //    context.Books.Remove(item);
        //    await context.SaveChangesAsync();
        //    return NoContent();
        //}

        //private async Task<Book?> GetItemAsync(int id)
        //{
        //    return await context.Books.FindAsync(id);
        //}

        //private async Task<bool> ItemExistsAsync(int id)
        //{
        //    return await context.Books.AnyAsync(x => x.Id == id);
        //}
    }
}
