import { CommonModule } from '@angular/common';
import { Component, inject } from '@angular/core';
import { MatIcon } from '@angular/material/icon';
import { Router, RouterLink, RouterLinkActive } from '@angular/router';
import { AccountService } from '../../core/services/account.service';

@Component({
  selector: 'app-header',
  standalone: true,
  imports: [ 
    MatIcon, CommonModule, 
    RouterLink,
    RouterLinkActive,
    
  ],
  templateUrl: './header.component.html',
  styleUrl: './header.component.scss'
})
export class HeaderComponent {
  menuOpen = false;

  accountService = inject(AccountService);
  private router = inject(Router);

  toggleMenu() {
    this.menuOpen = !this.menuOpen;
  }

  logout() {
    this.accountService.logout().subscribe({
      next: () => 
      {
        this.accountService.currentUser.set(null); // Clear user data after logout
        this.router.navigate(['/login']);        
      },
      error: (err) => {
        console.error('Logout failed', err);
      }
    });
  }
}