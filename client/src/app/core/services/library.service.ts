import { inject, Injectable } from '@angular/core';
import { environment } from '../../../environments/environment';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { AuthorDto } from '../../shared/models/author-dto';

@Injectable({
  providedIn: 'root'
})
export class LibraryService 
{
  baseUrl = environment.apiBaseUrl;
  private http = inject(HttpClient);

  getAllAuthors(): Observable<AuthorDto[]>
  {
    return this.http.get<AuthorDto[]>(`${this.baseUrl}authors`);
  }
}
