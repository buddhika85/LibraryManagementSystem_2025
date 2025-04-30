import { HttpErrorResponse, HttpInterceptorFn } from '@angular/common/http';
import { inject } from '@angular/core';
import { Router } from '@angular/router';
import { catchError, throwError } from 'rxjs';
import { SnackBarService } from '../services/snack-bar.service';

export const errorInterceptor: HttpInterceptorFn = (req, next) => {

  const router = inject(Router);
  const snackBar = inject(SnackBarService);

  return next(req).pipe(
    catchError((err : HttpErrorResponse) => {
      if (err.status === 400) {
        //alert(err.error.title || err.error);
        snackBar.error(err.error.title || err.error);
      }
      if (err.status === 401) {
        snackBar.error(err.error.title || err.error);
      }
      if (err.status === 404) {
        router.navigateByUrl('/not-found');
      }
      if (err.status === 500) {
        router.navigateByUrl('/server-error');

      }
      return throwError(() => err );
    })
  );
};
