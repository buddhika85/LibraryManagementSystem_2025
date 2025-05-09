import {  Component, inject, OnInit} from '@angular/core';
import { BorrwalsService } from '../../../core/services/borrwals.service';
import { BorrowalSummaryListDto } from '../../../shared/models/borrowal-summary-dto';

@Component({
  selector: 'app-member-borrowals-history',
  standalone: true,
  imports: [],
  templateUrl: './member-borrowals-history.component.html',
  styleUrl: './member-borrowals-history.component.scss'
})
export class MemberBorrowalsHistoryComponent implements OnInit
{  
  private borrowalsService = inject(BorrwalsService);

  ngOnInit(): void 
  {
    this.borrowalsService.getBorrowalSummaryForMember().subscribe({
      next: (data: BorrowalSummaryListDto) => {
        console.log(data);
      },
      error: (error) => {
        console.log(`Error in loading member borrowals history ${error}`)
      },
      complete: () => {}
    });
  }
}
