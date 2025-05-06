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
import { EnumUtils } from '../../../core/utils/enum.utils';
import { BorrwalsService } from '../../../core/services/borrwals.service';

@Component({
  selector: 'app-return-book-dialog',
  standalone: true,
  imports: [CommonModule, MatButtonModule, 
        ReactiveFormsModule, MatDialogModule, MatFormFieldModule, MatInputModule, MatSelectModule, MatOptionModule, 
        MatDatepickerModule, MatNativeDateModule],
  templateUrl: './return-book-dialog.component.html',
  styleUrl: './return-book-dialog.component.scss'
})
export class ReturnBookDialogComponent 
{
  private borrowalService = inject(BorrwalsService);
  private dialogRef = inject(MatDialogRef<ReturnBookDialogComponent>);
  private formBuilder = inject(FormBuilder);

  validationErrors?: string[];
  borrowalForm!: FormGroup;
  errorMessage: string = '';

  constructor(@Inject(MAT_DIALOG_DATA) public data: {  }) 
  {    
  }

  ngOnInit(): void 
  {
    this.createForm();
  }

  createForm() 
  {
    this.borrowalForm = this.formBuilder.group({  });
  }

  
  clearForm(): void {
    this.createForm();
    this.errorMessage = '';
    this.validationErrors = undefined;
  }

  onSubmit(): void {  }
}
