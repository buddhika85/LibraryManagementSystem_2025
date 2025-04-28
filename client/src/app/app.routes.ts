import { Routes } from '@angular/router';
import { HomeComponent } from './features/home/home.component';
import { BookListComponent } from './features/book-list/book-list.component';
import { ProfileComponent } from './features/profile/profile.component';

export const routes: Routes = [
     
    { path: '', component: HomeComponent },
    { path: 'manageBooks', component: BookListComponent },
    //{ path: 'manageBooks/:id', component:BookDetailComponent },
    { path: 'profile', component: ProfileComponent },
    { path: '**', redirectTo: '', pathMatch: 'full' } // Redirect to home for any unknown routes   
];
