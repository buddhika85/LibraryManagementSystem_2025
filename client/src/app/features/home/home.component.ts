import { Component, inject } from '@angular/core';
import { AccountService } from '../../core/services/account.service';
import { IsAdminDirective } from '../../shared/directives/is-admin.directive';
import { IsMemberDirective } from '../../shared/directives/is-member.directive';
import { IsStaffDirective } from '../../shared/directives/is-staff.directive';

@Component({
  selector: 'app-home',
  standalone: true,
  imports: [
     IsAdminDirective,
        IsAdminDirective,
        IsMemberDirective,
        IsStaffDirective
  ],
  templateUrl: './home.component.html',
  styleUrl: './home.component.scss'
})
export class HomeComponent {

  accountService = inject(AccountService);
}
