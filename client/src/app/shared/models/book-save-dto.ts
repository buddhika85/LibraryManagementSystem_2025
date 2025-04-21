import { BaseDto } from "./base-dto";
import { BookGenre } from "./book-genre";

export interface BookSaveDto extends BaseDto {
    title: string;
    genre: BookGenre;
    authorIds: number[];
    publishedDate: Date;
    pictureUrl: string;
  }