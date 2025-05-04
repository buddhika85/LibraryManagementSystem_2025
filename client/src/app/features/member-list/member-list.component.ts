import { Component, inject, OnInit, ViewChild } from '@angular/core';
import { MatPaginator, MatPaginatorModule } from '@angular/material/paginator';
import { MatSort, MatSortModule } from '@angular/material/sort';
import { MatTableDataSource, MatTableModule } from '@angular/material/table';
import { MatInputModule } from '@angular/material/input';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MemberService } from '../../core/services/member.service';
import { MatDividerModule } from '@angular/material/divider';
import { MatButtonModule } from '@angular/material/button';
import { MatIcon } from '@angular/material/icon';
import { MatDialog } from '@angular/material/dialog'; 
import { MemberUserDisplayDto, UsersListDto } from '../../shared/models/user-display-dto';
import { SnackBarService } from '../../core/services/snack-bar.service';

@Component({
  selector: 'app-member-list',
  standalone: true,
  imports: [MatFormFieldModule, MatInputModule, MatTableModule, MatSortModule, MatPaginatorModule, 
        MatDividerModule, MatButtonModule, MatIcon],
  templateUrl: './member-list.component.html',
  styleUrl: './member-list.component.scss'
})
export class MemberListComponent implements OnInit
{

  pageTitle = 'Members';
  memberService = inject(MemberService);
  snackbar = inject(SnackBarService);

  userList: UsersListDto = {
    usersList: [],
    count: 0
  };

  // Mat-table 
  displayedColumns: string[] = ['fullName', 'email', 
    'phoneNumber', 'roleStr', 'addressStr', 'activeStr', 'edit', 'status'];
  dataSource!: MatTableDataSource<MemberUserDisplayDto>;
  
  @ViewChild(MatPaginator) paginator!: MatPaginator;
  @ViewChild(MatSort) sort!: MatSort;

  ngOnInit(): void 
  {
    this.loadGridData();
  }

  private loadGridData() {
    
    this.memberService.getAllMembers().subscribe({
      next: data => {
        this.userList = data;
        // Assign the data to the data source for the table to render
        this.dataSource = new MatTableDataSource(this.userList.usersList);
        this.dataSource.paginator = this.paginator;
        this.dataSource.sort = this.sort;
      },
      error: error => console.error('There was an error when loaging members table!', error),
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

  add() {
    //this.openAddEditBookDialog(0);
  }

  edit(email: string) {
    //this.openAddEditBookDialog(id);
  }

  activateDeactivate(email: string) {
    this.memberService.activateDeactivateMembers(email).subscribe({
      next: () => {
        this.loadGridData();
        this.snackbar.success('Member status updated successfully!');
      },
      error: () => this.snackbar.error('There was an error when activating/deactivating member!'),
      complete: () => {}
    });
  }
}
