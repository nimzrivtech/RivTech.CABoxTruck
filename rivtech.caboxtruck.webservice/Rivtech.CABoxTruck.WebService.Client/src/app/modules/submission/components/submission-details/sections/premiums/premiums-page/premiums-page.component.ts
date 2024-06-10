import { Component, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { Router } from '@angular/router';
import { Location } from '@angular/common';
import { MenuOverrideService } from '../../../../../../../core/services/layout/menu-override.service';
import { NextBackButtonComponent } from '../../../submission-details-shared/components/next-back-button/next-back-button.component';
import { ClickTypes } from '../../../../../../../shared/constants/click-type.constants';
import { QuoteValidationService } from '../../../../../../../core/services/validations/quote-validation.service';
import { SubmissionNavValidateService } from '../../../../../../../core/services/navigation/submission-nav-validate.service';
import { QuoteLimitsData } from '../../../../../../../modules/submission/data/quote/quote-limits.data';
import { SubmissionData } from '@app/modules/submission/data/submission.data';
import Utils from '@app/shared/utilities/utils';
import { from, Observable, of, Subject, throwError } from 'rxjs';
import { catchError, map, switchMap, takeUntil, tap } from 'rxjs/operators';
import { RaterApiData } from '@app/modules/submission/data/rater-api.data';
import NotifUtils from '@app/shared/utilities/notif-utils';
import { ToastrService } from 'ngx-toastr';
import { FormsData } from '../../../../../data/forms/forms.data';

@Component({
  selector: 'app-premiums-page',
  templateUrl: './premiums-page.component.html',
  styleUrls: ['./premiums-page.component.scss']
})
export class PremiumsPageComponent implements OnInit, OnDestroy {
  private readonly destroy$ = new Subject();
  isScrollPage: Boolean = true;
  constructor(
    private location: Location,
    private router: Router,
    public quoteLimitsData: QuoteLimitsData,
    public quotesValidation: QuoteValidationService,
    public formValidation: SubmissionNavValidateService,
    private menuOverrideService: MenuOverrideService,
  ) { }

  @ViewChild(NextBackButtonComponent) nextBack: NextBackButtonComponent;

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
    this.nextBack.location = this.location;
    const prevUrl = this.nextBack.getPrev();
    const nextUrl = this.nextBack.getNext();
    switch (clickType) {
      case ClickTypes.Back:
        this.router.navigate([prevUrl]);
        break;
      case ClickTypes.Next:
        this.menuOverrideService.overrideSideBarBehavior();
        this.router.navigate([nextUrl]);
        break;
    }
  }
}
