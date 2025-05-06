import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { environment } from '../../../environments/environment';
import { BorrowalsDisplayListDto } from '../../shared/models/borrowals-display-list-dto';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class BorrwalsService {

  private http = inject(HttpClient);
  baseUrl = environment.apiBaseUrl + 'borrowals/';
    
  GetAllBorrowals(): Observable<BorrowalsDisplayListDto>
  {
    return this.http.get<BorrowalsDisplayListDto>(this.baseUrl + 'all-borrowals');
  }
}
