import { Component, inject, OnInit, ViewChild } from '@angular/core';
import { MatPaginator, MatPaginatorModule } from '@angular/material/paginator';
import { MatSort, MatSortModule } from '@angular/material/sort';
import { MatTableDataSource, MatTableModule } from '@angular/material/table';
import { MatInputModule } from '@angular/material/input';
import { MatFormFieldModule } from '@angular/material/form-field';
import { BookService } from '../../core/services/book.service';
import { BookWithAuthorListDto } from '../../shared/models/book-with-author-list-dto';
import { BookWithAuthorsDto } from '../../shared/models/book-with-authors-dto';
import { MatDividerModule } from '@angular/material/divider';
import { MatButtonModule } from '@angular/material/button';
import { MatIcon } from '@angular/material/icon';
import { MatDialog } from '@angular/material/dialog';
import { AddEditBookDialogComponent } from './add-edit-book-dialog/add-edit-book-dialog.component';
import { BookGenre } from '../../shared/models/book-genre';

@Component({
  selector: 'app-book-list',
  standalone: true,
  imports: [MatFormFieldModule, MatInputModule, MatTableModule, MatSortModule, MatPaginatorModule, 
      MatDividerModule, MatButtonModule, MatIcon],
  templateUrl: './book-list.component.html',
  styleUrl: './book-list.component.scss'
})
export class BookListComponent implements OnInit {


  // page labels
  pageTitle = 'Books List';

  // Mat-table 
  displayedColumns: string[] = ['bookId', 'bookTitle', 'bookGenreStr', 'bookPublishedDateStr', 'edit', 'delete'];
  dataSource!: MatTableDataSource<BookWithAuthorsDto>;

  @ViewChild(MatPaginator) paginator!: MatPaginator;
  @ViewChild(MatSort) sort!: MatSort;


  // for data fetching
  private bookService = inject(BookService);
  booksWithAuthorList: BookWithAuthorListDto = {
    bookWithAuthorList: [], 
    count: 0
  };    

  // dialog
  readonly dialog = inject(MatDialog);

  ngOnInit(): void 
  {
    this.bookService.getAllBooksWithAuthors().subscribe({
      next: data => {
        this.booksWithAuthorList = data;
        // Assign the data to the data source for the table to render
        this.dataSource = new MatTableDataSource(this.booksWithAuthorList.bookWithAuthorList);
        this.dataSource.paginator = this.paginator;
        this.dataSource.sort = this.sort;
        console.log(this.booksWithAuthorList);
      },
      error: error => console.error('There was an error!', error),
      complete: () => console.log('Request complete')
    });
  }

  // seach filter on top of the table
  applyFilter(event: Event) {
    const filterValue = (event.target as HTMLInputElement).value;
    this.dataSource.filter = filterValue.trim().toLowerCase();

    if (this.dataSource.paginator) {
      this.dataSource.paginator.firstPage();
    }
  }

  addBook() {
    // alert('Add book button clicked!');
    const dialogRef = this.dialog.open(AddEditBookDialogComponent, {
      width: '600px',
      data: {
        authors: [], // Array<{ id: number, name: string }>
        book: null
      }
    }).afterClosed().subscribe(result => {
      if (result) {
        // Save book logic here
      }
    });

  }
    
  deleteBook(bookId: number) {
    alert('Delete book button clicked! BookId: ' + bookId);
  }
  
  editBook(bookId: number) {
    // alert('Edit book button clicked! BookId: ' + bookId);

    const dialogRef = this.dialog.open(AddEditBookDialogComponent, {
      width: '600px',
      data: {
        authors: [], // Array<{ id: number, name: string }>
        book: {
          id: 1,
          title: 'The Time Machine',
          genre: BookGenre.Fantasy,
          publishedDate: new Date('1895-05-07'),
          pictureUrl: 'the-time-machine.jpg',
          authors: [
            {
              id: 101,
              name: 'H.G. Wells',
              country: 'UK',
              biography: 'English writer known for science fiction works.',
              dateOfBirth: new Date('1866-09-21'),
              dateOfBirthStr: '21/09/1866'
            }
          ]
        }
      }
    }).afterClosed().subscribe(result => {
      if (result) {
        // Save book logic here
      }
    });
  }
}