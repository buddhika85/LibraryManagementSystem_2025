import { Component, inject, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, ReactiveFormsModule } from '@angular/forms';
import { MatButton } from '@angular/material/button';
//import { MatCard } from '@angular/material/card';
import { MatFormField, MatLabel } from '@angular/material/form-field';
import { MatInput } from '@angular/material/input';
import { AccountService } from '../../../core/services/account.service';
import { Router } from '@angular/router';
import { first } from 'rxjs';
import { UserRoles } from '../../../shared/models/user-roles-enum';

@Component({
  selector: 'app-register',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
        //MatCard,
        MatButton,
        MatInput,
        MatLabel,
        MatFormField
  ],
  templateUrl: './register.component.html',
  styleUrl: './register.component.scss'
})
export class RegisterComponent implements OnInit {

  errorMessage: string = '';

  private formBuilder = inject(FormBuilder);
  private accountService = inject(AccountService);
  private router = inject(Router);

   registerForm!: FormGroup;
  
  ngOnInit(): void {
    this.registerForm = this.formBuilder.group({  
      firstName: [''],
      lastName: [''],
      email: [''],
      phoneNumber: [''],
      password: [''],
      line1: [''],
      line2: [''],
      city: [''],
      state: [''],
      postcode: [''],
      country: ['Asustria'],
      role: [UserRoles.member]
    });
  }

  onSubmit(): void {
    if (this.registerForm.valid) {
      const registerData = this.registerForm.value;
      this.accountService.registerMember(registerData).pipe(first()).subscribe({
        next: (data) => {
          //console.log(data.message);
          this.router.navigate(['/account/login']);
        },
        error: (error) => {
          this.errorMessage = 'Registration error:';
          console.error('Registration error:', error);
        }
      });
    } 
    else 
    {
      this.errorMessage = 'Please fill in all required fields.';
    }
  }
}
