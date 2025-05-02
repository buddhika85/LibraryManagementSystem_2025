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
import { UserInfoDto } from '../../../shared/models/user-info-dto';

@Component({
  selector: 'app-profile',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,        
        MatButton,
        MatInput,
        MatLabel,
        MatFormField
  ],
  templateUrl: './profile.component.html',
  styleUrl: './profile.component.scss'
})
export class ProfileComponent implements OnInit  {

    errorMessage: string = '';
  
    private unchangedPassword: string = '*********';
    private formBuilder = inject(FormBuilder);
    private accountService = inject(AccountService);
    private router = inject(Router);
  
    private snackBarService = inject(SnackBarService);
    validationErrors?: string[];
  
    profileForm!: FormGroup;
    profileUser!: UserInfoDto | null;

    
  ngOnInit(): void {
    this.profileUser = this.accountService.currentUser();
    this.createForm();
  }

  private createForm(): void 
  {
    this.profileForm = this.formBuilder.group({  
      firstName: [this.profileUser?.firstName, Validators.required],
      lastName: [this.profileUser?.lastName, Validators.required],
      email: [this.profileUser?.email, [Validators.required, Validators.email]],
      phoneNumber: [this.profileUser?.phoneNumber, Validators.required],
      password: [this.unchangedPassword, Validators.required],
      line1: [this.profileUser?.address.line1, Validators.required],
      line2: [this.profileUser?.address.line2, Validators.required],
      city: [this.profileUser?.address.city, Validators.required],
      state: [this.profileUser?.address.state, Validators.required],
      postcode: [this.profileUser?.address.postcode, Validators.required],
      country: [this.profileUser?.address.country, Validators.required],
      role: [UserRoles.member]
    });
  }

  clearForm(): void {
    this.profileForm.reset();
    this.errorMessage = '';
    this.validationErrors = undefined;
  }

  onSubmit(): void {

    if (this.profileForm.valid) {

      const registerData: MemberRegisterDto = this.getMemberRegisterDto(this.profileForm);

      this.accountService.registerMember(registerData).subscribe({
        next: (data) => {
          //console.log(data.message);
          debugger
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

  getMemberRegisterDto(profileForm: FormGroup): MemberRegisterDto 
  {
    const addressVals: AddressDto = { 
      line1: this.profileForm.get('line1')?.value, 
      line2: this.profileForm.get('line2')?.value, 
      city: this.profileForm.get('city')?.value, 
      state: this.profileForm.get('state')?.value, 
      postcode: this.profileForm.get('postcode')?.value, 
      country: this.profileForm.get('country')?.value 
    };
    const registerData: MemberRegisterDto = { 
      firstName: this.profileForm.get('firstName')?.value, 
      lastName: this.profileForm.get('lastName')?.value,
      email: this.profileForm.get('email')?.value,
      phoneNumber: this.profileForm.get('phoneNumber')?.value, 
      password: this.profileForm.get('password')?.value,
      address: addressVals, 
      role: UserRoles.member 
    };
    return registerData;
  }
}
