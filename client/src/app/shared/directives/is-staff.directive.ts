import { Directive, effect, inject, TemplateRef, ViewContainerRef } from '@angular/core';
import { AccountService } from '../../core/services/account.service';

@Directive({
  selector: '[appIsStaff]',
  standalone: true
})
export class IsStaffDirective {

  private accountService = inject(AccountService);
  private viewContainerRef = inject(ViewContainerRef);
  private templateRef = inject(TemplateRef<any>);

  constructor() {
      effect(() => 
      {
        if (this.accountService.isStaff()) 
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
