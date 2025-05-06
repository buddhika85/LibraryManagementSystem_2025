import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { environment } from '../../../environments/environment';
import { BorrowalsDisplayListDto } from '../../shared/models/borrowals-display-list-dto';
import { Observable } from 'rxjs';
import { BorrowFormDto } from '../../shared/models/borrow-form-dto';

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
}
