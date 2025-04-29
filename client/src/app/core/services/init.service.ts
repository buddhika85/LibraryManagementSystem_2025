import { inject, Injectable } from '@angular/core';
import { AccountService } from './account.service';

@Injectable({
  providedIn: 'root'
})
export class InitService {

  private accountService = inject(AccountService);

  init() {
    var userInfo = this.accountService.getUserInfo();
    return userInfo;
  }
}
