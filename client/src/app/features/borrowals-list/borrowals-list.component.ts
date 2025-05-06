import { Component, inject, OnInit } from '@angular/core';
import { BorrwalsService } from '../../core/services/borrwals.service';
import { BorrowalsDisplayListDto } from '../../shared/models/borrowals-display-list-dto';
import { SnackBarService } from '../../core/services/snack-bar.service';

@Component({
  selector: 'app-borrowals-list',
  standalone: true,
  imports: [],
  templateUrl: './borrowals-list.component.html',
  styleUrl: './borrowals-list.component.scss'
})
export class BorrowalsListComponent implements OnInit
{
  borrowalsService = inject(BorrwalsService);
  borrwals: BorrowalsDisplayListDto = { borrowalsList : [], count: 0};

  snackBarService = inject(SnackBarService);

  ngOnInit(): void 
  {
    this.loadBorrowals();
  }

  private loadBorrowals()   
  {
    this.borrowalsService.GetAllBorrowals().subscribe({
      next: (data) =>  {        
        this.borrwals = data;
        console.log(this.borrwals);
      },
      error: () =>  {
        this.snackBarService.error('Unable to load borrowals');
      },
      complete: () => {}
    });
  }
}
