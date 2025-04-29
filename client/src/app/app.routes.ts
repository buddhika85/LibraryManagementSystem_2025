import { Routes } from '@angular/router';
import { HomeComponent } from './features/home/home.component';
import { BookListComponent } from './features/book-list/book-list.component';
import { ProfileComponent } from './features/account/profile/profile.component';
import { LoginComponent } from './features/account/login/login.component';
import { RegisterComponent } from './features/account/register/register.component';


export const routes: Routes = [
     
    { path: '', component: HomeComponent},
    { path: 'account/login', component: LoginComponent},
    { path: 'account/register', component: RegisterComponent},
    { path: 'account/profile', component: ProfileComponent },
  
    { path: 'manageBooks', component: BookListComponent },
    //{ path: 'manageBooks/:id', component:BookDetailComponent },

  
    { path: '**', redirectTo: '', pathMatch: 'full' } // Redirect to home for any unknown routes   
];
