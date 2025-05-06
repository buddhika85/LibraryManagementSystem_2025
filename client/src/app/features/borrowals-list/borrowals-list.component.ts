import { Component, inject, OnInit, ViewChild } from '@angular/core';
import { BorrwalsService } from '../../core/services/borrwals.service';
import { BorrowalsDisplayListDto } from '../../shared/models/borrowals-display-list-dto';
import { SnackBarService } from '../../core/services/snack-bar.service';

import { MatDividerModule } from '@angular/material/divider';
import { MatButtonModule } from '@angular/material/button';
import { MatIcon } from '@angular/material/icon';
import { MatDialog } from '@angular/material/dialog';
import { MatPaginator, MatPaginatorModule } from '@angular/material/paginator';
import { MatSort, MatSortModule } from '@angular/material/sort';
import { MatTableDataSource, MatTableModule } from '@angular/material/table';
import { MatInputModule } from '@angular/material/input';
import { MatFormFieldModule } from '@angular/material/form-field';
import { BorrowalsDisplayDto } from '../../shared/models/borrowals-display-dto';
import { environment } from '../../../environments/environment';

@Component({
  selector: 'app-borrowals-list',
  standalone: true,
  imports: [MatFormFieldModule, MatInputModule, MatTableModule, MatSortModule, MatPaginatorModule, 
        MatDividerModule, MatButtonModule, MatIcon],
  templateUrl: './borrowals-list.component.html',
  styleUrl: './borrowals-list.component.scss'
})
export class BorrowalsListComponent implements OnInit
{
  // page labels
  pageTitle = 'Borrowals';

  // Mat-table 
  displayedColumns: string[] = ['id', 'bookDisplayStr', 'bookPictureUrl', 'memberName', 'memberEmail', 
      'borrowalDateStr', 'dueDateStr', 'borrowalStatusStr', 'return'];
  dataSource!: MatTableDataSource<BorrowalsDisplayDto>;

  @ViewChild(MatPaginator) paginator!: MatPaginator;
  @ViewChild(MatSort) sort!: MatSort;

  borrowalsService = inject(BorrwalsService);
  borrowals: BorrowalsDisplayListDto = { borrowalsList : [], count: 0 };

  snackBarService = inject(SnackBarService);

  ngOnInit(): void 
  {
    this.loadBorrowals();
  }

  private loadBorrowals()   
  {
    this.borrowalsService.GetAllBorrowals().subscribe({
      next: (data) =>  {        
        this.borrowals = data;
        this.dataSource = new MatTableDataSource(this.borrowals.borrowalsList);
        this.dataSource.paginator = this.paginator;
        this.dataSource.sort = this.sort;
        console.log(this.borrowals);
      },
      error: () =>  {
        this.snackBarService.error('Unable to load borrowals');
      },
      complete: () => {}
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

  borroBook(){
    //alert('borrow');
  }

  returnBook(borrowalId: number)
  {
    alert('Return ' + borrowalId);
  }

  getImageUrl(imageName: string): string 
  {
    return imageName ? `${environment.apiBookImageUrl}${imageName}` : `${environment.apiImagesUrl}no-image-available.jpg`;
  }
}
