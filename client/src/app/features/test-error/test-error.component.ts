import { Component, inject } from '@angular/core';
import { MatButton } from '@angular/material/button';
import { TestErrorService } from '../../core/services/test-error.service';

@Component({
  selector: 'app-test-error',
  standalone: true,
  imports: [
    MatButton
  ],
  templateUrl: './test-error.component.html',
  styleUrl: './test-error.component.scss'
})
export class TestErrorComponent 
{
  errorService = inject(TestErrorService);
  validationErrors?: string[];
  
  get404Error() { 
    this.errorService.getNotFound().subscribe({
      next: (response) => console.log(response),
      error: (error) => console.log(error)
    });
  }

  get500Error() { 
    this.errorService.getServerError().subscribe({
      next: (response) => console.log(response),
      error: (error) => console.log(error)
    });
  }

  get400Error() { 
    this.errorService.getBadRequest().subscribe({
      next: (response) => console.log(response),
      error: (error) => console.log(error)
    });
  }

  get401Error() { 
    this.errorService.getUnauthorized().subscribe({
      next: (response) => console.log(response),
      error: (error) => console.log(error)
    });
  }

  getValidationError() { 
    this.errorService.getValidationError().subscribe({
      next: (response) => console.log(response),
      error: (error) => 
      {
        this.validationErrors = error;
      }
    });
  }
  
  getSecret() { 
    this.errorService.getSecret().subscribe({
      next: (response) => console.log(response),
      error: (error) => console.log(error)
    });
  }
}
