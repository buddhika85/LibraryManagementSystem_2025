using Core.Entities;
using Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{  
    public class AuthorsController(AppDbContext context) : BaseController
    {
        private readonly AppDbContext context = context;

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Author>>> GetAll()
        {
            return await context.Authors.ToListAsync();
        }


        [HttpGet("{id:int}")]
        public async Task<ActionResult<Author>> GetById(int id)
        {
            var item = await context.Authors.FindAsync(id);
            if (item == null)
                return NotFound();
            return item;
        }

        [HttpPost]
        public async Task<ActionResult<Author>> Create(Author item)
        {
            context.Authors.Add(item);
            await context.SaveChangesAsync();
            return item;
        }


        [HttpPut("{id:int}")]
        public async Task<ActionResult> Update(int id, Author item)
        {
            if (!await ItemExistsAsync(id) || item.Id != id)
                return BadRequest("Cannot update this item");

            context.Entry(item).State = EntityState.Modified;
            await context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult<Author>> Delete(int id)
        {
            var item = await GetItemAsync(id);
            if (item == null)
                return NotFound($"Item not found for deletion");


            context.Authors.Remove(item);
            await context.SaveChangesAsync();
            return NoContent();
        }

        private async Task<Author?> GetItemAsync(int id)
        {
            return await context.Authors.FindAsync(id);
        }

        private async Task<bool> ItemExistsAsync(int id)
        {
            return await context.Authors.AnyAsync(x => x.Id == id);
        }
    }
}
