import { Component, inject, OnInit } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { HeaderComponent } from "./layout/header/header.component";
import { BookWithAuthorListDto } from './shared/models/book-with-author-list-dto';
import { BookService } from './core/services/book.service';
import { BookListComponent } from "./features/book-list/book-list.component";

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet, HeaderComponent, BookListComponent],
  templateUrl: './app.component.html',
  styleUrl: './app.component.scss'
})
export class AppComponent implements OnInit
{
 
  title = 'LMS-Client';
  private bookService = inject(BookService);

  booksWithAuthorList: BookWithAuthorListDto = {
    bookWithAuthorList: [], 
    count: 0
  };    

  ngOnInit(): void 
  {
    this.bookService.getAllBooksWithAuthors().subscribe({
      next: data => {
        this.booksWithAuthorList = data;
        console.log(this.booksWithAuthorList);
      },
      error: error => console.error('There was an error!', error),
      complete: () => console.log('Request complete')
    });
  }

}
