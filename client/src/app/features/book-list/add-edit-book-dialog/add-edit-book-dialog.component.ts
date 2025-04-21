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


@Component({
  selector: 'app-add-edit-book-dialog',
  standalone: true,
  imports: [CommonModule, MatButtonModule, ReactiveFormsModule, MatDialogModule, MatFormFieldModule, MatInputModule, MatSelectModule, MatOptionModule, MatDatepickerModule, MatNativeDateModule],
  templateUrl: './add-edit-book-dialog.component.html',
  styleUrl: './add-edit-book-dialog.component.scss'
})
export class AddEditBookDialogComponent implements OnInit 
{
  form!: FormGroup;
  genres = Object.values(BookGenre);
  authors: AuthorDto[] = [];

  constructor(private fb: FormBuilder, private dialogRef: MatDialogRef<AddEditBookDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: { book?: BookSaveDto, authors: AuthorDto[] }) 
  {
    this.authors = data.authors;
  }

  ngOnInit(): void {
    this.form = this.fb.group({
      id: [this.data.book?.id || 0],
      title: [this.data.book?.title || '', Validators.required],
      genre: [this.data.book?.genre || '', Validators.required],
      authorIds: [this.data.book?.authorIds || [], Validators.required],
      publishedDate: [this.data.book?.publishedDate || '', Validators.required],
      pictureUrl: [this.data.book?.pictureUrl || ''],
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
