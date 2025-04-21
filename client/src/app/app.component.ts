import { Component, inject, OnInit } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { HeaderComponent } from "./layout/header/header.component";
import { HttpClient } from '@angular/common/http';
import { BookWithAuthorListDto } from './shared/models/book-with-author-list-dto';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet, HeaderComponent],
  templateUrl: './app.component.html',
  styleUrl: './app.component.scss'
})
export class AppComponent implements OnInit
{
 
  title = 'LMS-Client';
  baseUrl = 'https://localhost:5001/api/';
  private http = inject(HttpClient);

  booksWithAuthorList: BookWithAuthorListDto = {
    bookWithAuthorList: [], 
    count: 0
  };    

  ngOnInit(): void {
    this.http.get<BookWithAuthorListDto>(this.baseUrl + 'books').subscribe({
      next: data => {
        this.booksWithAuthorList = data;
        console.log(this.booksWithAuthorList);
      },
      error: error => console.error('There was an error!', error),
      complete: () => console.log('Request complete')
    });
  }

}
