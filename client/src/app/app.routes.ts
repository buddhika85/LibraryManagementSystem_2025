import { Routes } from '@angular/router';
import { HomeComponent } from './features/home/home.component';
import { BookListComponent } from './features/book-list/book-list.component';
import { ProfileComponent } from './features/account/profile/profile.component';
import { LoginComponent } from './features/account/login/login.component';
import { RegisterComponent } from './features/account/register/register.component';
import { TestErrorComponent } from './features/test-error/test-error.component';
import { ServerErrorComponent } from './shared/components/server-error/server-error.component';
import { NotFoundComponent } from './shared/components/not-found/not-found.component';
import { authGuard } from './core/guards/auth.guard';
import { LandingComponent } from './features/landing/landing.component';
import { ChangePasswordComponent } from './features/account/change-password/change-password.component';
import { MemberListComponent } from './features/member-list/member-list.component';
import { StaffListComponent } from './features/staff-list/staff-list.component';
import { BorrowalsListComponent } from './features/borrowals-list/borrowals-list.component';
import { MemberBorrowalsHistoryComponent } from './features/borrowals-list/member-borrowals-history/member-borrowals-history.component';


export const routes: Routes = [
     
    { path: '', component: LandingComponent},
    { path: 'home', component: HomeComponent},

   

    { path: 'test-error', component: TestErrorComponent },
    { path: 'not-found', component: NotFoundComponent },
    { path: 'server-error', component: ServerErrorComponent },

    { path: 'account/login', component: LoginComponent },
    { path: 'account/register', component: RegisterComponent},

    { path: 'account/profile', component: ProfileComponent, canActivate: [authGuard] },
    { path: 'account/change-password', component: ChangePasswordComponent, canActivate: [authGuard]},
  
    { path: 'manageBooks', component: BookListComponent, canActivate: [authGuard]  },
   
    { path: 'manageMembers', component: MemberListComponent, canActivate: [authGuard]  },
    { path: 'manageStaff', component: StaffListComponent, canActivate: [authGuard]  },

    { path: 'manageBorrowals', component: BorrowalsListComponent, canActivate: [authGuard]  },

    { path: 'memberBorrowals', component: MemberBorrowalsHistoryComponent, canActivate: [authGuard]  },
  
    { path: '**', redirectTo: 'not-found', pathMatch: 'full' } // Redirect to not found for any unknown routes   
];
