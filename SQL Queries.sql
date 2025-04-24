use LMS;

SELECT * from [Authors];

SELECT * from Books;

select * from AuthorBook;

--select * from [__EFMigrationsHistory];


select b.Id 'BookId', b.Title, a.Id 'AuthorId', a.Name 'Author'  from AuthorBook t 
	inner join Authors a on t.AuthorsId = a.Id
	inner join Books b on t.BooksId = b.Id
	order by b.Id desc;