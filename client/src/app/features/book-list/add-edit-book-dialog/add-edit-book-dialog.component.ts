import { Component, Inject, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MAT_DIALOG_DATA, MatDialogRef, MatDialogModule } from '@angular/material/dialog';

import { BookGenre } from '../../../shared/models/book-genre';
import { BookSaveDto } from '../../../shared/models/book-save-dto';
import { AuthorDto } from '../../../shared/models/author-dto';

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
  selector: 'app-add-edit-book-dialog',
  standalone: true,
  imports: [CommonModule, MatButtonModule, 
      ReactiveFormsModule, MatDialogModule, MatFormFieldModule, MatInputModule, MatSelectModule, MatOptionModule, 
      MatDatepickerModule, MatNativeDateModule],
  templateUrl: './add-edit-book-dialog.component.html',
  styleUrl: './add-edit-book-dialog.component.scss'
})
export class AddEditBookDialogComponent implements OnInit 
{
  form!: FormGroup;
  
  genres = Object.keys(BookGenre)
    .filter(key => isNaN(Number(key))) // filter out numeric keys
    .map(key => ({
      label: key,
      value: BookGenre[key as keyof typeof BookGenre]
    }));

  authors: AuthorDto[] = [];
  book?: BookSaveDto;

  constructor(private fb: FormBuilder, private dialogRef: MatDialogRef<AddEditBookDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: { book?: BookSaveDto, authors: AuthorDto[] }) 
  {
    this.book = data.book;
    this.authors = data.authors;
  }

  ngOnInit(): void {
    this.form = this.fb.group({
      id: [this.book?.id || 0],
      title: [this.book?.title || '', Validators.required],
      genre: [this.book == null ? BookGenre.None : EnumUtils.convertToBookGenre(this.book.genre) || '', Validators.required],
      authorIds: [this.book?.authorIds || [], Validators.required],
      publishedDate: [this.book?.publishedDate || '', Validators.required],
      pictureUrl: [this.book?.pictureUrl || ''],
    });
  }

  onFileSelected(event: Event) {
    const file = (event.target as HTMLInputElement).files?.[0];
    if (file) {
      // Optional: Handle file upload logic (e.g., send to backend or convert to base64)
      this.form.patchValue({ pictureUrl: file.name });
    }
  }

  onSave() {
    if (this.form.valid) {
      this.dialogRef.close(this.form.value);
    }
  }

  onCancel() {
    this.dialogRef.close();
  }
}
