import { computed, inject, Injectable, signal } from '@angular/core';
import { environment } from '../../../environments/environment';
import { HttpClient, HttpParams } from '@angular/common/http';
import { LoginResponseDto, UserInfoDto } from '../../shared/models/user-info-dto';
import { LoginRequestDto } from '../../shared/models/login-request-dto';
import { MemberRegisterDto, RegisterDto } from '../../shared/models/register-dto';
import { map, Observable } from 'rxjs';
import { UserRoles } from '../../shared/models/user-roles-enum';
import { UserUpdateDto } from '../../shared/models/user-update-dto';
import { ChangePasswordDto } from '../../shared/models/change-password-dto';

@Injectable({
  providedIn: 'root'
})
export class AccountService 
{

  private http = inject(HttpClient);
  baseUrl = environment.apiBaseUrl + 'account';
  
  currentUser = signal<UserInfoDto | null>(null);
  
  isAdmin = computed(() => {
    if (this.currentUser() != null && this.currentUser()?.role == UserRoles.admin) {
      return true;
    }
    return false;
  });

  isStaff = computed(() => {
    if (this.currentUser() != null && this.currentUser()?.role == UserRoles.staff) {
      return true;
    }
    return false;
  });

  isMember = computed(() => {
    if (this.currentUser() != null && this.currentUser()?.role == UserRoles.member) {
      return true;
    }
    return false;
  });



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
  // registerAdmin(registerDto : RegisterDto)
  // {
  //   return this.http.post(this.baseUrl + '/register', registerDto);
  // }


  getUserInfo() 
  {
    return this.http.get<UserInfoDto>(this.baseUrl + '/userinfo').pipe(map(user => {
      this.currentUser.set(user);
      return user;
    }));
  }

  getAuthState() : Observable<{ isAuthenticated: boolean }> 
  {
    return this.http.get<{isAuthenticated: boolean}>(this.baseUrl + '/auth-status');
  }

  updateProfile(dto: UserUpdateDto)
  {
    return this.http.put(this.baseUrl + `/updateProfile/${dto.email}`, dto);
  }

  changePassword(dto: ChangePasswordDto) 
  {
    return this.http.post(this.baseUrl + `/changePassword`, dto);
  }
}
