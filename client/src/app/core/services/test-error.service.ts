import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { environment } from '../../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class TestErrorService 
{

  private http = inject(HttpClient);
  baseUrl = environment.apiBaseUrl + 'buggy';

  getUnauthorized() {
    return this.http.get(this.baseUrl + '/unauthorized');
  }

  getBadRequest() {
    return this.http.get(this.baseUrl + '/badrequest');  
  }

  getNotFound() {
    return this.http.get(this.baseUrl + '/notfound');
  }

  getServerError() {
    return this.http.get(this.baseUrl + '/internalerror');
  }

  getValidationError() {
    return this.http.post(this.baseUrl + '/validationerror', {});
  }

  getSecret() {
    return this.http.get(this.baseUrl + '/secret');
  }
}
