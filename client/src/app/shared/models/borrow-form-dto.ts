import { AuthorDto } from "./author-dto"
import { BookGenre } from "./book-genre";

export type BorrowFormDto = 
{
    authors: AuthorDto[];
    genres: BookGenre[];
}