import { BookGenre } from "./book-genre";

export type BookFilterDto = 
{
    bookGenres: BookGenre[];
    authorIds: number[];
}