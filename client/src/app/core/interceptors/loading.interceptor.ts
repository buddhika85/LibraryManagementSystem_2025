import { HttpInterceptorFn } from '@angular/common/http';
import { inject } from '@angular/core';
import { delay, finalize, identity } from 'rxjs';

import { environment } from '../../../environments/environment';
import { BusyService } from '../services/busy.service';

export const loadingInterceptor: HttpInterceptorFn = (req, next) => {
  const busyService = inject(BusyService);

  busyService.busy();

  return next(req).pipe(
    // (environment.production ? delay(0) : delay(500)),
    (environment.production ? identity : delay(500)),         // Simulate network delay in development mode
    finalize(() => busyService.idle())
  )
};