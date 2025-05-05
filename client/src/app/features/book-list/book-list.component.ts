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
import { environment } from '../../../environments/environment';

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
  pageTitle = 'Books';

  // Mat-table 
  displayedColumns: string[] = ['bookId', 'bookPictureUrl', 'bookTitle', 'authorListStr', 'bookGenreStr', 'bookPublishedDateStr', 'isAvailable','edit', 'delete'];
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
    this.loadGridData();
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
    this.openAddEditBookDialog(0);
  }
    
  deleteBook(bookId: number) {
    //alert('Delete book button clicked! BookId: ' + bookId);
    this.bookService.deleteBook(bookId).subscribe(
      {
        next: () => {
          this.loadGridData();
        },
        error: error => console.error(`There was an error when deleting book with ID ${bookId}`, error),
        complete: () => {}
      }
    );
  }
  
  editBook(bookId: number) {
    // alert('Edit book button clicked! BookId: ' + bookId);
    this.openAddEditBookDialog(bookId);    
  }

  private openAddEditBookDialog(bookId: number) {
    this.bookService.getBookForEditOrInsert(bookId).subscribe(
      {
        next: data => {

          // blur active element to avoid aria-hidden warning
          (document.activeElement as HTMLElement)?.blur();
          
          const dialogRef = this.dialog.open(AddEditBookDialogComponent, {
            width: '600px',
            data: {
              authors: data.allAuthors,
              book: data.book
            }
          }).afterClosed().subscribe(result => {
            if (result) {
              this.loadGridData();
            }
          });
        },
        error: error => console.error(`There was an error when finding book with ID ${bookId}`, error),
        complete: () => {}
      }
    );    
  }

  

  private loadGridData() {
    this.bookService.getAllBooksWithAuthors().subscribe({
      next: data => {
        this.booksWithAuthorList = data;
        // Assign the data to the data source for the table to render
        this.dataSource = new MatTableDataSource(this.booksWithAuthorList.bookWithAuthorList);
        this.dataSource.paginator = this.paginator;
        this.dataSource.sort = this.sort;
        console.log(this.booksWithAuthorList);
      },
      error: error => console.error('There was an error when loaging books table!', error),
      complete: () => {}
    });
  }
  
  getImageUrl(imageName: string): string 
  {
    return imageName ? `${environment.apiBookImageUrl}${imageName}` : `${environment.apiImagesUrl}no-image-available.jpg`;
  }
    
}