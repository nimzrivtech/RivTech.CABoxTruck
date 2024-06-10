import { Component, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { BrokerGenInfoData } from '../../../../data/broker/general-information.data';
import { ClickTypes } from '../../../../../../shared/constants/click-type.constants';
import { MenuOverrideService } from '../../../../../../core/services/layout/menu-override.service';
import { Router, RouterStateSnapshot, UrlTree } from '@angular/router';
import { NextBackButtonComponent } from '../../submission-details-shared/components/next-back-button/next-back-button.component';
import { DatePipe, Location } from '@angular/common';
import { BrokerInfoDto } from '../../../../../../shared/models/submission/brokerinfo/brokerInfoDto';
import { BrokerValidationService } from '../../../../../../core/services/validations/broker-validation.service';
import { SubmissionNavValidateService } from '../../../../../../core/services/navigation/submission-nav-validate.service';
import { SubmissionData } from '../../../../../../modules/submission/data/submission.data';
import { CanComponentDeactivate } from '@app/core/guards/candeactivate.guard';
import { Observable, of, Subject } from 'rxjs';
import { RaterApiData } from '@app/modules/submission/data/rater-api.data';
import { catchError, map, switchMap, takeUntil, tap } from 'rxjs/operators';
import NotifUtils from '@app/shared/utilities/notif-utils';
import Utils from '@app/shared/utilities/utils';
import { UserCredentials } from '@app/shared/models/management/user';
import { PathConstants } from '@app/shared/constants/path.constants';
import { createSubmissionDetailsMenuItems } from '../../submission-details-navitems';
import { LayoutService } from '@app/core/services/layout/layout.service';

@Component({
  selector: 'app-broker',
  templateUrl: './broker.component.html',
  styleUrls: ['./broker.component.scss']
})
export class BrokerComponent implements OnInit, CanComponentDeactivate, OnDestroy {
  private readonly destroy$ = new Subject();
  private userConfirmedBeforeExit: boolean;

  get riskDetail() {
    return this.submissionData.riskDetail;
  }

  constructor(public Data: BrokerGenInfoData,
    private router: Router,
    private menuOverrideService: MenuOverrideService,
    private location: Location,
    public brokerValidation: BrokerValidationService,
    public submissionData: SubmissionData,
    public formValidation: SubmissionNavValidateService,
    private submissionNavValidateService: SubmissionNavValidateService,
    private layoutService: LayoutService,
  ) { }

  @ViewChild(NextBackButtonComponent) nextBack: NextBackButtonComponent;

  ngOnInit() {
    this.userConfirmedBeforeExit = false;
    this.Data.initiateFormGroup(new BrokerInfoDto());
    this.resetDropdowns();
    this.Data.dateFormatOptions();
    this.Data.initiatePolicyContacts();
  }

  private resetDropdowns() {
    this.Data.dropdownList.agencyList = this.Data.dropdownList.agentList =
      this.Data.dropdownList.retailerList = this.Data.dropdownList.retailAgentList =
      this.Data.dropdownList.reasonForMove = [];
  }

  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
  }

  canDeactivate(nextState: RouterStateSnapshot): Observable<boolean> {
    Utils.blockUI();
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

  private savePage$() {
    return this.Data.saveBrokerGenInfo$().pipe(
      takeUntil(this.destroy$),
      tap(() => {
        this.submissionNavValidateService.validateCurrentCategory(PathConstants.Submission.Broker.Index);
        this.layoutService.updateMenu(createSubmissionDetailsMenuItems(this.submissionData.riskDetail.id, this.submissionData.validationList));
      }),
      catchError(error => {
        NotifUtils.showError(error?.message ?? "An error occured.");
        console.error(error);
        return of(error);
      })
    );
  }

  get isScroll() {
    return this.Data.formInfo.controls['isBrokeredAccount'].value || this.Data.formInfo.controls['isIncumbentAgency'].value || this.Data.formInfo.controls['isMidtermMove'].value;
  }

  get fc() { return this.fg.controls; }
  get fg() { return this.Data.formInfo; }

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
}
