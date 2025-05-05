import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { environment } from '../../../environments/environment';
import { UsersListDto } from '../../shared/models/user-display-dto';
import { Observable } from 'rxjs';
import { UserInfoDto } from '../../shared/models/user-info-dto';
import { InsertUpdateUserDto } from '../../shared/models/insert-update-user-dto';
import { ResultDto } from '../../shared/models/result-dto';
import { UserUpdateDto } from '../../shared/models/user-update-dto';

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

  activateDeactivateMembers(username: string)
  {       
    return this.http.put(this.baseUrl + `/activateDeactivateMembers/${username}`, {});
  }

  getMemberForEdit(email: string | null) : Observable<UserInfoDto>
  {
    return this.http.get<UserInfoDto>(this.baseUrl + `/getMemberForEdit/${email}`);
  }

  addMember(userDto : InsertUpdateUserDto) : Observable<ResultDto>
  {
    return this.http.post<ResultDto>(this.baseUrl + `/addMember`, userDto);
  }

  updateMember(email: string, dto: UserUpdateDto) 
  {
    return this.http.put(this.baseUrl + `/updateMember/${email}`, dto);
  }
}
