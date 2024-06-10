import { Component, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { Router } from '@angular/router';
import { Location } from '@angular/common';
import { ClickTypes } from '../../../../../../../shared/constants/click-type.constants';
import { QuoteValidationService } from '../../../../../../../core/services/validations/quote-validation.service';
import { QuoteLimitsData } from '../../../../../../../modules/submission/data/quote/quote-limits.data';
import { from, Observable, of, Subject, throwError } from 'rxjs';
import { NextBackBtnComponent } from '../../../../../../../shared/components/next-back-btn/next-back-btn.component';
import { PathConstants } from '../../../../../../../shared/constants/path.constants';
import { NavigationService } from '../../../../../../../core/services/navigation/navigation.service';


@Component({
  selector: 'app-policy-premiums-page',
  templateUrl: './policy-premiums-page.component.html',
  styleUrls: ['./policy-premiums-page.component.scss']
})
export class PolicyPremiumsPageComponent implements OnInit, OnDestroy {
  private readonly destroy$ = new Subject();
  isScrollPage: Boolean = true;
  constructor(
    private location: Location,
    private router: Router,
    public quoteLimitsData: QuoteLimitsData,
    private navigationService: NavigationService
  ) { }

  @ViewChild(NextBackBtnComponent) nextBack: NextBackBtnComponent;

  ngOnInit() {
    this.quoteLimitsData.initializePremiumForms();
    // Utils.blockUI();
    // this.quoteLimitsData.loadQuoteOptionsPremiumAvg().pipe(
    //   takeUntil(this.destroy$),
    //   catchError(error => {
    //     console.error(error);
    //     NotifUtils.showError("An error occured.");
    //     return of(error);
    //   }),
    //   tap(() => Utils.unblockUI())
    // ).subscribe();
  }

  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
  }

  public onClick(clickType?) {
    switch (clickType) {
      case ClickTypes.Back:
        this.navigationService.navigatePolicyRoute(PathConstants.Policy.Rating);
        break;
      case ClickTypes.Next:
        this.navigationService.navigatePolicyRoute(PathConstants.Policy.Issuance);
        break;
    }
  }
}
