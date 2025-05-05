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
import { MemberService } from '../../core/services/member.service';
import { ResultDto } from '../../shared/models/result-dto';
import { InsertUpdateUserDto } from '../../shared/models/insert-update-user-dto';
import { AddressDto } from '../../shared/models/address-dto';
import { UserUpdateDto } from '../../shared/models/user-update-dto';


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
 
  isAddMode: boolean;
  userType: UserRoles;
  userRoleStr: string;
  user: UserInfoDto;

  errorMessage: string = '';
  private formBuilder = inject(FormBuilder);
  private memberService = inject(MemberService);
  validationErrors?: string[];  
  userAddEditForm!: FormGroup;


  private dialogRef = inject(MatDialogRef<AddEditUserDialogComponent>);

  constructor(@Inject(MAT_DIALOG_DATA) public data: { userType: UserRoles, user: UserInfoDto}) 
  {
    this.userType = data.userType;
    this.userRoleStr = UserRoles[this.userType];
    this.user = data.user;
    this.isAddMode = this.user.email === '';
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
      postcode: [this.user.address.postcode, Validators.required]
    });
  }

  clearForm(): void {
    this.createForm();
    this.errorMessage = '';
    this.validationErrors = undefined;
  }

  onSubmit(): void {  
    if (this.userAddEditForm.valid) 
    {             
        if (this.isAddMode)   
        {          
          this.addMember();          
        }
        else
        {
          this.updateUser();
        }
    }
    else
    {
      this.errorMessage = 'Error - please fill the form properly !';
    }

  }


  private addMember(): void
  {
    const memberDto: InsertUpdateUserDto = {... this.userAddEditForm.value };   
    memberDto.address = this.createAddressDto();
    memberDto.role = UserRoles.member;
    this.memberService.addMember(memberDto).subscribe({
      next: (result: ResultDto) => {        
        if(result && result.isSuccess) {
          this.dialogRef.close(true);
        }
        else {
          this.errorMessage = 'Error inserting member!';
          console.log(`${this.errorMessage} ${result?.errorMessage}`);          
        }
      },
      error: (errors) => {        
        this.errorMessage = '';
        console.log(`${errors}`);
        this.validationErrors = errors;
      },
      complete: () => {}
    });
  }

  private updateUser() : void
  {
    const dto: UserUpdateDto = { ... this.userAddEditForm.value };
    dto.address = this.createAddressDto();
    this.memberService.updateMember(this.userAddEditForm.value.email, dto).subscribe({
      next: () => { 
          this.dialogRef.close(true);
      },
      error: (errors) => {   
        debugger     
        this.errorMessage = '';
        console.log(`${errors}`);
        this.validationErrors = errors;
      },
      complete: () => {}
    });
  }

  private createAddressDto(): AddressDto
  {
    const address: AddressDto = { 
      line1:  this.userAddEditForm.value.line1, 
      line2:  this.userAddEditForm.value.line2,
      city:  this.userAddEditForm.value.city,
      state:  this.userAddEditForm.value.state,
      postcode:  this.userAddEditForm.value.postcode,
      country:  'Australia'
    }
    return address;
  }
}







