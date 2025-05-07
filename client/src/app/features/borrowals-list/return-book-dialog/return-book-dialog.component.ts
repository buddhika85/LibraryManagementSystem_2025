import { ChangeDetectorRef, NgZone, Component, inject, Inject, OnInit } from '@angular/core';
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
import { BorrowalReturnInfoDto } from '../../../shared/models/borrowal-return-info-dto';
import { SnackBarService } from '../../../core/services/snack-bar.service';

@Component({
  selector: 'app-return-book-dialog',
  standalone: true,
  imports: [CommonModule, MatButtonModule, 
        ReactiveFormsModule, MatDialogModule, MatFormFieldModule, MatInputModule, MatSelectModule, MatOptionModule, 
        MatDatepickerModule, MatNativeDateModule],
  templateUrl: './return-book-dialog.component.html',
  styleUrl: './return-book-dialog.component.scss'
})
export class ReturnBookDialogComponent implements OnInit
{
  private borrowalService = inject(BorrwalsService);
  private dialogRef = inject(MatDialogRef<ReturnBookDialogComponent>);
  private formBuilder = inject(FormBuilder);
  private snackBarService = inject(SnackBarService);

  // for UI change detections after API calls
  private cdRef = inject(ChangeDetectorRef);
  private ngZone = inject(NgZone);

  borrowalId: number;
  borrowalReturnInfoDto!: BorrowalReturnInfoDto;

  validationErrors?: string[];
  returnForm!: FormGroup;
  errorMessage: string = '';


  constructor(@Inject(MAT_DIALOG_DATA) public data: { borrowalId: number  }) 
  {  
    this.borrowalId = data.borrowalId;  

    this.borrowalService.getBorrowalReturnInfoDto(this.borrowalId).subscribe({
      next: (data: BorrowalReturnInfoDto) => {
        if (data.isSuccess)
        {
          this.ngZone.run(() => {  // Ensures updates happen inside Angular’s detection cycle
            this.borrowalReturnInfoDto = data;
            console.log(this.borrowalReturnInfoDto);
            this.createForm();
            //this.cdRef.detectChanges();           // Manually refresh bindings - without this it gives a bounding error
          });
        }
        else
        {
          this.snackBarService.error('Error in loading book return form');
        }
      },
      error: (error) => {},
      complete: () => {}
    });
  }

  ngOnInit(): void 
  {
    // moved the code to constructor 
    // ERROR RuntimeError: NG0100: ExpressionChangedAfterItHasBeenCheckedError: Expression has changed after it was checked.
  }

  createForm() 
  {
    this.returnForm = this.formBuilder.group({  
      borrowalId: [{value: this.borrowalReturnInfoDto.borrowalId, disabled: true}],
      bookDisplayStr: [{value: this.borrowalReturnInfoDto.borrowalsDisplayDto?.bookDisplayStr, disabled: true}],
      
      
      borrowalDateStr: [{value: this.borrowalReturnInfoDto.borrowalDateStr, disabled: true}, Validators.required],
      dueDateStr: [{value: this.borrowalReturnInfoDto.dueDateStr, disabled: true}, Validators.required],
      isOverdue: [{value: this.borrowalReturnInfoDto.isOverdue, disabled: true}, Validators.required],
      
      lateDays: [{value: this.borrowalReturnInfoDto.lateDays, disabled: true}, Validators.required],
      perDayLateFeeDollars: [{value: this.borrowalReturnInfoDto.perDayLateFeeDollars, disabled: true}, Validators.required],
      amountDue: [{value: this.borrowalReturnInfoDto.amountDue, disabled: true}, Validators.required],
      
      paid: [{value: false, disabled: this.borrowalReturnInfoDto.amountDue === 0.0}, Validators.required]
    });
  }

  
  clearForm(): void {
    this.createForm();
    this.errorMessage = '';
    this.validationErrors = undefined;
  }

  onSubmit(): void {  }
}
