import { Component, OnInit, ViewChild,OnDestroy } from '@angular/core';
import { NextBackButtonComponent } from '../../submission-details-shared/components/next-back-button/next-back-button.component';
import { Location } from '@angular/common';
import { ClickTypes } from '../../../../../../shared/constants/click-type.constants';
import { MenuOverrideService } from '../../../../../../core/services/layout/menu-override.service';
import { Router , RouterStateSnapshot, UrlTree, NavigationStart} from '@angular/router';
import { BindingData } from '../../../../../../modules/submission/data/binding/binding.data';
import { BindingDto } from '../../../../../../shared/models/submission/binding/BindingDto';
import { SubmissionData } from '../../../../../../modules/submission/data/submission.data';
import { FileUploadDocumentDto } from '../../../../../../shared/models/submission/binding/FileUploadDocumentDto';
import { BindingRequirementsDto } from '../../../../../../shared/models/submission/binding/BindingRequirementsDto';
import { QuoteConditionsDto } from '../../../../../../shared/models/submission/binding/QuoteConditionsDto';
import { NotesData } from '../../../../../../modules/submission/data/note/note.data';
import { DocumentData } from '../../../../../../modules/documents/document.data';
import { BindValidationService } from '@app/core/services/validations/bind-validation.service';
import { PathConstants } from '@app/shared/constants/path.constants';
import { SubmissionNavValidateService } from '@app/core/services/navigation/submission-nav-validate.service';
import { LayoutService } from '@app/core/services/layout/layout.service';
import { createSubmissionDetailsMenuItems } from '../../submission-details-navitems';
import { Observable, of, Subject } from 'rxjs';
import { CanComponentDeactivate } from '@app/core/guards/candeactivate.guard';
import { catchError, map, switchMap, takeUntil, tap } from 'rxjs/operators';
import Utils from '@app/shared/utilities/utils';
import NotifUtils from '@app/shared/utilities/notif-utils';
@Component({
  selector: 'app-bind',
  templateUrl: './bind.component.html',
  styleUrls: ['./bind.component.scss']
})
export class BindComponent implements OnInit ,CanComponentDeactivate, OnDestroy  {

  @ViewChild(NextBackButtonComponent) nextBack: NextBackButtonComponent;

  constructor(private location: Location,
    private menuOverrideService: MenuOverrideService,
    private router: Router,
    public Data: BindingData,
    private submissionData: SubmissionData,
    public bindValidation : BindValidationService,
    private submissionNavValidateService: SubmissionNavValidateService,
    private layoutService: LayoutService,
  ) { }

  private readonly destroy$ = new Subject();
  private reset$ = new Subject();

  ngOnInit() {
    //this.Data.renderTable();
    this.Data.retrieveDropdownValues(() => {
      this.Data.initializeForms();
      setTimeout(() => {
        this.Data.loadBindReqDefaults();
        this.Data.loadQuoteConditionDefaults();
        this.Data.initiateARSCommissionValues();
        this.initializeTables();
        // this.Data.saveBinding();
      }, 1500);
    });
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
    this.nextBack.location = this.location;
    let url = '';
    switch (clickType) {
      case ClickTypes.Back:
        this.menuOverrideService.overrideSideBarBehavior();
        url = this.nextBack.getPrev();
        this.router.navigate([url]);
        break;
      case ClickTypes.Next:
        this.menuOverrideService.overrideSideBarBehavior();
        url = this.nextBack.getNext();
        this.router.navigate([url]);
        break;
    }
  }

  get isScroll() {
    return false;
  }

  get hasQuoteIssued() {
    const preBindQuote = this.submissionData.riskDetail?.riskDocuments?.some(f => f.fileName === 'Pre-Bind Quote' && f.isConfidential);
    if (preBindQuote === true) {
      this.submissionData.isQuoteIssued = preBindQuote;
    }
    if (this.submissionData.isQuoteIssued === true) {
      return true;
    }
    return false;
  }

  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
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
      tap(() => {
        this.submissionNavValidateService.validateCurrentCategory(PathConstants.Submission.Bind.Index);
        this.layoutService.updateMenu(createSubmissionDetailsMenuItems(this.submissionData.riskDetail.id, this.submissionData.validationList));
      }),
      catchError(error => {
        NotifUtils.showError(error?.message ?? "An error occured.");
        console.error(error);
        return of(error);
      })
    );
  }

}