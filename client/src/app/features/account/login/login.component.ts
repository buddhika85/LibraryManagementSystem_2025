import { Component, inject, OnInit } from '@angular/core';
import { Form, FormBuilder, FormGroup, ReactiveFormsModule } from '@angular/forms';
import { MatButton } from '@angular/material/button';
import { MatCard } from '@angular/material/card';
import { MatFormField, MatInput, MatLabel } from '@angular/material/input';
import { AccountService } from '../../../core/services/account.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [
    ReactiveFormsModule,
    MatCard,
    MatButton,
    MatInput,
    MatLabel,
    MatFormField
  ],
  templateUrl: './login.component.html',
  styleUrl: './login.component.scss'
})
export class LoginComponent implements OnInit {
  private formBuilder = inject(FormBuilder);
  private accountService = inject(AccountService);
  private router = inject(Router);

  loginForm!: FormGroup;
  errorMessage: string = '';

  constructor() {
  }

  ngOnInit(): void {
    this.createForm();
  }


  createForm() {
    this.loginForm = this.formBuilder.group(
      {
        email: [''],
        password: ['']
      }
    );
  }

  onSubmit(): void {
    if (this.loginForm.valid) 
    {
      const loginData = this.loginForm.value;

      this.accountService.login(loginData).subscribe({
        next: (data) => 
        {
          console.log(data.message);
          this .accountService.getUserInfo().subscribe();     // Fetch user info after login     
          this.router.navigate(['/home']);
        },
        error: (error) => 
        {
          console.error('Login failed', error);
          this.errorMessage = 'Invalid email or password.';
        }
      });
    } 
    else 
    {
      this.errorMessage = 'Please fill in all required fields.';
    }
  }
}
