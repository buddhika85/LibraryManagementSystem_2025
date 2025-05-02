import { Component, inject, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatButton } from '@angular/material/button';
import { MatFormField, MatLabel } from '@angular/material/form-field';
import { MatInput } from '@angular/material/input';
import { AccountService } from '../../../core/services/account.service';
import { Router } from '@angular/router';

import { SnackBarService } from '../../../core/services/snack-bar.service';
import { UserInfoDto } from '../../../shared/models/user-info-dto';
import { ChangePasswordDto } from '../../../shared/models/change-password-dto';

@Component({
  selector: 'app-change-password',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,        
        MatButton,
        MatInput,
        MatLabel,
        MatFormField
  ],
  templateUrl: './change-password.component.html',
  styleUrl: './change-password.component.scss'
})
export class ChangePasswordComponent implements OnInit  {

  errorMessage: string = '';
 
  private formBuilder = inject(FormBuilder);
  private accountService = inject(AccountService);
  private router = inject(Router);

  private snackBarService = inject(SnackBarService);
  validationErrors?: string[];

  changePwForm!: FormGroup;
  changePasswordDto!: ChangePasswordDto | null;

  profileUser!: UserInfoDto | null;

  ngOnInit(): void {
    this.profileUser = this.accountService.currentUser();
    this.createForm();
  }

  private createForm(): void 
  {
    this.changePwForm = this.formBuilder.group({  
     
      username: [this.profileUser?.email, [Validators.required, Validators.email]],
     
      oldPassword: ['Pass#Word1', Validators.required],
      newPassword: ['Pass#Word2', Validators.required],
      confirmPassword: ['Pass#Word2', Validators.required],
     
    });
  }

  clearForm(): void {
    this.createForm();
    this.errorMessage = '';
    this.validationErrors = undefined;
  }

  onSubmit(): void {

    if (this.changePwForm.valid) 
    {    
      this.changePasswordDto = {... this.changePwForm.value};
     
      if (this.changePasswordDto && this.changePasswordDto.newPassword == this.changePasswordDto.confirmPassword) 
      {
        this.accountService.changePassword(this.changePasswordDto).subscribe({
          next: (data) => {
          
            this.snackBarService.success("Updating password successful! Please log in again.");
            this.router.navigateByUrl('/account/login');
          },
          error: (errors) => {
            debugger
            this.errorMessage = 'Update password error:';
            this.validationErrors = errors;
          }
        });
      }
      else
      {
        this.errorMessage = 'New and confirmed password must match. Please try again.';
        this.snackBarService.error(this.errorMessage);
      }    
    } 
    else 
    {
      this.errorMessage = 'Please fill in all required fields.';
      this.snackBarService.error(this.errorMessage);
    }
  }
}
