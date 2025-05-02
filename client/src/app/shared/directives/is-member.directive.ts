import { Directive, effect, inject, OnInit, TemplateRef, ViewContainerRef } from '@angular/core';
import { AccountService } from '../../core/services/account.service';

@Directive({
  selector: '[appIsMember]',
  standalone: true
})
export class IsMemberDirective {

  private accountService = inject(AccountService);
  private viewContainerRef = inject(ViewContainerRef);
  private templateRef = inject(TemplateRef<any>);

  constructor() {
      effect(() => 
      {
        if (this.accountService.isMember()) 
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
