import {  Component, inject, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ReactiveFormsModule } from '@angular/forms';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { MatOptionModule } from '@angular/material/core';
import { MatAutocompleteModule } from '@angular/material/autocomplete';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatNativeDateModule } from '@angular/material/core';
import { MatButtonModule } from '@angular/material/button';
import {MatExpansionModule} from '@angular/material/expansion';
import {MatIconModule} from '@angular/material/icon';

import { AuthorDto } from '../../../shared/models/author-dto';
import { BorrowalsSearchDto } from '../../../shared/models/borrowals-search-dto';
import { BookGenre } from '../../../shared/models/book-genre';
import { BorrowalStatus } from '../../../shared/models/borrowal-status-enum';
import { LibraryService } from '../../../core/services/library.service';
import { SnackBarService } from '../../../core/services/snack-bar.service';
@Component({
  selector: 'app-borrowals-search',
  standalone: true,
  imports: [
    CommonModule, MatButtonModule,
        ReactiveFormsModule, MatFormFieldModule, MatInputModule, MatSelectModule, MatOptionModule,
        MatDatepickerModule, MatNativeDateModule, MatAutocompleteModule,
         MatExpansionModule,
    MatIconModule,
  ],
  templateUrl: './borrowals-search.component.html',
  styleUrl: './borrowals-search.component.scss'
})
export class BorrowalsSearchComponent implements OnInit
{
  private libraryService = inject(LibraryService);
  private snackBarService = inject(SnackBarService);
  private formBuilder = inject(FormBuilder);
  searchForm!: FormGroup;

  genres = Object.keys(BookGenre)
      .filter(key => isNaN(Number(key))) // filter out numeric keys
      .map(key => ({
        label: key,
        value: BookGenre[key as keyof typeof BookGenre]
  }));

  statuses = Object.keys(BorrowalStatus)
      .filter(key => isNaN(Number(key))) // filter out numeric keys
      .map(key => ({
        label: key,
        value: BorrowalStatus[key as keyof typeof BorrowalStatus]
  }));
  
  authors: AuthorDto[] = [];

  ngOnInit(): void {
    this.libraryService.getAllAuthors().subscribe({
      next: (data) => {
        this.authors = data;
        this.createForm();
      },
      error: (error) => {
        this.snackBarService.error('Error in loading all authors');
      },
      complete: () => {}
    });
    
  }

  private createForm(): void 
  {
    this.searchForm = this.formBuilder.group({
      bookName: [''],
      authorIds: [],
      genres: [],   // {value: [BookGenre.None]}
      memberName: [''],
      memberEmail: ['', Validators.email],
      borrowedOn: [],
      dueOn: [new Date()],
      statuses: [[BorrowalStatus.Out]],   // BorrowalStatus.Out
      delayed: [0]          // 0, 1 or 2
    });
  }

  onSubmit(): void
  {
    const searchParams: BorrowalsSearchDto = {
        bookName: this.searchForm.value.bookName,
        authorIds: this.searchForm.value.authorIds,
        genres: this.searchForm.value.genres,
        memberName: this.searchForm.value.memberName,
        memberEmail: this.searchForm.value.memberEmail,
        borrowedOn: new Date(this.searchForm.value.borrowedOn), // Ensure type correctness
        dueOn: new Date(this.searchForm.value.dueOn),
        statuses: this.searchForm.value.statuses,
        delayed: this.searchForm.value.delayed
    };
    console.log(searchParams);
  }

  clearForm(): void
  {
    this.createForm();
  }
}
