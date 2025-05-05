import { Component, inject, Inject, OnInit } from '@angular/core';
import { UserInfoDto } from '../../../shared/models/user-info-dto';

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
import { EnumUtils } from '../../../core/utils/enum.utils';

@Component({
  selector: 'app-add-edit-member-dialog',
  standalone: true,
  imports: [CommonModule, MatButtonModule, 
        ReactiveFormsModule, MatDialogModule, MatFormFieldModule, MatInputModule, MatSelectModule, MatOptionModule, 
        MatDatepickerModule, MatNativeDateModule],
  templateUrl: './add-edit-member-dialog.component.html',
  styleUrl: './add-edit-member-dialog.component.scss'
})
export class AddEditMemberDialogComponent implements OnInit
{
  private user: UserInfoDto;
  private fb = inject(FormBuilder);
  private dialogRef = inject(MatDialogRef<AddEditMemberDialogComponent>);

  constructor(@Inject(MAT_DIALOG_DATA) public data: { user: UserInfoDto}) 
    {
      this.user = data.user;
    }

  
  ngOnInit(): void 
  {
    this.createForm();
  }

  createForm() 
  {
    
  }
}
