import {  Component, inject, OnInit, ViewChild} from '@angular/core';
import { BorrwalsService } from '../../../core/services/borrwals.service';
import { BorrowalSummaryDto, BorrowalSummaryListDto } from '../../../shared/models/borrowal-summary-dto';
import { SnackBarService } from '../../../core/services/snack-bar.service';
import { MatTableDataSource } from '@angular/material/table';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';

@Component({
  selector: 'app-member-borrowals-history',
  standalone: true,
  imports: [],
  templateUrl: './member-borrowals-history.component.html',
  styleUrl: './member-borrowals-history.component.scss'
})
export class MemberBorrowalsHistoryComponent implements OnInit
{  
  pageTitle = 'Your Borrowals Hisotry';
  
  private borrowalsService = inject(BorrwalsService);
  private snackbar = inject(SnackBarService);

  history: BorrowalSummaryListDto = {
    borrowalSummaries: [],
    count: 0
  }

  // Mat-table 
  displayedColumns: string[] = ['borrowalsId', 'bookTitle', 'bookPic', 'borrowalDate', 'dueDate', 'borrowalStatusStr'];
  dataSource!: MatTableDataSource<BorrowalSummaryDto>;
  
  @ViewChild(MatPaginator) paginator!: MatPaginator;
  @ViewChild(MatSort) sort!: MatSort;

  ngOnInit(): void 
  {
    this.loadGridData();    
  }

  private loadGridData() 
  {
    this.borrowalsService.getBorrowalSummaryForMember().subscribe({
    next: (data: BorrowalSummaryListDto) => {
      this.history = data;
        // Assign the data to the data source for the table to render
        this.dataSource = new MatTableDataSource(this.history.borrowalSummaries);
        this.dataSource.paginator = this.paginator;
        this.dataSource.sort = this.sort;
      },
      error: (error) => {
        const errorMsg = `Error in loading member borrowals history ${error}`;
        console.log(errorMsg)
        this.snackbar.error(errorMsg);
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
}
