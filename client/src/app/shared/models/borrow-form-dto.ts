import { AuthorDto } from "./author-dto"
import { BookGenre } from "./book-genre";
import { UserDisplayDto } from "./user-display-dto";

export type BorrowFormDto = 
{
    authors: AuthorDto[];
    genres: BookGenre[];
    members: UserDisplayDto[];
}