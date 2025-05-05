import { Component, inject, Inject, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MAT_DIALOG_DATA, MatDialogRef, MatDialogModule } from '@angular/material/dialog';
import { ReactiveFormsModule } from '@angular/forms';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { MatOptionModule } from '@angular/material/core';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatNativeDateModule } from '@angular/material/core';
import { MatButtonModule } from '@angular/material/button';
import { UserInfoDto } from '../../shared/models/user-info-dto';
import { UserRoles } from '../../shared/models/user-roles-enum';


@Component({
  selector: 'app-add-edit-user-dialog',
  standalone: true,
  imports: [CommonModule, MatButtonModule, 
    ReactiveFormsModule, MatDialogModule, MatFormFieldModule, MatInputModule, MatSelectModule, MatOptionModule, 
    MatDatepickerModule, MatNativeDateModule],
  templateUrl: './add-edit-user-dialog.component.html',
  styleUrl: './add-edit-user-dialog.component.scss'
})
export class AddEditUserDialogComponent implements OnInit
{
 
  userType: UserRoles;
  userRoleStr: string;
  user: UserInfoDto;

  errorMessage: string = '';
  private formBuilder = inject(FormBuilder);
  validationErrors?: string[];  
  userAddEditForm!: FormGroup;


  private dialogRef = inject(MatDialogRef<AddEditUserDialogComponent>);

  constructor(@Inject(MAT_DIALOG_DATA) public data: { userType: UserRoles, user: UserInfoDto}) 
  {
    this.userType = data.userType;
    this.userRoleStr = UserRoles[this.userType];
    this.user = data.user;
  }

  
  ngOnInit(): void 
  {
    this.createForm();
  }

  createForm() 
  {
    this.userAddEditForm = this.formBuilder.group({  
      firstName: [this.user.firstName, Validators.required],
      lastName: [this.user.lastName, Validators.required],
      email: [this.user.email, [Validators.required, Validators.email]],
      phoneNumber: [this.user.phoneNumber, Validators.required],
      password: ['*******', Validators.required],                             // API will generate a temporary password and email to user for changes - staff, or admin does not know it
      line1: [this.user.address.line1, Validators.required],
      line2: [this.user.address.line2, Validators.required],
      city: [this.user.address.city, Validators.required],
      state: [this.user.address.state, Validators.required],
      postcode: [this.user.address.postcode, Validators.required],
      country: [this.user.address.country, Validators.required]
    });
  }

  clearForm(): void {
    this.createForm();
    this.errorMessage = '';
    this.validationErrors = undefined;
  }

  onSubmit(): void {
  
     
  }
}







