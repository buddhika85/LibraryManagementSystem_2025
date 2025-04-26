use LMS;

SELECT * from [Authors];

SELECT * from Books;

select * from AuthorBook;

--select * from [__EFMigrationsHistory];


select b.Id 'BookId', b.Title, a.Id 'AuthorId', a.Name 'Author'  from AuthorBook t 
	inner join Authors a on t.AuthorsId = a.Id
	inner join Books b on t.BooksId = b.Id
	order by b.Id desc;


-- identity
select * from Addresses;
select * from AspNetRoles;

select * from AspNetUsers;
select * from AspNetUserRoles;

select t.UserName, r.[Name] as 'Role', a.Line1 'Address Line 1' from AspNetUsers t 
	left join AspNetUserRoles ur on t.Id = ur.UserId
	left join AspNetRoles r on ur.RoleId = r.Id
	left join Addresses a on t.AddressId = a.Id;

--select * from AspNetUserClaims;
--select * from AspNetRoleClaims;
--select * from AspNetUserTokens;

