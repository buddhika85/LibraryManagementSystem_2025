import { BookGenre } from "./book-genre"
import { BorrowalStatus } from "./borrowal-status-enum"

export interface BorrowalsSearchDto {
    bookName?: string,
    authorIds?: number[],
    genres?: BookGenre[],
    memberName?: string,
    memberEmail?: string,
    borrowedOn?: Date,
    dueOn?: Date,
    statuses?: BorrowalStatus[],
    delayed?: number,
    applyFilters: boolean      
}