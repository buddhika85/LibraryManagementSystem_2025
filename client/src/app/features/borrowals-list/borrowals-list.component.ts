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
import { BorrowalsSearchDto } from '../../shared/models/borrowals-search-dto';
import { environment } from '../../../environments/environment';
import { BorrowBookDialogComponent } from './borrow-book-dialog/borrow-book-dialog.component';
import { ReturnBookDialogComponent } from './return-book-dialog/return-book-dialog.component';
import { BorrowalsSearchComponent } from "./borrowals-search/borrowals-search.component";

import {MatExpansionModule} from '@angular/material/expansion';
import {MatIconModule} from '@angular/material/icon';

@Component({
  selector: 'app-borrowals-list',
  standalone: true,
  imports: [MatFormFieldModule, MatInputModule, MatTableModule, MatSortModule, MatPaginatorModule,
    MatDividerModule, MatButtonModule, MatIcon, BorrowalsSearchComponent,
    MatExpansionModule,
      MatIconModule,],
  templateUrl: './borrowals-list.component.html',
  styleUrl: './borrowals-list.component.scss'
})
export class BorrowalsListComponent implements OnInit
{
  // page labels
  pageTitle = 'Borrowals';

  // Mat-table 
  displayedColumns: string[] = ['id', 'bookDisplayStr', 'bookPictureUrl', 'memberName', 'memberEmail', 
      'borrowalDateStr', 'dueDateStr', 'isDelayed', 'borrowalStatusStr', 'return'];
  dataSource!: MatTableDataSource<BorrowalsDisplayDto>;

  @ViewChild(MatPaginator) paginator!: MatPaginator;
  @ViewChild(MatSort) sort!: MatSort;

  borrowalsService = inject(BorrwalsService);
  borrowals: BorrowalsDisplayListDto = { borrowalsList : [], count: 0 };

  snackBarService = inject(SnackBarService);

  // dialog
  readonly dialog = inject(MatDialog);

  ngOnInit(): void 
  {
    this.loadGridData();
  }

  private loadGridData()   
  {
    this.borrowalsService.getAllBorrowals().subscribe({
      next: (data) =>  {        
        this.borrowals = data;
        this.dataSource = new MatTableDataSource(this.borrowals.borrowalsList);
        this.dataSource.paginator = this.paginator;
        this.dataSource.sort = this.sort;
        //console.log(this.borrowals);
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

  borrowBook(){
    // blur active element to avoid aria-hidden warning
    (document.activeElement as HTMLElement)?.blur();

    const dialogRef = this.dialog.open(BorrowBookDialogComponent, {
      width: '1100px',  // width
      maxWidth: '90vw', // Optional: Prevent exceeding viewport width    
      data: {
        // userType: UserRoles.member,
        // user: user                
      }
    }).afterClosed().subscribe(result => {
      if (result) {        
        this.loadGridData();
      }
    });
  }

  returnBook(borrowalId: number)
  {
    //alert('Return ' + borrowalId);
    // blur active element to avoid aria-hidden warning
    (document.activeElement as HTMLElement)?.blur();

    const dialogRef = this.dialog.open(ReturnBookDialogComponent, {
      width: '1100px',  // width
      maxWidth: '90vw', // Optional: Prevent exceeding viewport width  
      data: {
        borrowalId: borrowalId                
      }
    }).afterClosed().subscribe(result => {
      if (result) {        
        this.loadGridData();
      }
    });
  }

  getImageUrl(imageName: string): string 
  {
    return imageName ? `${environment.apiBookImageUrl}${imageName}` : `${environment.apiImagesUrl}no-image-available.jpg`;
  }

  onSearchParamUpdate(borrowalsSearchParams: BorrowalsSearchDto)
  {
    if (borrowalsSearchParams.applyFilters)
      console.log(borrowalsSearchParams);
    else
    {
      console.log('clear form');
      this.loadGridData();
    }
  }
}
