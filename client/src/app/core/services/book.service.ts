import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { BookWithAuthorListDto } from '../../shared/models/book-with-author-list-dto';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class BookService {

  baseUrl = 'https://localhost:5001/api/';
  private http = inject(HttpClient);
  
  getAllBooksWithAuthors() : Observable<BookWithAuthorListDto> 
  {
    return this.http.get<BookWithAuthorListDto>(this.baseUrl + 'books');
  }
}
