import { HttpErrorResponse, HttpInterceptorFn } from '@angular/common/http';
import { inject } from '@angular/core';
import { NavigationExtras, Router } from '@angular/router';
import { catchError, throwError } from 'rxjs';
import { SnackBarService } from '../services/snack-bar.service';

export const errorInterceptor: HttpInterceptorFn = (req, next) => {

  const router = inject(Router);
  const snackBar = inject(SnackBarService);

  return next(req).pipe(
    catchError((err : HttpErrorResponse) => {
      if (err.status === 400) 
      {
        if (err.error.errors) 
        {
          const modelStateErrors = [] as string[];
          for (const key in err.error.errors) {
            if (err.error.errors[key]) {
              modelStateErrors.push(err.error.errors[key]);
            }
          }

          throw modelStateErrors.flat();      // flatten all child errors into a single array
        }
        else
        {
          snackBar.error(err.error.title || err.error);
        }
      }
      
      if (err.status === 401) {
        
        snackBar.error(err.error.title || err.error);
      }
      if (err.status === 403) {
        snackBar.error('Forbidden');
      }
      if (err.status === 404) {
        router.navigateByUrl('/not-found');
      }
      if (err.status === 500) {
        const navigationExtras: NavigationExtras = {state: {error: err.error}}
        router.navigateByUrl('/server-error', navigationExtras);
      }
      return throwError(() => err)
    })
  );
};
