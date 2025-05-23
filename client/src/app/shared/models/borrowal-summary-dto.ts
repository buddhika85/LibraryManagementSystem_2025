import { BookGenre } from "./book-genre";
import { BorrowalStatus } from "./borrowal-status-enum";

export interface BorrowalSummaryDto 
{
    borrowalsId: number;
    borrowalDate: Date; 
    borrowalDateStr: string;

    dueDate: Date;
    dueDateStr: string;

    borrowalStatus: BorrowalStatus;
    borrowalStatusStr: string;

    // Payment Information
    wasPaid: boolean;
    wasOverdue: boolean;
    amountPaid: number;
    lateDaysOnPayment: number;
    paymentAcceptedBy: string;

    // Book Details
    bookId: number;
    bookTitle: string;
    bookPic: string;
    bookGenre: BookGenre;
    bookGenreStr: string;

    // Borrower Details
    borrowerEmail: string;
    borrowerName: string;
}


export interface BorrowalSummaryListDto 
{
    borrowalSummaries: BorrowalSummaryDto[];
    count: number;
}