import { Component, ElementRef, EventEmitter, OnInit, Output, ViewChild } from '@angular/core';
import { SubmissionNavSavingService } from '../../../../../../../../core/services/navigation/submission-nav-saving.service';
import { QuoteLimitsData } from '../../../../../../../../modules/submission/data/quote/quote-limits.data';
import { SubmissionData } from '../../../../../../../../modules/submission/data/submission.data';
import { Subject } from 'rxjs';

@Component({
  selector: 'app-premium-section',
  templateUrl: './premium-section.component.html',
  styleUrls: ['./premium-section.component.scss']
})
export class PremiumSectionComponent implements OnInit {
  private readonly destroy$ = new Subject();
  hideMe: boolean = false;
  constructor(
    public quoteLimitsData: QuoteLimitsData,
    public submissionData: SubmissionData,
    public submissionNavSavingService: SubmissionNavSavingService
  ) { }

  ngOnInit() {
  }

  public ToggleHiding(): void {
    this.hideMe = !this.hideMe;
  }
}
