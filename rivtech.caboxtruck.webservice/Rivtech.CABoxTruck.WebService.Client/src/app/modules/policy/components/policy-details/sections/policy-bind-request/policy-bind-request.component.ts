import { Component, OnInit } from '@angular/core';
import { NavigationEnd, Router,RouterStateSnapshot,NavigationStart } from '@angular/router';
import { LayoutService } from '@app/core/services/layout/layout.service';
import { NavigationService } from '@app/core/services/navigation/navigation.service';
import { BindingData } from '@app/modules/submission/data/binding/binding.data';
import { RiskSpecificsData } from '@app/modules/submission/data/risk-specifics.data';
import { SubmissionData } from '@app/modules/submission/data/submission.data';
import { ClickTypes } from '@app/shared/constants/click-type.constants';
import { PathConstants } from '@app/shared/constants/path.constants';
import { BindingDto } from '@app/shared/models/submission/binding/BindingDto';
import { BindingRequirementsDto } from '@app/shared/models/submission/binding/BindingRequirementsDto';
import { FileUploadDocumentDto } from '@app/shared/models/submission/binding/FileUploadDocumentDto';
import { QuoteConditionsDto } from '@app/shared/models/submission/binding/QuoteConditionsDto';
import { Subject, Observable, of } from 'rxjs';
import { catchError, map, switchMap, takeUntil, tap } from 'rxjs/operators';
import * as _ from 'lodash';
import FormUtils from '@app/shared/utilities/form.utils';
import { createPolicyDetailsMenuItems } from '../../policy-details-navitems';
import Utils from '@app/shared/utilities/utils';
import NotifUtils from '@app/shared/utilities/notif-utils';
import { createSubmissionDetailsMenuItems } from '@app/modules/submission/components/submission-details/submission-details-navitems';
import { CanComponentDeactivate } from '@app/core/guards/candeactivate.guard';
@Component({
  selector: 'app-policy-bind-request',
  templateUrl: './policy-bind-request.component.html',
  styleUrls: ['./policy-bind-request.component.scss']
})
export class PolicyBindRequestComponent implements OnInit,CanComponentDeactivate {

  private reset$ = new Subject();
  private newCategoryRoute: string;
  private currentCategoryRoute: string;
  private readonly destroy$ = new Subject();
 

  constructor(private Data: BindingData,
              private submissionData: SubmissionData,
              private navigationService: NavigationService,
              private router: Router,
              private layoutService: LayoutService,
              private riskSpecificsData: RiskSpecificsData,
              ) { }

  ngOnInit() {
    this.currentCategoryRoute = this.getCurrentCategoryRoute();
    this.Data.retrieveDropdownValues(() => {
      this.initializeForms();
      this.savingUponNavigate();
      setTimeout(() => {
        // this.Data.loadBindReqDefaults();
        this.Data.loadQuoteConditionDefaults(null, true);
        this.initializeTables();
      }, 1500);
    });
  }

  private initializeForms() {
    if (this.submissionData.riskDetail?.binding) {
      this.Data.initiateBindingFormGroup(this.submissionData.riskDetail?.binding);
    } else {
      this.Data.initiateBindingFormGroup(new BindingDto());
    }

    if (this.submissionData.riskDetail?.bindingRequirements) {
      this.Data.bindingRequirementsList = this.submissionData.riskDetail?.bindingRequirements?.map(x => Object.assign({}, x));
    } else {
      this.Data.bindingRequirementsList = Array<BindingRequirementsDto>();
    }
    this.Data.initiateBindingRequirementsFormGroup(new BindingRequirementsDto());

    if (this.submissionData.riskDetail?.quoteConditions) {
      this.Data.quoteConditionsList = this.submissionData.riskDetail?.quoteConditions?.map(x => Object.assign({}, x));

      const hasSelectedAPD = (this.submissionData?.riskDetail?.riskCoverage?.compDeductibleId ?? 76) !== 76
                      || (this.submissionData?.riskDetail?.riskCoverage?.collDeductibleId ?? 65) !== 65
                      || (this.submissionData?.riskDetail?.riskCoverage?.fireDeductibleId ?? 76) !== 76;
      const hasSymbol10 = (this.submissionData?.riskDetail?.riskCoverage?.alSymbolId && this.submissionData?.riskDetail?.riskCoverage?.alSymbolId == 14 ||
      this.submissionData?.riskDetail?.riskCoverage?.alSymbolId && this.submissionData?.riskDetail?.riskCoverage?.alSymbolId == 15);

      if(!hasSelectedAPD)
        this.Data.quoteConditionsList = this.Data.quoteConditionsList.filter(f => f.quoteConditionsOptionId != '13');

      if(!hasSymbol10)
        this.Data.quoteConditionsList = this.Data.quoteConditionsList.filter(f => f.quoteConditionsOptionId != '23');
    } else {
      this.Data.quoteConditionsList = Array<QuoteConditionsDto>();
    }
    this.Data.initiateQuoteConditionFormGroup(new QuoteConditionsDto());
    this.Data.initiateFileUploadDocumentDtoFormGroup(new FileUploadDocumentDto());
    this.Data.initiateARSCommissionFields();
    this.Data.initiateARSCommissionValues();
  }

  private initializeTables() {
    this.Data.bindingRequirementsList?.forEach((bindingReq) => {
      bindingReq.bindRequirementsOption = this.Data.dropdownList.bindRequirementList?.find(x => x.value == bindingReq?.bindRequirementsOptionId)?.label;
      bindingReq.bindStatus = this.Data.dropdownList.bindStatusList?.find(x => x.value == bindingReq?.bindStatusId)?.label;
    });

    this.Data.quoteConditionsList?.forEach((quoteCondition) => {
      quoteCondition.quoteConditionsOption = this.Data.dropdownList.quoteConditionList?.find(x => x.value == quoteCondition?.quoteConditionsOptionId)?.label;
    });

    // sort onload
    const bindRequirementsOptionEl: HTMLElement = document.querySelector('[id=bindRequirementsOption]');
    bindRequirementsOptionEl?.setAttribute('data-order', 'asc');
    this.Data.sortBindRequirements(bindRequirementsOptionEl);

    const quoteConditionsOptionEl: HTMLElement = document.querySelector('[id=quoteConditionsOption]');
    quoteConditionsOptionEl?.setAttribute('data-order', 'asc');
    this.Data.sortQuoteConditions(quoteConditionsOptionEl);

    this.Data.setRequirementsPage(1, this.Data.bindingRequirementsList);
    this.Data.setConditionsPage(1, this.Data.quoteConditionsList);

    const riskDetail = this.submissionData?.riskDetail;
    if (riskDetail?.binding && riskDetail?.riskCoverages.length > 0) {
      if (riskDetail?.riskCoverages[+riskDetail?.binding?.bindOptionId - 1]?.depositPct < 100) { // If BindOption default value is Option 1
        this.Data.formBinding.controls.paymentTypesId.setValue(2); // Direct Bill Installment
      }
    }
  }

  public onClick(clickType?) {
    switch (clickType) {
      case ClickTypes.Back:
        this.navigationService.navigatePolicyRoute(`${PathConstants.Submission.RiskSpecifics.Index}/${PathConstants.Submission.RiskSpecifics.UWQuestions}`);
        break;
      case ClickTypes.Next:
        this.navigationService.navigatePolicyRoute(PathConstants.Submission.FormsSelection.Index);
        break;
    }
  }

  savingUponNavigate() {
    this.router.events
      .filter(event => event instanceof NavigationEnd)
      .pipe(takeUntil(this.reset$)).subscribe((event: NavigationEnd) => {
        this.newCategoryRoute = this.getCurrentCategoryRoute();
        if (['dashboard', 'login'].includes(this.newCategoryRoute, 0)) {
          return;
        }
        if (this.currentCategoryRoute === PathConstants.Submission.Bind.Index) {
          this.Data.saveBinding();
          this.submissionData.validationList.bind = this.checkBindSection();
          this.layoutService.updateMenu(createPolicyDetailsMenuItems(this.submissionData.riskDetail.riskId, this.submissionData.riskDetail.id, this.submissionData.validationList));
        }
        this.currentCategoryRoute = this.newCategoryRoute;
      });
  }

  private getCurrentCategoryRoute(): string {
    const splitUrl = _.split(this.router.url, '/');
    const currentCategoryIndex = splitUrl.length - 1;
    return splitUrl[currentCategoryIndex];
  }

  get BindForm() {
    return this.Data.formBinding;
  }

  checkBindSection() {
    return this.BindForm.controls['surplusLIneNum'].value !== '';
  }


  canDeactivate(nextState: RouterStateSnapshot): Observable<boolean> {
    return this.savePage$().pipe(
        tap(() => Utils.unblockUI()),
        map(() => true),
        catchError(err => {
          if (!!err.message)
            NotifUtils.showError(err.message);
          else 
            NotifUtils.showError("Error occured while saving.");
          console.error(err);
          return of(false);
        })
      );
  }

  routerNavigation(): Observable<any> {
    return this.router.events.filter(event => event instanceof NavigationStart)
      .pipe(takeUntil(this.reset$));
  }


  private savePage$() {
    return this.Data.resaveBindInfo$().pipe(
      takeUntil(this.destroy$),
      tap(() => {
        this.layoutService.updateMenu(createPolicyDetailsMenuItems(this.submissionData.riskDetail.riskId, this.submissionData.riskDetail.id, this.submissionData.validationList, this.riskSpecificsData.hiddenNavItems));
      }),
      catchError(error => {
        NotifUtils.showError(error?.message ?? "An error occured.");
        console.error(error);
        return of(error);
      })
    );
  }

 
}
