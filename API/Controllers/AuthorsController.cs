using Core.DTOs;
using Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class AuthorsController(ILibraryService libraryService) : BaseController
    {
        private readonly ILibraryService libraryService = libraryService;

        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<AuthorDto>>> GetAll()
        {
            var result = await libraryService.GetAuthorsAsync();
            return Ok(result);
        }


        //[HttpGet("{id:int}")]
        //public async Task<ActionResult<Author>> GetById(int id)
        //{
        //    var item = await context.Authors.FindAsync(id);
        //    if (item == null)
        //        return NotFound();
        //    return item;
        //}

        //[HttpPost]
        //public async Task<ActionResult<Author>> Create(Author item)
        //{
        //    context.Authors.Add(item);
        //    await context.SaveChangesAsync();
        //    return item;
        //}


        //[HttpPut("{id:int}")]
        //public async Task<ActionResult> Update(int id, Author item)
        //{
        //    if (!await ItemExistsAsync(id) || item.Id != id)
        //        return BadRequest("Cannot update this item");

        //    context.Entry(item).State = EntityState.Modified;
        //    await context.SaveChangesAsync();
        //    return NoContent();
        //}

        //[HttpDelete("{id:int}")]
        //public async Task<ActionResult<Author>> Delete(int id)
        //{
        //    var item = await GetItemAsync(id);
        //    if (item == null)
        //        return NotFound($"Item not found for deletion");


        //    context.Authors.Remove(item);
        //    await context.SaveChangesAsync();
        //    return NoContent();
        //}

        //private async Task<Author?> GetItemAsync(int id)
        //{
        //    return await context.Authors.FindAsync(id);
        //}

        //private async Task<bool> ItemExistsAsync(int id)
        //{
        //    return await context.Authors.AnyAsync(x => x.Id == id);
        //}
    }
}
