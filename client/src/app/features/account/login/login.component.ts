import { Component, inject, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatButton } from '@angular/material/button';
import { MatCard } from '@angular/material/card';
import { MatFormField, MatInput, MatLabel } from '@angular/material/input';
import { AccountService } from '../../../core/services/account.service';
import { ActivatedRoute, Router } from '@angular/router';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [
    CommonModule,
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
  private activatedRoute = inject(ActivatedRoute); 
  returnUrl = 'home';


  loginForm!: FormGroup;
  errorMessage: string = '';

  constructor() 
  {
    const url = this.activatedRoute.snapshot.queryParams['returnUrl'];
    if (url) 
    {
      this.returnUrl = url;
    }
  }

  ngOnInit(): void {
    this.createForm();
  }


  createForm() {
    this.loginForm = this.formBuilder.group(
      {
        email: ['', [Validators.required, Validators.email]],
        password: ['', Validators.required]
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
          this.router.navigateByUrl(this.returnUrl);
        },
        error: (error) => 
        {
          console.error('Login failed', error);
          this.errorMessage = 'Login failed - Invalid email or password.';
        }
      });
    } 
    else 
    {
      this.errorMessage = 'Please fill in all required fields.';
    }
  }
}
