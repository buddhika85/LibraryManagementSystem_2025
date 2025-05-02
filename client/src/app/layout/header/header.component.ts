import { CommonModule } from '@angular/common';
import { Component, inject } from '@angular/core';
import { MatIcon } from '@angular/material/icon';
import { Router, RouterLink, RouterLinkActive } from '@angular/router';
import { AccountService } from '../../core/services/account.service';
import { BusyService } from '../../core/services/busy.service';

import { MatProgressBar } from '@angular/material/progress-bar';
import { IsAdminDirective } from '../../shared/directives/is-admin.directive';
import { IsStaffOrAdminDirective } from '../../shared/directives/is-staff-or-admin.directive';
import { IsMemberDirective } from '../../shared/directives/is-member.directive';
import { IsLoggedInDirective } from '../../shared/directives/is-logged-in.directive';

import { MatMenu, MatMenuItem, MatMenuTrigger } from '@angular/material/menu';
import { MatDivider } from '@angular/material/divider';
import { MatButton } from '@angular/material/button';

@Component({
  selector: 'app-header',
  standalone: true,
  imports: [ 
    MatIcon, 
    MatButton,
    MatMenu,
    MatMenuItem,
    MatMenuTrigger,
    MatDivider,
    
    CommonModule, 
    RouterLink,
    RouterLinkActive,
    MatProgressBar,
    IsAdminDirective,
    IsStaffOrAdminDirective,
    IsMemberDirective,
    IsLoggedInDirective
  ],
  templateUrl: './header.component.html',
  styleUrl: './header.component.scss'
})
export class HeaderComponent {
  menuOpen = false;

  accountService = inject(AccountService);
  private router = inject(Router);

  busyService = inject(BusyService);

  toggleMenu() {
    this.menuOpen = !this.menuOpen;
  }

  logout() {
    this.accountService.logout().subscribe({
      next: () => 
      {
        this.accountService.currentUser.set(null); // Clear user data after logout
        this.router.navigateByUrl('');            //  go to { path: '', component: LandingComponent},
      },
      error: (err) => {
        console.error('Logout failed', err);
      }
    });
  }
}