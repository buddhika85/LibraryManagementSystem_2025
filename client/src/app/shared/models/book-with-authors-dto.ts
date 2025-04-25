import { AuthorDto } from './author-dto';
import { BookGenre } from './book-genre';

export interface BookWithAuthorsDto {
  bookId: number;
  bookTitle: string;
  bookGenre: BookGenre;      
  bookGenreStr: string;
  bookPublishedDate: string;  
  bookPublishedDateStr: string;
  bookPictureUrl: string;
  authorList: AuthorDto[];
  authorListStr: string;
}
