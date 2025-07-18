﻿using Core.DTOs;
using Core.Entities;
using Core.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data
{
    public class BorrowalsRepository : GenericRepository<Borrowals>, IBorrowalsRepository
    {
        public BorrowalsRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<IReadOnlyList<Borrowals>> GetAllBorrowalsWithNavPropsAsync()
        {
            return await context.Borrowals.Include(x => x.Book).Include(x => x.Book.Authors).Include(x => x.AppUser).ToListAsync();
        }

        public async Task<Borrowals?> GetAllBorrowalWithNavPropsAsync(int borrowalId)
        {
            return await context.Borrowals.Where(x => x.Id == borrowalId).Include(x => x.Book).Include(x => x.Book.Authors).Include(x => x.AppUser).SingleOrDefaultAsync();
        }

        public async Task<IReadOnlyCollection<BorrowalSummaryDto>> GetBorrowalSummaryForMemberAsync(string memberEmail)
        {
            return await context.BorrowalSummaryDtos
                .FromSqlInterpolated($"EXEC GetBorrowalsSummarySP {memberEmail}")                  // FromSqlInterpolated prevents sql injection    
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<IReadOnlyList<Borrowals>> SearchBorrowalsWithNavPropsAsync(BorrowalsSearchDto searchDto)
        {
            // laxy loading - building complex query first 
            var query = context.Borrowals.AsNoTracking()
                 .Include(x => x.Book).AsNoTracking()
                 .Include(x => x.Book.Authors).AsNoTracking()
                 .Include(x => x.AppUser).AsNoTracking()
                .Where(x =>
                        (string.IsNullOrWhiteSpace(searchDto.BookName) || x.Book.Title.Contains(searchDto.BookName))
                        && (searchDto.AuthorIds == null || !searchDto.AuthorIds.Any() || x.Book.Authors.Any(author => searchDto.AuthorIds.Contains(author.Id)))
                    && (searchDto.Genres == null || !searchDto.Genres.Any() || searchDto.Genres.Contains(x.Book.Genre))
                    && (string.IsNullOrWhiteSpace(searchDto.MemberName)
                        || (string.IsNullOrWhiteSpace(x.AppUser.FirstName) || x.AppUser.FirstName.Contains(searchDto.MemberName))
                        || (string.IsNullOrWhiteSpace(x.AppUser.LastName) || x.AppUser.LastName.Contains(searchDto.MemberName)))
                    && (string.IsNullOrWhiteSpace(searchDto.MemberEmail) || string.IsNullOrWhiteSpace(x.AppUser.Email) || x.AppUser.Email.Contains(searchDto.MemberEmail))
                    && ((searchDto.BorrowedOn == null || x.BorrowalDate >= DateOnly.FromDateTime(searchDto.BorrowedOn.Value))       // >= BorrowalDate 
                        && (searchDto.DueOn == null || x.DueDate <= DateOnly.FromDateTime(searchDto.DueOn.Value)))                 // && <= DueDate                  
                    );

            // after building query, query the DB only once
            var searchResult = await query.AsNoTracking().ToListAsync();

            return searchResult;
        }
    }
}
