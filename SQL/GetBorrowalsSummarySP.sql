-- ===============================================================
-- To retrieve all borrowals related information by users email
-- ===============================================================
use LMS;

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

ALTER PROCEDURE GetBorrowalsSummarySP 
	
	@memberEmail nvarchar(256) = null
AS
BEGIN
	
	SET NOCOUNT ON;

    -- select statement
	select 
		b.Id 'BorrowalsId', x.BorrowalDate, x.DueDate, x.BorrowalStatus,
		r.WasPaid, r.WasOverdue, r.AmountAccepted 'AmountPaid', r.LateDays 'LateDaysOnPayment', concat(u2.FirstName, ' ',  u2.LastName) 'PaymentAcceptedBy',
		x.BookId, b.Title 'BookId', b.PictureUrl 'BookPic', b.Genre 'BookGenre',
		u.Email 'BorrowerEmail', concat(u.FirstName, ' ', u.LastName) 'BorrowerName'

		from Borrowals x 
		left join BorrowalReturns r on x.Id = r.BorrowalsId
		left join AspNetUsers u2 on r.AppUserId = u2.Id
		inner join Books b on x.BookId = b.Id
		inner join AspNetUsers u on x.AppUserId = u.Id
		
		where @memberEmail is null or u.Email = @memberEmail 
		
		order by x.DueDate, b.Id;
END
GO


----

-- create indexes for fast filering 
CREATE INDEX IX_Borrowals_Email ON AspNetUsers(Email);
CREATE INDEX IX_Borrowals_DueDate ON Borrowals(DueDate);

----
DECLARE	@return_value int

EXEC	@return_value = [dbo].[GetBorrowalsSummarySP]
		@memberEmail = 'member1@gmail.com'

SELECT	'Return Value' = @return_value