import { BorrowalsDisplayDto } from "./borrowals-display-dto";
import { ResultDto } from "./result-dto";

export interface BorrowalReturnInfoDto extends ResultDto {
    borrowalId: number;
    borrowalsDisplayDto?: BorrowalsDisplayDto; 
  
    borrowalDateStr: string;
    dueDateStr: string;
  
    isOverdue: boolean;
    lateDays: number;
    perDayLateFeeDollars: number;
  
    amountDue: number; 
  }