using AutoMapper;
using Core.DTOs;
using Core.Entities;
using Core.Enums;
using Core.Interfaces;
using Infrastructure.Data;
using Infrastructure.Helpers;

namespace Infrastructure.Services
{
    public class BorrowalsService : IBorrowalsService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;
        private readonly IBorrowalsRepository borrowalsRepository;
        private readonly IBorrowalReturnsRepository borrowalReturnsRepository;
        private readonly IUserRepository userRepository;
        private readonly IBooksRepository booksRepository;
       

        public BorrowalsService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
            borrowalsRepository = unitOfWork.BorrowalsRepository;
            borrowalReturnsRepository = unitOfWork.BorrowalReturnsRepository;
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


        #region get borrowal return info
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

        #endregion get borrowal return info

        #region return book

        /// <summary>
        /// Ochestrator method for book return.
        /// Calls multiple methods for validations, DB record retrieval, transaction processing on DB
        /// </summary>
        /// <param name="returnsAcceptDto">Book return information</param>
        /// <returns>result of returning book</returns>
        public async Task<ReturnResultDto> ReturnBookAsync(ReturnsAcceptDto returnsAcceptDto)
        {
            var dto = new ReturnResultDto();

            // payment validation
            var errorMessage = ValidatePayment(returnsAcceptDto);
            if (errorMessage != null)
            {
                dto.ErrorMessage = errorMessage;
                return dto;
            }

            // fetch DB records needed
            var (staffMember, borrowal) = await FetchReturnEntities(returnsAcceptDto.Email, returnsAcceptDto.BorrowalId);

            // validate DB entities
            errorMessage = ValidateBorrowReturnEntities(returnsAcceptDto.Email, returnsAcceptDto.BorrowalId, staffMember, borrowal);
            if (errorMessage != null)
            {
                dto.ErrorMessage = errorMessage;
                return dto;
            }

            var transactionSuccessful = await ProcessReturnTransaction(returnsAcceptDto, borrowal!, staffMember!);       // 2 params will never be null here
            if (!transactionSuccessful)
            {
                dto.ErrorMessage = "Saving borrowal transaction failed";                
            }

            return dto;
        }

        /// <summary>
        /// Validate whether payment was accepted by staff member or not
        /// </summary>
        /// <param name="returnsAcceptDto">Book return information</param>
        /// <returns>if error an error message else null</returns>
        private string? ValidatePayment(ReturnsAcceptDto returnsAcceptDto)
        {
            if (returnsAcceptDto.IsOverdue && !returnsAcceptDto.Paid)
            {
                return $"Staff member with email {returnsAcceptDto.Email} has not accepted the payment.";
            }
            return null;
        }

        /// <summary>
        /// Validate borrowal return related enities 
        /// </summary>
        /// <param name="staffEmail">staff member associated with return</param>
        /// <param name="borrowalId">borrowal record ID</param>
        /// <param name="staffMember">staff member record</param>
        /// <param name="borrowal">original borrowal record</param>
        /// <returns></returns>
        private string? ValidateBorrowReturnEntities(string staffEmail, int borrowalId, AppUser? staffMember, Borrowals? borrowal)
        {
            if (staffMember == null)
                return $"Staff member with email {staffEmail} does not exists";
            if (borrowal == null)
                return $"Borrowal with Id {borrowalId} does not exists";
            return null;
        }

        /// <summary>
        /// Fetch borrowal return entities from DB
        /// </summary>
        /// <param name="staffMember">staff member record</param>
        /// <param name="borrowalId">borrowal record ID</param>
        /// <returns>Task<(AppUser?, Borrowals?)></returns>
        private async Task<(AppUser?, Borrowals?)> FetchReturnEntities(string staffEmail, int borrowalId)
        {
            // get staff, if not admin user with email provided
            var staffMember = await userRepository.GetUserByRoleAndEmailAsync(staffEmail, UserRoles.Staff)
                             ?? await userRepository.GetUserByRoleAndEmailAsync(staffEmail, UserRoles.Admin);

            // borrowal record along with book navigation property
            var borrowal = await borrowalsRepository.GetAllBorrowalWithNavPropsAsync(borrowalId);

            return (staffMember, borrowal);
        }

        /// <summary>
        /// Book return transaction processing and commiting changes to DB. 3 steps to either all succeed of all fail.
        ///     1. insert borrowalReturn record
        ///     2. update borrowalStatus of Borrowals table
        ///     3. update book isAvailable to true
        /// </summary>
        /// <param name="returnsAcceptDto">return related information</param>
        /// <param name="borrowal">original borrowal record</param>
        /// <param name="staffMember">staff member record</param>
        /// <returns>true if transaction successful, else false if transaction is not successful</returns>
        private async Task<bool> ProcessReturnTransaction(ReturnsAcceptDto returnsAcceptDto, Borrowals borrowal, AppUser staffMember)
        {
            // starting transaction
            await unitOfWork.BeginTransactionAsync();
            try
            {
                // 1. insert borrowalReturn record
                var borrowalsReturn = new BorrowalReturn
                { 
                    AppUser = staffMember,
                    AppUserId = staffMember.Id,
                    Borrowals = borrowal,
                    BorrowalsId = borrowal.Id,
                    AmountAccepted = returnsAcceptDto.AmountAccepted,
                    WasPaid = returnsAcceptDto.Paid,
                    WasOverdue = returnsAcceptDto.IsOverdue
                };
                borrowalReturnsRepository.Add(borrowalsReturn);

                // 2. update borrowalStatus of Borrowals table
                borrowal.BorrowalStatus = BorrowalStatus.Returned;
                borrowalsRepository.Update(borrowal);

                // 3. update book isAvailable to true
                borrowal.Book.IsAvailable = true;
                borrowalsRepository.Update(borrowal);

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
               
        #endregion return book
    }
}
