import { ChangeDetectorRef, Component, inject, Inject, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MAT_DIALOG_DATA, MatDialogRef, MatDialogModule } from '@angular/material/dialog';
import { ReactiveFormsModule } from '@angular/forms';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { MatOptionModule } from '@angular/material/core';
import { MatAutocompleteModule } from '@angular/material/autocomplete';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatNativeDateModule } from '@angular/material/core';
import { MatButtonModule } from '@angular/material/button';
import { BorrwalsService } from '../../../core/services/borrwals.service';
import { BorrowFormDto } from '../../../shared/models/borrow-form-dto';
import { SnackBarService } from '../../../core/services/snack-bar.service';
import { BookGenre } from '../../../shared/models/book-genre';
import { BookWithAuthorsDto } from '../../../shared/models/book-with-authors-dto';
import { BookFilterDto } from '../../../shared/models/book-filter-dto';
import { environment } from '../../../../environments/environment';
import { MemberUserDisplayDto, UserDisplayDto } from '../../../shared/models/user-display-dto';
import { map, Observable, startWith } from 'rxjs';

@Component({
  selector: 'app-borrow-book-dialog',
  standalone: true,
  imports: [CommonModule, MatButtonModule,
    ReactiveFormsModule, MatDialogModule, MatFormFieldModule, MatInputModule, MatSelectModule, MatOptionModule,
    MatDatepickerModule, MatNativeDateModule, MatAutocompleteModule],
  templateUrl: './borrow-book-dialog.component.html',
  styleUrl: './borrow-book-dialog.component.scss'
})
export class BorrowBookDialogComponent implements OnInit 
{

  private borrowalService = inject(BorrwalsService);
  private dialogRef = inject(MatDialogRef<BorrowBookDialogComponent>);
  private formBuilder = inject(FormBuilder);
  private snackBarService = inject(SnackBarService);
  private cdRef = inject(ChangeDetectorRef);

  validationErrors?: string[];
  borrowalForm!: FormGroup;
  errorMessage: string = '';
  borrowFormDto! : BorrowFormDto; 

  filterMembers$!: Observable<UserDisplayDto[]>;
  
  genres = Object.keys(BookGenre)
  .filter(key => isNaN(Number(key))) // filter out numeric keys
  .map(key => ({
    label: key,
    value: BookGenre[key as keyof typeof BookGenre]
  }));

  booksWithAuthorsList: BookWithAuthorsDto[] = [];

  constructor(@Inject(MAT_DIALOG_DATA) public data: {  }) 
  {    
    this.borrowalService.getBorrowFormData().subscribe(
      {
        next: (data) => {
          this.borrowFormDto = data;
          console.log(this.borrowFormDto);    
          
          

          this.createForm();   
          
          this.filterMembers$ = this.borrowalForm.get('member')!.valueChanges.pipe(
            startWith(''),
            map(value => this.filterMembers(value || '')) 
          );
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
      book: [Validators.required],
      member: ['', Validators.required]
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
    const filter: BookFilterDto = { 
      authorIds: this.borrowalForm.get('authors')?.value || [],
      bookGenres: this.borrowalForm.get('genre')?.value || []
    };
    this.borrowalService.filterBooks(filter).subscribe({
      next: (data) => {
        this.booksWithAuthorsList = data.bookWithAuthorList;
      },
      error: (error) => {
        this.errorMessage = 'error in loading book information based on selected authors and genres';
        this.snackBarService.error(this.errorMessage);
      },
      complete: () => {}
    });
  }


  getImageUrl(imageName: string): string 
  {
    return imageName ? `${environment.apiBookImageUrl}${imageName}` : `${environment.apiImagesUrl}no-image-available.jpg`;
  }

  filterMembers(value: string): UserDisplayDto[] 
  {
    if (!this.borrowFormDto?.members) return [];

    const filterValue = value.toLowerCase();
    const filteredMembers = this.borrowFormDto.members.filter(member =>
      member.firstName.toLowerCase().includes(filterValue) ||
      member.lastName.toLowerCase().includes(filterValue) ||
      member.email.toLowerCase().includes(filterValue)
    );

    this.cdRef.detectChanges(); // Force UI refresh
    return filteredMembers;

  }

}
