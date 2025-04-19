use LMS;

SELECT * from [Authors];

SELECT * from Books;

select * from AuthorBook;

select * from [__EFMigrationsHistory];


select * from AuthorBook t 
	inner join Authors a on t.AuthorId = a.Id
	inner join Books b on t.BookId = b.Id;