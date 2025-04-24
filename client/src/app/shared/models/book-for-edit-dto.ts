import { AuthorDto } from "./author-dto";
import { BookSaveDto } from "./book-save-dto";
import { ResultDto } from "./result-dto";


export interface BookForEditDto extends ResultDto {
  book?: BookSaveDto; 
  allAuthors: ReadonlyArray<AuthorDto>;
}
