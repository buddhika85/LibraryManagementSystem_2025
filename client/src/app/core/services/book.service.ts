import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { BookWithAuthorListDto } from '../../shared/models/book-with-author-list-dto';
import { Observable } from 'rxjs';

import { environment } from '../../../environments/environment';
import { BookForEditDto } from '../../shared/models/book-for-edit-dto';
import { BookSaveDto } from '../../shared/models/book-save-dto';
import { ResultDto } from '../../shared/models/result-dto';

@Injectable({
  providedIn: 'root'
})
export class BookService {

  baseUrl = environment.apiBaseUrl; //'https://localhost:5001/api/';
  private http = inject(HttpClient);
  
  getAllBooksWithAuthors() : Observable<BookWithAuthorListDto> 
  {   
    return this.http.get<BookWithAuthorListDto>(`${this.baseUrl}books`);  // same is - this.baseUrl + 'books'
  }

  getBookForEditOrInsert(id: number) : Observable<BookForEditDto>
  {   
    return this.http.get<BookForEditDto>(`${this.baseUrl}books/edit/${id}`);
  }

  insertBook(book: BookSaveDto): Observable<ResultDto>
  {
    return this.http.post<ResultDto>(`${this.baseUrl}books`, book);
  }

  updateBook(book: BookSaveDto): Observable<ResultDto>
  {
    return this.http.put<ResultDto>(`${this.baseUrl}books/${book.id}`, book);
  }
}
