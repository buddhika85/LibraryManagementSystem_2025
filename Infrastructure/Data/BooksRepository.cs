﻿using Core.DTOs;
using Core.Entities;
using Core.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data
{
    public class BooksRepository : GenericRepository<Book>, IBooksRepository
    {
        public BooksRepository(AppDbContext context) : base(context)
        {
        }

        /// <summary>
        /// Filter book by Id - returns author information also
        /// </summary>
        /// <param name="id">int</param>
        /// <returns>Book with navigation property author</returns>
        public async Task<Book?> GetBookByIdAsync(int id)
        {
            return await context.Books.Include(x => x.Authors).SingleOrDefaultAsync(x => x.Id == id);
        }

        public async Task<IList<Book>> GetBooksIncludingAuthorsAsync()
        {
            return await context.Books.AsNoTracking().Include(x => x.Authors).AsNoTracking().ToListAsync();
        }

        public async Task<IList<Book>> GetBooksIncludingAuthorsAsync(BookFilterDto bookFilterDto, bool isAvailable)
        {
            var query = context.Books.Include(x => x.Authors)
                                .Where(x => x.IsAvailable == isAvailable);

            if (bookFilterDto.AuthorIds.Any())
            {
                query = query.Where(x => x.Authors.Any(a => bookFilterDto.AuthorIds.Contains(a.Id)));
            }

            if (bookFilterDto.BookGenres.Any())
            {
                query = query.Where(x => bookFilterDto.BookGenres.Contains((int)x.Genre));
            }

            return await query.ToListAsync();
        }
    }
}
