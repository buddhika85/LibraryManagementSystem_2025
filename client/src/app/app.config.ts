import { APP_INITIALIZER, ApplicationConfig, provideZoneChangeDetection } from '@angular/core';
import { provideRouter } from '@angular/router';

import { routes } from './app.routes';
import { provideAnimationsAsync } from '@angular/platform-browser/animations/async';
import { provideHttpClient, withInterceptors } from '@angular/common/http';



import { provideNativeDateAdapter } from '@angular/material/core';
import { InitService } from './core/services/init.service';
import { lastValueFrom } from 'rxjs';
import { authInterceptor } from './core/interceptors/auth.interceptor';
import { errorInterceptor } from './core/interceptors/error.interceptor';
import { loadingInterceptor } from './core/interceptors/loading.interceptor';

function initializeApp(initService : InitService)
{
  return () => lastValueFrom(initService.init()).finally(() => {
    console.log('App initialized'); 
  });
}

export const appConfig: ApplicationConfig = {
  providers: [
    provideZoneChangeDetection({ eventCoalescing: true }), 
    provideRouter(routes), 
    provideAnimationsAsync(),
    provideHttpClient(withInterceptors([
      errorInterceptor,
      loadingInterceptor,
      authInterceptor]
    )),
    {
      provide: APP_INITIALIZER,
      useFactory: initializeApp,
      multi: true,
      deps: [InitService]
    },
    
    provideNativeDateAdapter(), 
  ]
};
