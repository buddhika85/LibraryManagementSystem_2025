import { BookWithAuthorsDto } from './book-with-authors-dto';

export interface BookWithAuthorListDto {
  bookWithAuthorList: BookWithAuthorsDto[];
  count: number;
}
