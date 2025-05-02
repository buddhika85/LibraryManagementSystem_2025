import { Directive, effect, inject, OnInit, TemplateRef, ViewContainerRef } from '@angular/core';
import { AccountService } from '../../core/services/account.service';

@Directive({
  selector: '[appIsLoggedIn]',
  standalone: true
})
export class IsLoggedInDirective {

  private accountService = inject(AccountService);
  private viewContainerRef = inject(ViewContainerRef);
  private templateRef = inject(TemplateRef<any>);

  constructor() {
    effect(() => {
      if (this.accountService.isAdmin() || this.accountService.isStaff() || this.accountService.isMember()) {
        this.viewContainerRef.createEmbeddedView(this.templateRef);
      }
      else {
        this.viewContainerRef.clear();
      }
    });

  }

}
