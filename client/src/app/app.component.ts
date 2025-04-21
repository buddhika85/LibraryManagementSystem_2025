import { Component, inject, OnInit } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { HeaderComponent } from "./layout/header/header.component";
import { HttpClient } from '@angular/common/http';

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

  ngOnInit(): void {
    this.http.get(this.baseUrl + 'books').subscribe({
      next: data => console.log(data),
      error: error => console.error('There was an error!', error),
      complete: () => console.log('Request complete')
    });
  }

}
