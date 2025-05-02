import { Directive, effect, inject, OnInit, TemplateRef, ViewContainerRef } from '@angular/core';
import { AccountService } from '../../core/services/account.service';

@Directive({
  selector: '[appIsStaffOrAdmin]',
  standalone: true
})
export class IsStaffOrAdminDirective {

  private accountService = inject(AccountService);
  private viewContainerRef = inject(ViewContainerRef);
  private templateRef = inject(TemplateRef<any>);

  constructor() {
    effect(() => 
    {
      if (this.accountService.isAdmin() || this.accountService.isStaff()) 
        {
          this.viewContainerRef.createEmbeddedView(this.templateRef);
        }
        else 
        {
          this.viewContainerRef.clear();
        }
    });
  
  }

}
