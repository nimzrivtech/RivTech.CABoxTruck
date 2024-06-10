import { Component, HostListener, OnInit, ViewChild } from '@angular/core';
import { LimitsData } from '../../../../../../../../../modules/submission/data/limits/limits.data';
import { OptionForm, QuoteLimitsData, QuoteLimitsForm } from '../../../../../../../../../modules/submission/data/quote/quote-limits.data';
import { SubmissionData } from '../../../../../../../../../modules/submission/data/submission.data'; import { RaterApiData } from '../../../../../../../../../modules/submission/data/rater-api.data';
import { LayoutService } from '../../../../../../../../../core/services/layout/layout.service';
import { GeneralLiabilityCargoValidationService } from '../../../../../../../../../core/services/validations/general-liability-cargo-validation.service';
import { SubmissionNavValidateService } from '../../../../../../../../../core/services/navigation/submission-nav-validate.service';
import { PathConstants } from '../../../../../../../../../shared/constants/path.constants';
import { createSubmissionDetailsMenuItems } from '../../../../../../../../../modules/submission/components/submission-details/submission-details-navitems';
import { RiskSpecificsData } from '../../../../../../../../../modules/submission/data/risk-specifics.data';
import { ToastrService } from 'ngx-toastr';
import { SubmissionNavSavingService } from '../../../../../../../../../core/services/navigation/submission-nav-saving.service';
import { finalize, first, map, switchMap, take, tap } from 'rxjs/operators';
import { FormArray, FormBuilder, FormControl, FormGroup } from '@angular/forms';
import { RiskCoverageDTO, RiskCoverageListDTO } from '@app/shared/models/submission/limits/riskCoverageDto';
import Utils from '../../../../../../../../../shared/utilities/utils';
import { BehaviorSubject, Observable, of } from 'rxjs';
import { RiskCoveragePremiumDTO } from '@app/shared/models/submission/limits/riskCoveragePremiumDto';
import { Subscription } from 'rxjs';
import { ReportData } from '@app/modules/submission/data/report/report.data';
import { BindingData } from '../../../../../../../../../modules/submission/data/binding/binding.data';

@Component({
  selector: 'app-policy-premium-vehicle-chart',
  templateUrl: './policy-premium-vehicle-chart.component.html',
  styleUrls: ['./policy-premium-vehicle-chart.component.scss']
})
export class PolicyPremiumVehicleChartComponent implements OnInit {
  maxArray: number = 3;

  currencySettings: any = {
    align: 'left',
    allowNegative: false,
    precision: 2,
    nullable: true
  };

  showPropertyDamageField: boolean = false;

  /**
   * Holds references to all subscription that will be unsubscribed upon component's onDestroy event.
   */
  subscriptions: Subscription[] = [];

  /**
   * Indicates whether the component is ready to do some tasks upon init or waits if not yet ready. 
   */
  isReady = new BehaviorSubject<boolean>(false);
  
  get premiumAvgForm() {
    return this.quoteLimitsData.premiumAvgForm;
  }

  get optionsPremiumArray() {
    return this.quoteLimitsData.optionsPremiumArray;
  }

  constructor(
    public quoteLimitsData: QuoteLimitsData,
    public limitsData: LimitsData,
  ) {
    // this.quoteLimitsData.initiateFormFields();
  }

  ngOnInit() {
    // this.quoteLimitsData.loadQuoteOptionsPremiumAvg().pipe().subscribe();
    // Utils.blockUI();
    this.quoteLimitsData.loadPremiumAvgMetrics().pipe(
      // tap(() => Utils.unblockUI())
    ).subscribe();
  }

  // tslint:disable-next-line: use-lifecycle-interface
  async ngOnDestroy() {
    // unloading premium when changing url or redirecting
    //this.raterApiData.UnloadPremium(true); //Moved to submission-details.component for all pages use
    //this.subscriptions.forEach(subs => subs.unsubscribe());

  }
}

