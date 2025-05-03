import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { environment } from '../../../environments/environment';
import { UsersListDto } from '../../shared/models/user-display-dto';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class MemberService {

  private http = inject(HttpClient);
  baseUrl = environment.apiBaseUrl + 'members';
  
  getAllMembers() : Observable<UsersListDto>
  {
    return this.http.get<UsersListDto>(this.baseUrl + '/allMembers');
  }
}
