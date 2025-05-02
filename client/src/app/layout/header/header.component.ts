import { CommonModule } from '@angular/common';
import { Component, inject } from '@angular/core';
import { MatIcon } from '@angular/material/icon';
import { Router, RouterLink, RouterLinkActive } from '@angular/router';
import { AccountService } from '../../core/services/account.service';
import { BusyService } from '../../core/services/busy.service';

import { MatProgressBar } from '@angular/material/progress-bar';

@Component({
  selector: 'app-header',
  standalone: true,
  imports: [ 
    MatIcon, CommonModule, 
    RouterLink,
    RouterLinkActive,
    MatProgressBar
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