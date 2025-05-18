import { inject, Injectable } from '@angular/core';
import { AccountService } from './account.service';
import { tap } from 'rxjs';
import { SignalRService } from './signal-r.service';

@Injectable({
  providedIn: 'root'
})
export class InitService {

  private accountService = inject(AccountService);
  private signalRService = inject(SignalRService);

  init() {
    let userInfo = this.accountService.getUserInfo();

    // initalize signal R connection
    userInfo.pipe(
      tap(userInfo => {
        if (userInfo)
          this.signalRService.createHubConnection();
      })
    );

    return userInfo;
  }
}
