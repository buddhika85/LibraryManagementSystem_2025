import { inject, Injectable, signal } from '@angular/core';
import { environment } from '../../../environments/environment';
import { HttpClient, HttpParams } from '@angular/common/http';
import { LoginResponseDto, UserInfoDto } from '../../shared/models/user-info-dto';
import { LoginRequestDto } from '../../shared/models/login-request-dto';
import { MemberRegisterDto, RegisterDto } from '../../shared/models/register-dto';
import { map } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class AccountService 
{

  private http = inject(HttpClient);
  baseUrl = environment.apiBaseUrl + 'account';
  
  currentUser = signal<UserInfoDto | null>(null);

  login(loginRequest : LoginRequestDto) 
  {
    let params = new HttpParams();
    params = params.append('useCookies', true);
    return this.http.post<any>(this.baseUrl + '/login', loginRequest, { params });
  }

  logout()
  {
    return this.http.post(this.baseUrl + '/logout', {});
  }

  // guest and any role can execute below method
  registerMember(memberRegisterDto : MemberRegisterDto)
  {
    return this.http.post(this.baseUrl + '/registerMember', memberRegisterDto);
  }

  // admin only can execute below method
  registerAdmin(registerDto : RegisterDto)
  {
    return this.http.post(this.baseUrl + '/register', registerDto);
  }


  getUserInfo() 
  {
    return this.http.get<UserInfoDto>(this.baseUrl + '/userinfo').pipe(map(user => {
      this.currentUser.set(user);
      return user;
    }));
  }
}
