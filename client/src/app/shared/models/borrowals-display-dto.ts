import { BaseDto } from "./base-dto";
import { BookWithAuthorsDto } from "./book-with-authors-dto";
import { BorrowalStatus } from "./borrowal-status-enum";
import { UserInfoDto } from "./user-info-dto";

export interface BorrowalsDisplayDto extends BaseDto {
    borrowalDate: string; 
    borrowalDateStr: string;
    
    dueDate: string;
    isDelayed: boolean;
    dueDateStr: string;
  
    borrowalStatus: BorrowalStatus;
    borrowalStatusStr: string;
  
    // Foreign Keys
    bookId: number;
    appUserId: string;
  
    // FK nav properties to display
    book: BookWithAuthorsDto;
    bookDisplayStr: string;
    bookPictureUrl: string;
  
    appUser: UserInfoDto;
    memberName: string;
    memberEmail: string;
  }
