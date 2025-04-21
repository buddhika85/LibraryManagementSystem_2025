import { AuthorDto } from './author-dto';
import { BookGenre } from './book-genre';

export interface BookWithAuthorsDto {
  bookId: number;
  bookTitle: string;
  bookGenre: BookGenre;       // or `number` if not deserialized as string
  bookGenreStr: string;
  bookPublishedDate: string;  // ISO string from .NET DateTime
  bookPublishedDateStr: string;
  bookPictureUrl: string;
  authorList: AuthorDto[];
}
