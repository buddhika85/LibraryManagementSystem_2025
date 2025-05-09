export interface BorrowalSummaryDto {
    borrowalsId: number;
    borrowalDate: string; // Converted DateTime to string for serialization
    dueDate: string;
    borrowalStatus: string;

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
    bookGenre: string;

    // Borrower Details
    borrowerEmail: string;
    borrowerName: string;
}


export interface BorrowalSummaryListDto {
    borrowalSummaries: ReadonlyArray<BorrowalSummaryDto>;
    count: number;
}