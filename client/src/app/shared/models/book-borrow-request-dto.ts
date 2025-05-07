import { ResultDto } from "./result-dto";

export interface BookBorrowRequestDto {
    bookId: number;
    email: string;
    startDate: Date;
    endDate: Date;
};
  
export interface  BorrowResultDto extends ResultDto {
    bookId: number;
    bookTitle: string;
    bookAuthors: string;
    memberEmail: string;
    memberFullName: string;
    startDate: Date;
    endDate: Date;
};