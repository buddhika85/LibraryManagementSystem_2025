import { BorrowalsDisplayDto } from "./borrowals-display-dto";
import { ResultDto } from "./result-dto";

export interface BorrowalReturnInfoDto extends ResultDto {
    borrowalId: number;
    borrowalsDisplayDto?: BorrowalsDisplayDto; 
  
    borrowalDateStr: string;
    dueDateStr: string;
  
    isOverdue: boolean;
    isOverdueStr: string;

    lateDays: number;

    perDayLateFeeDollars: number;
    perDayLateFeeDollarsStr: string;

    amountDue: number; 
    amountDueStr: string;
  }