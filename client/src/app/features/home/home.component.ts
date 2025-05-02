import { Component, inject } from '@angular/core';
import { AccountService } from '../../core/services/account.service';
import { IsAdminDirective } from '../../shared/directives/is-admin.directive';
import { IsMemberDirective } from '../../shared/directives/is-member.directive';
import { IsStaffDirective } from '../../shared/directives/is-staff.directive';
import { IsLoggedInDirective } from '../../shared/directives/is-logged-in.directive';

@Component({
  selector: 'app-home',
  standalone: true,
  imports: [
     IsAdminDirective,
        IsAdminDirective,
        IsMemberDirective,
        IsStaffDirective,
        IsLoggedInDirective
  ],
  templateUrl: './home.component.html',
  styleUrl: './home.component.scss'
})
export class HomeComponent {

  accountService = inject(AccountService);
}
