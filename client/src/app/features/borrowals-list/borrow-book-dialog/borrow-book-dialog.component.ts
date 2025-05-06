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
import { BorrowFormDto } from '../../../shared/models/borrow-form-dto';
import { SnackBarService } from '../../../core/services/snack-bar.service';
import { BookGenre } from '../../../shared/models/book-genre';

@Component({
  selector: 'app-borrow-book-dialog',
  standalone: true,
  imports: [CommonModule, MatButtonModule,
    ReactiveFormsModule, MatDialogModule, MatFormFieldModule, MatInputModule, MatSelectModule, MatOptionModule,
    MatDatepickerModule, MatNativeDateModule],
  templateUrl: './borrow-book-dialog.component.html',
  styleUrl: './borrow-book-dialog.component.scss'
})
export class BorrowBookDialogComponent implements OnInit 
{

  private borrowalService = inject(BorrwalsService);
  private dialogRef = inject(MatDialogRef<BorrowBookDialogComponent>);
  private formBuilder = inject(FormBuilder);
  private snackBarService = inject(SnackBarService);

  validationErrors?: string[];
  borrowalForm!: FormGroup;
  errorMessage: string = '';
  borrowFormDto! : BorrowFormDto;
  
  genres = Object.keys(BookGenre)
  .filter(key => isNaN(Number(key))) // filter out numeric keys
  .map(key => ({
    label: key,
    value: BookGenre[key as keyof typeof BookGenre]
  }));

  constructor(@Inject(MAT_DIALOG_DATA) public data: {  }) 
  {    
    this.borrowalService.getBorrowFormData().subscribe(
      {
        next: (data) => {
          this.borrowFormDto = data;
          this.createForm();        
        },
        error: (error) =>  {
          this.errorMessage = 'error in loading borrow form information';
          this.snackBarService.error(this.errorMessage);
        },
        complete:() => {}
      }
    );    
  }

  ngOnInit(): void 
  {  
  }

  createForm() 
  {
    this.borrowalForm = this.formBuilder.group({  
      genre: [[], Validators.required],
      authors:  [[], Validators.required],
      book: [Validators.required]
    });

    this.borrowalForm.get('genre')?.valueChanges.subscribe(selectedGenres => {
      this.filterBooks();
    });
  
    this.borrowalForm.get('authors')?.valueChanges.subscribe(selectedAuthors => {
      this.filterBooks();
    });
  }

  
  clearForm(): void {
    this.createForm();
    this.errorMessage = '';
    this.validationErrors = undefined;
  }

  onSubmit(): void {  }

  private filterBooks()
  {
    const selectedGenres = this.borrowalForm.get('genre')?.value || [];
    const selectedAuthors = this.borrowalForm.get('authors')?.value || [];

    alert('selected genres ' + selectedGenres + '   selected authors ' + selectedAuthors);
  }
}
