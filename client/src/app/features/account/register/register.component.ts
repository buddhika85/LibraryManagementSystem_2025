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
import { MemberRegisterDto } from '../../../shared/models/register-dto';
import { AddressDto } from '../../../shared/models/address-dto';

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
      firstName: ['Jack'],
      lastName: ['Gill'],
      email: ['j@g.c'],
      phoneNumber: ['123456789'],
      password: ['test'],
      line1: ['123'],
      line2: ['Test St'],
      city: ['Hills'],
      state: ['State'],
      postcode: ['2134'],
      country: ['Australia'],
      role: [UserRoles.member]
    });
  }

  onSubmit(): void {

    if (this.registerForm.valid) {

      const registerData: MemberRegisterDto = this.getMemberRegisterDto(this.registerForm);

      this.accountService.registerMember(registerData).subscribe({
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

