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
import { BookGenre } from '../../../shared/models/book-genre';
import { BorrowalStatus } from '../../../shared/models/borrowal-status-enum';
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
    this.createForm();
  }

  private createForm(): void 
  {
    this.searchForm = this.formBuilder.group({
      bookName: [''],
      author: [''],
      genre: [BookGenre.None],
      memberName: [''],
      memberEmail: ['', Validators.email],
      borrowedOn: [],
      dueOn: [new Date()],
      status: [BorrowalStatus.Out],
      delayed: [0]          // 0, 1 or 2
    });
  }

  onSubmit(): void
  {}

  clearForm(): void
  {
    this.createForm();
  }
}
