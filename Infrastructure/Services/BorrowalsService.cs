using AutoMapper;
using Core.DTOs;
using Core.Entities;
using Core.Enums;
using Core.Interfaces;
using Infrastructure.Helpers;

namespace Infrastructure.Services
{
    public class BorrowalsService : IBorrowalsService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;
        private readonly IBorrowalsRepository borrowalsRepository;
        private readonly IUserRepository userRepository;
        private readonly IBooksRepository booksRepository;
       

        public BorrowalsService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
            borrowalsRepository = unitOfWork.BorrowalsRepository;
            userRepository = unitOfWork.UserRepository;
            booksRepository = unitOfWork.BookRepository;
        }
        
        public async Task<BorrowalsDisplayListDto> GetAllBorrowalsAsync()
        {
            var allBrrowals = await borrowalsRepository.GetAllBorrowalsWithNavPropsAsync();
            return new BorrowalsDisplayListDto
            {
                BorrowalsList = mapper.Map<IReadOnlyList<BorrowalsDisplayDto>>(allBrrowals)
            };
        }

        #region borrow book

        //public async Task<BorrowResultDto> BorrowBook(BookBorrowRequestDto bookBorrowRequest)
        //{
        //    var dto = new BorrowResultDto
        //    {
        //        StartDate = bookBorrowRequest.StartDate,
        //        EndDate = bookBorrowRequest.EndDate,
        //        MemberEmail = bookBorrowRequest.Email
        //    };
        //    if (bookBorrowRequest.StartDate < DateOnly.FromDateTime(DateTime.Today))
        //    {
        //        dto.ErrorMessage = "Borrowal start date cannot be in the past.";
        //        return dto;
        //    }

        //    if (bookBorrowRequest.EndDate <= bookBorrowRequest.StartDate)
        //    {
        //        dto.ErrorMessage = "Return date must be after the borrowal start date.";
        //        return dto;
        //    }

        //    var user = await userRepository.GetUserByRoleAndEmailAsync(bookBorrowRequest.Email, UserRoles.Member);
        //    if (user == null)
        //    {
        //        dto.ErrorMessage = $"A member with {bookBorrowRequest.Email} does not exist";
        //        return dto;
        //    }
        //    dto.MemberFullName = $"{user.FirstName} {user.LastName}";

        //    var book = await booksRepository.GetBookByIdAsync(bookBorrowRequest.BookId);
        //    if (book == null)
        //    {
        //        dto.ErrorMessage = $"A book with {bookBorrowRequest.BookId} does not exist";
        //        return dto;
        //    }
        //    dto.BookTitle = book.Title;
        //    dto.BookAuthors = string.Join(", ", book.Authors);

        //    if (!book.IsAvailable)
        //    {
        //        dto.ErrorMessage = $"Book {bookBorrowRequest.BookId} - {book.Title} is currently unavailable";
        //        return dto;
        //    }

        //    await unitOfWork.BeginTransactionAsync();
        //    try
        //    {
        //        // borrow
        //        borrowalsRepository.Add(
        //            new Borrowals
        //            {
        //                AppUser = user,
        //                AppUserId = user.Id,
        //                BookId = book.Id,
        //                Book = book,
        //                BorrowalStatus = BorrowalStatus.Out,
        //                BorrowalDate = bookBorrowRequest.StartDate,
        //                DueDate = bookBorrowRequest.EndDate
        //            }
        //        );

        //        // set availability of book
        //        book.IsAvailable = false;
        //        booksRepository.Update(book);

        //        await unitOfWork.SaveAllAsync();                // Save changes first
        //        await unitOfWork.CommitTransactionAsync();      // Only commit if everything succeeds
        //    }
        //    catch (Exception ex)
        //    {
        //        await unitOfWork.RollbackTransactionAsync();    // Roll back if anything fails
        //        //throw new Exception("Transaction failed", ex);

        //        dto.ErrorMessage = $"Saving borrowal transaction failed";
        //    }

        //    return dto;
        //}


        /// <summary>
        /// Borrow book - ochestrates the functionlity and makes use of helper methods
        /// </summary>
        /// <param name="bookBorrowRequest">Book Borrow Request Information</param>
        /// <returns>BorrowResultDto containing information of the transaction</returns>
        public async Task<BorrowResultDto> BorrowBook(BookBorrowRequestDto bookBorrowRequest)
        {
            var dto = new BorrowResultDto
            {
                StartDate = bookBorrowRequest.StartDate,
                EndDate = bookBorrowRequest.EndDate,
                MemberEmail = bookBorrowRequest.Email
            };

            // validations
            var validationError = ValidateBorrowRequest(bookBorrowRequest);
            if (validationError != null)
            {
                dto.ErrorMessage = validationError;
                return dto;
            }

            // fetch DB records needed
            var (user, book) = await GetBorrowEntities(bookBorrowRequest);

            // validation of fetched DB records
            if (user == null || book == null)
            {
                dto.ErrorMessage = user == null ? $"A member with {bookBorrowRequest.Email} does not exist"
                                                : $"A book with {bookBorrowRequest.BookId} does not exist";
                return dto;
            }

            dto.MemberFullName = $"{user.FirstName} {user.LastName}";
            dto.BookTitle = book.Title;
            dto.BookAuthors = string.Join(", ", book.Authors.Select(x => x.Name));

            if (!book.IsAvailable)
            {
                dto.ErrorMessage = $"Book {bookBorrowRequest.BookId} - {book.Title} is currently unavailable";
                return dto;
            }

            // transaction - borrowing book
            var transactionSuccessful = await ProcessBorrowTransaction(bookBorrowRequest, user, book);
            if (!transactionSuccessful)
            {
                dto.ErrorMessage = "Saving borrowal transaction failed";
            }

            return dto;
        }

        /// <summary>
        /// Validating book borrow request
        /// </summary>
        /// <param name="bookBorrowRequest">Book Borrow Request Information</param>
        /// <returns>Error message if an error or null</returns>
        private string? ValidateBorrowRequest(BookBorrowRequestDto bookBorrowRequest)
        {
            if (bookBorrowRequest.StartDate < DateTime.Today)
                return "Borrowal start date cannot be in the past.";

            if (bookBorrowRequest.EndDate <= bookBorrowRequest.StartDate)
                return "Return date must be after the borrowal start date.";

            return null;
        }

        /// <summary>
        /// Helper for featching DB records related to borrowal
        /// </summary>
        /// <param name="bookBorrowRequest"></param>
        /// <returns>Returns if Book and member object could be found</returns>
        private async Task<(AppUser?, Book?)> GetBorrowEntities(BookBorrowRequestDto bookBorrowRequest)
        {
            var user = await userRepository.GetUserByRoleAndEmailAsync(bookBorrowRequest.Email, UserRoles.Member);
            var book = await booksRepository.GetBookByIdAsync(bookBorrowRequest.BookId);

            return (user, book);
        }

        /// <summary>
        /// Processes borrowal as a single transaction.
        /// Transaction has 2 steps.
        ///     1. Insert borrowal record
        ///     2. Update book record to make isAvailable = false
        /// If one of these steps fail entire transaction will be rollbacked.
        /// </summary>
        /// <param name="borrowBookRequest">Borrowal information</param>
        /// <param name="user">Member</param>
        /// <param name="book">Book</param>
        /// <returns>true if entire transaction is successful</returns>
        private async Task<bool> ProcessBorrowTransaction(BookBorrowRequestDto borrowBookRequest, AppUser user, Book book)
        {
            // starting transaction
            await unitOfWork.BeginTransactionAsync();
            try
            {
                // 1. Insert borrowal record
                borrowalsRepository.Add(new Borrowals
                {
                    AppUser = user,
                    AppUserId = user.Id,
                    BookId = book.Id,
                    Book = book,
                    BorrowalStatus = BorrowalStatus.Out,
                    BorrowalDate = DateOnly.FromDateTime(borrowBookRequest.StartDate),
                    DueDate = DateOnly.FromDateTime(borrowBookRequest.EndDate)
                });

                // 2. Update book record to make isAvailable = false
                book.IsAvailable = false;
                booksRepository.Update(book);

                // save to local DBSet
                await unitOfWork.SaveAllAsync();

                // commit to DB
                await unitOfWork.CommitTransactionAsync();
                return true;
            }
            catch
            {
                // rolling back the transtion if failure
                await unitOfWork.RollbackTransactionAsync();
                return false;
            }
        }

        #endregion borrow book

        public async Task<BorrowalReturnInfoDto> GetBorrowalReturnInfoDto(int borrowalId, decimal perDayLateFeeDollars)
        {
            var dto = new BorrowalReturnInfoDto
            {
                BorrowalId = borrowalId,
                PerDayLateFeeDollars = perDayLateFeeDollars
            };

            var borrowal = await borrowalsRepository.GetAllBorrowalWithNavPropsAsync(borrowalId);
            if (borrowal == null)
            {
                dto.ErrorMessage = $"Borrowal with such Id - {borrowalId} does not exist.";
                return dto;
            }

            MapBorrowalToDto(borrowal, dto);

            var validateError = ValidateBorrowalStatusForReturn(borrowal);
            if (validateError != null)
            {
                dto.ErrorMessage = validateError;
                return dto;
            }

            ApplyLateFeeInfo(perDayLateFeeDollars, borrowal.DueDate, dto);

            return dto;
        }

        private void MapBorrowalToDto(Borrowals borrowal, BorrowalReturnInfoDto dto)
        {
            dto.BorrowalDateStr = borrowal.BorrowalDate.ToShortDateString();
            dto.DueDateStr = borrowal.DueDate.ToShortDateString();
            dto.BorrowalsDisplayDto = mapper.Map<BorrowalsDisplayDto>(borrowal);
        }

        private static void ApplyLateFeeInfo(decimal perDayLateFeeDollars, DateOnly dueDate, BorrowalReturnInfoDto dto)
        {
            var gapDays = DateTimeUtils.GetDayGap(dueDate);
            if (gapDays > 0)
            {                
                dto.LateDays = gapDays;
                dto.IsOverdue = true;
                dto.PerDayLateFeeDollars = gapDays * perDayLateFeeDollars;
            }            
        }

        private string? ValidateBorrowalStatusForReturn(Borrowals borrowal)
        {
            if (borrowal.BorrowalStatus != BorrowalStatus.Out)
            {
                return $"Borrowal Id - {borrowal.Id} has a status of {borrowal.BorrowalStatus}. So cannot return.";
            }
            return null;
        }

        
    }
}
