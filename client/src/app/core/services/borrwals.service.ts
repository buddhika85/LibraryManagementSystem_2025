import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { environment } from '../../../environments/environment';
import { BorrowalsDisplayListDto } from '../../shared/models/borrowals-display-list-dto';
import { Observable } from 'rxjs';
import { BorrowFormDto } from '../../shared/models/borrow-form-dto';
import { BookWithAuthorListDto } from '../../shared/models/book-with-author-list-dto';
import { BookFilterDto } from '../../shared/models/book-filter-dto';
import { BookBorrowRequestDto, BorrowResultDto } from '../../shared/models/book-borrow-request-dto';
import { BorrowalReturnInfoDto } from '../../shared/models/borrowal-return-info-dto';
import { ReturnsAcceptDto } from '../../shared/models/returns-accept-dto';
import { ReturnResultDto } from '../../shared/models/result-dto';
import { BorrowalSummaryListDto } from '../../shared/models/borrowal-summary-dto';

@Injectable({
  providedIn: 'root'
})
export class BorrwalsService {

  private http = inject(HttpClient);
  baseUrl = environment.apiBaseUrl + 'borrowals/';
    
  getAllBorrowals(): Observable<BorrowalsDisplayListDto>
  {
    return this.http.get<BorrowalsDisplayListDto>(this.baseUrl + 'all-borrowals');
  }

  getBorrowFormData(): Observable<BorrowFormDto>
  {
    return this.http.get<BorrowFormDto>(this.baseUrl + 'borrow-form-data');
  }

  filterBooks(filter: BookFilterDto): Observable<BookWithAuthorListDto>
  {
    return this.http.post<BookWithAuthorListDto>(this.baseUrl + 'filter-books', filter);
  }

  borrowBook(requestDto: BookBorrowRequestDto): Observable<BorrowResultDto>
  {
    return this.http.post<BorrowResultDto>(this.baseUrl + 'borrow-book', requestDto);
  }

  getBorrowalReturnInfoDto(borrowalId: number): Observable<BorrowalReturnInfoDto>
  {
    return this.http.get<BorrowalReturnInfoDto>(this.baseUrl + `borrowal-return-info/${borrowalId}`);
  }

  returnBook(dto: ReturnsAcceptDto): Observable<ReturnResultDto>
  {
    return this.http.post<ReturnResultDto>(this.baseUrl + 'return-book', dto);
  }
  
  getBorrowalSummaryForMember(): Observable<BorrowalSummaryListDto>
  {
    return this.http.get<BorrowalSummaryListDto>(this.baseUrl + 'borrowal-summary-logged-member');
  }
}
