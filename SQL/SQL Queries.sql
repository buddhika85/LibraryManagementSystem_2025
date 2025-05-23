use LMS;

SELECT * from [Authors];

SELECT * from Books;
--update Books set PictureUrl = ''

select * from AuthorBook;

--select * from [__EFMigrationsHistory];


select b.Id 'BookId', b.Title, a.Id 'AuthorId', a.Name 'Author', b.pictureUrl  from AuthorBook t 
	inner join Authors a on t.AuthorsId = a.Id
	inner join Books b on t.BooksId = b.Id
	order by b.Id desc;


-- identity
select * from Addresses;
select * from AspNetRoles;

select * from AspNetUsers;
select * from AspNetUserRoles;

select t.UserName, r.[Name] as 'Role', a.Id 'Address ID', a.Line1 'Address Line 1', t.IsActive from AspNetUsers t 
	left join AspNetUserRoles ur on t.Id = ur.UserId
	left join AspNetRoles r on ur.RoleId = r.Id
	left join Addresses a on t.AddressId = a.Id;

--select * from AspNetUserClaims;
--select * from AspNetRoleClaims;
--select * from AspNetUserTokens;

--delete from Addresses where Id not in (2, 3)

--update AspNetUsers set IsActive = 0 where Email = 'm2@g.c';


--delete from AspNetUsers where FirstName = 'a';


select * from Borrowals;
--update Books set IsAvailable = 0 where Id = 2;
--insert into Borrowals values (GETDATE(), GETDATE() + 7, 2, 2, 'ef22f114-942d-4b85-a7ae-abeddbfe817c')

select * from BorrowalReturns;
