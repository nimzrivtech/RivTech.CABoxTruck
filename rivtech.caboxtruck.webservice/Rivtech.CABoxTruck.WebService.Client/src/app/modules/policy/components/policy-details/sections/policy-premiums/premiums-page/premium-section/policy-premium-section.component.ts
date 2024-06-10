import { Component, ElementRef, EventEmitter, OnInit, Output, ViewChild } from '@angular/core';
import { SubmissionNavSavingService } from '../../../../../../../../core/services/navigation/submission-nav-saving.service';
import { QuoteLimitsData } from '../../../../../../../../modules/submission/data/quote/quote-limits.data';
import { SubmissionData } from '../../../../../../../../modules/submission/data/submission.data';
import { Subject } from 'rxjs';

@Component({
  selector: 'app-policy-premium-section',
  templateUrl: './policy-premium-section.component.html',
  styleUrls: ['./policy-premium-section.component.scss']
})
export class PolicyPremiumSectionComponent implements OnInit {
  private readonly destroy$ = new Subject();
  hideMe: boolean = false;
  constructor(
    public quoteLimitsData: QuoteLimitsData,
  ) { }

  ngOnInit() {
  }

  public ToggleHiding(): void {
    this.hideMe = !this.hideMe;
  }
}
