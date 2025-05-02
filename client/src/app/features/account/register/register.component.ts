import { Component, inject, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatButton } from '@angular/material/button';
import { MatFormField, MatLabel } from '@angular/material/form-field';
import { MatInput } from '@angular/material/input';
import { AccountService } from '../../../core/services/account.service';
import { Router } from '@angular/router';
import { UserRoles } from '../../../shared/models/user-roles-enum';
import { MemberRegisterDto } from '../../../shared/models/register-dto';
import { AddressDto } from '../../../shared/models/address-dto';
import { SnackBarService } from '../../../core/services/snack-bar.service';

@Component({
  selector: 'app-register',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,        
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

  private snackBarService = inject(SnackBarService);
  validationErrors?: string[];

  registerForm!: FormGroup;
  
  ngOnInit(): void {
    this.createForm();
  }

  private createForm(): void 
  {
    this.registerForm = this.formBuilder.group({  
      firstName: ['Jack', Validators.required],
      lastName: ['Gill', Validators.required],
      email: ['j@g.c', [Validators.required, Validators.email]],
      phoneNumber: ['123456789', Validators.required],
      password: ['test', Validators.required],
      line1: ['123', Validators.required],
      line2: ['Test St', Validators.required],
      city: ['Hills', Validators.required],
      state: ['State', Validators.required],
      postcode: ['2134', Validators.required],
      country: ['Australia'],
      role: [UserRoles.member]
    });
  }

  clearForm(): void {
    this.registerForm.reset();
    this.errorMessage = '';
    this.validationErrors = undefined;
  }

  onSubmit(): void {

    if (this.registerForm.valid) {

      const registerData: MemberRegisterDto = this.getMemberRegisterDto(this.registerForm);

      this.accountService.registerMember(registerData).subscribe({
        next: (data) => {
          
          this.snackBarService.success("Registration successful! Please log in.");
          this.router.navigateByUrl('/account/login');
        },
        error: (errors) => {
          debugger
          this.errorMessage = 'Registration error:';
          this.validationErrors = errors;
        }
      });
    } 
    else 
    {
      this.errorMessage = 'Please fill in all required fields.';
    }
  }

  getMemberRegisterDto(registerForm: FormGroup): MemberRegisterDto 
  {
    const addressVals: AddressDto = { 
      line1: this.registerForm.get('line1')?.value, 
      line2: this.registerForm.get('line2')?.value, 
      city: this.registerForm.get('city')?.value, 
      state: this.registerForm.get('state')?.value, 
      postcode: this.registerForm.get('postcode')?.value, 
      country: this.registerForm.get('country')?.value 
    };
    const registerData: MemberRegisterDto = { 
      firstName: this.registerForm.get('firstName')?.value, 
      lastName: this.registerForm.get('lastName')?.value,
      email: this.registerForm.get('email')?.value,
      phoneNumber: this.registerForm.get('phoneNumber')?.value, 
      password: this.registerForm.get('password')?.value,
      address: addressVals, 
      role: UserRoles.member 
    };
    return registerData;
  }
  
}

