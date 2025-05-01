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


export const routes: Routes = [
     
    { path: '', component: LandingComponent},
    { path: 'home', component: HomeComponent},

   

    { path: 'test-error', component: TestErrorComponent },
    { path: 'not-found', component: NotFoundComponent },
    { path: 'server-error', component: ServerErrorComponent },

    { path: 'account/login', component: LoginComponent },
    { path: 'account/register', component: RegisterComponent},
    { path: 'account/profile', component: ProfileComponent },
  
    { path: 'manageBooks', component: BookListComponent, canActivate: [authGuard]  },
    //{ path: 'manageBooks/:id', component:BookDetailComponent },

  
    { path: '**', redirectTo: 'not-found', pathMatch: 'full' } // Redirect to not found for any unknown routes   
];
