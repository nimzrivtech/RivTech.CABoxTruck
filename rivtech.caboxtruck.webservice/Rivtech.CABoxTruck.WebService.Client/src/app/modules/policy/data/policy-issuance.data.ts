import { Injectable } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { LayoutService } from '@app/core/services/layout/layout.service';
import { RaterApiData } from '@app/modules/submission/data/rater-api.data';
import { SubmissionData } from '@app/modules/submission/data/submission.data';
import { RiskSpecificsData } from '@app/modules/submission/data/risk-specifics.data';
import { BindIssueService } from '@app/modules/submission/services/bind-issue.service';
import { EndorsementService } from '@app/modules/submission/services/endorsement.service';
import { BindAndIssueLabelConstants } from '@app/shared/constants/bind-and-issue.labels.constants';
import { ErrorMessageConstant } from '@app/shared/constants/error-message.constants';
import { PathConstants } from '@app/shared/constants/path.constants';
import FormUtils from '@app/shared/utilities/form.utils';
import NotifUtils from '@app/shared/utilities/notif-utils';
import Utils from '@app/shared/utilities/utils';
import { IAngularMyDpOptions, IMyDate } from 'angular-mydatepicker';
import * as moment from 'moment';
import { ToastrService } from 'ngx-toastr';
import { BehaviorSubject, forkJoin, Observable, of, Subscription, throwError } from 'rxjs';
import { catchError, finalize, switchMap, take, tap } from 'rxjs/operators';
import { createPolicyDetailsMenuItems } from '../components/policy-details/policy-details-navitems';
import { PolicySummaryData } from './policy-summary.data';
import { DatePipe } from '@angular/common';
import { VehicleData } from '@app/modules/submission/data/vehicle/vehicle.data';
import { Moment } from 'moment';
import { EndorsementProcessQueueDTO } from '@app/shared/models/policy/Issuance/endorsement-process-queue-dto';
import { BackgroundService } from '@app/core/services/BackgroundService';
import { NotificationFilterDTO } from '@app/shared/models/notification/notification-filter.dto';
import { NotificationService } from '@app/core/services/NotificationService';
import { NotificationDTO } from '@app/shared/models/notification/notification.dto';

@Injectable(
  { providedIn: 'root' }
)
export class PolicyIssuanceData {

  issuanceForm: FormGroup;
  datepickerOptions: IAngularMyDpOptions;
  policyChanges: string[] = [];
  enableIssueAndResetButtons: boolean = false;
  premiumChangeList: any;
  isNewlyIssued: boolean = false;
  isLoadingChanges: boolean = false;
  hasError: boolean;
  endorsementEffectiveDatePrompted: boolean;  
  issuanceNotification: NotificationDTO;
  issuanceDefaultNotification: NotificationDTO;
  issuanceFailedNotification: NotificationDTO;
  public isIssuanceFailed: boolean = false;

  private _endorsementEffectiveDate: BehaviorSubject<Date> = new BehaviorSubject<Date>(null);
  get endorsementEffectiveDate$(): Observable<any> { return this._endorsementEffectiveDate.asObservable(); }

  constructor(private formBuilder: FormBuilder,
    private submissionData: SubmissionData,
    private bindIssueService: BindIssueService,
    private router: Router,
    private toastr: ToastrService,
    private raterApiData: RaterApiData,
    private endorsementService: EndorsementService,
    private policySummaryData: PolicySummaryData,
    private riskSpecificsData: RiskSpecificsData,
    private layoutService: LayoutService,
    public vehicleData: VehicleData,
    private datePipe: DatePipe,
    private notificationService: NotificationService,
    private backgroundService: BackgroundService) { }

  initiateFormFields() {
    this.isNewlyIssued = false;
    this.isLoadingChanges = false;
    this.policyChanges = [];
    const effectiveDate = this.submissionData.riskDetail?.endorsementEffectiveDate;
    const policyEffectiveDate = moment(this.submissionData.riskDetail?.brokerInfo.effectiveDate).subtract(1, 'day').toDate();

    console.debug(`Expiration Date: ${this.submissionData.riskDetail?.brokerInfo?.expirationDate}`);
    const policyExpirationDate = moment(this.submissionData.riskDetail?.brokerInfo?.expirationDate).toDate();

    this.datepickerOptions = {
      dateRange: false,
      dateFormat: 'mm/dd/yyyy',
      disableUntil: this.convertToIMyDate(policyEffectiveDate),
      disableSince: this.convertToIMyDate(policyExpirationDate),
      inputFieldValidation: true
    };

    console.debug(this.datepickerOptions.disableSince);

    this.issuanceForm = this.formBuilder.group({
      effectiveDate: [{
        isRange: false, singleDate: {
          jsDate: new Date(effectiveDate)
        }
      }, Validators.required],
      policyChanges: [null, Validators.required],
      endorsementText: [null, Validators.required]
    });

    return this.issuanceForm;
  }

  get hasPendingEndorsement(): boolean {
    let isActive = this.policySummaryData.policyStatus == "Active";
    let hasPendingEndorsement = this.policySummaryData.policySubStatus == "Pending Endorsement";
    return isActive && hasPendingEndorsement;
  }

  convertToIMyDate(date: Date) : IMyDate {
    return { year: date.getFullYear(), month: date.getMonth() + 1, day: date.getDate() };
  }

  populateFormFields() {
    console.info("Populating issuance...", {
      effectiveDte: this.submissionData?.riskDetail?.endorsementEffectiveDate
    });
    this.issuanceForm.get('effectiveDate').setValue({ isRange: false, singleDate: { jsDate: new Date(this.submissionData?.riskDetail?.endorsementEffectiveDate ?? new Date(this.submissionData.currentServerDateTime)) } });
    this.issuanceForm.get('policyChanges').setValue(JSON.stringify(this.policyChanges));
    //this.enableIssueAndResetButtons = this.hasPendingEndorsement && this.validateRequiredToRateModules();
  }

  resetFormFields() {
    this.issuanceForm.controls['endorsementText'].patchValue(null);
    this.issuanceForm.get('effectiveDate').setValue({ isRange: false, singleDate: { jsDate: new Date(this.submissionData.currentServerDateTime) } });
    this.issuanceForm.get('policyChanges').setValue(null);
    this.enableIssueAndResetButtons = false;
    this.policyChanges = [];
  }

  getPolicyChanges$(): Observable<any> {
    return this.bindIssueService.GetPolicyChanges(this.submissionData.riskDetail.id).pipe(
      tap((data) => {
        this.policyChanges = data;
        this.issuanceForm.controls['policyChanges'].patchValue(JSON.stringify(this.policyChanges));
        this.issuanceForm.controls['endorsementText'].patchValue(this.policyChanges.join('\n'));
      }),
      catchError((error) => {
        console.error(error);
        NotifUtils.showError(ErrorMessageConstant.getPolicyChangesErrorMsg);
        return throwError(error);
      })
    );
  }

  IssueEndorsement() {
    Utils.blockUI();
    const policyChanges = {
      riskDetailId: this.submissionData.riskDetail.id,
      effectiveDate: this.issuanceForm.get('effectiveDate').value?.singleDate?.jsDate?.toLocaleDateString(),
      endorsementText: this.issuanceForm.value.endorsementText,
      policyChanges: this.issuanceForm.value.policyChanges
    };
    this.bindIssueService.IssueEndorsement(policyChanges).pipe(take(1)).subscribe(data => {
      NotifUtils.showSuccessWithConfirmationFromUser(BindAndIssueLabelConstants.issuedSuccessfully, () => {
        this.policySummaryData.setEnableEditPolicy(false);
        this.isNewlyIssued = true;
        this.policySummaryData.updateEstimatedPremium(true);
        this.resetFormFields();
        this.router.navigateByUrl(`/policies/${this.submissionData.riskDetail.riskId}/${data.id}/${PathConstants.Policy.Summary}`);
      });
    }, (error) => {
      console.error(error);
      NotifUtils.showError(ErrorMessageConstant.issuePolicyErrorMsg);
    });
  }

  ResetPolicy() {
      NotifUtils.showSuccessWithConfirmationFromUser(BindAndIssueLabelConstants.resetSuccessfully, () => {
        this.submissionData.getRiskByIdAsync(this.submissionData.riskDetail.id).pipe(
          take(1),
          finalize(() => {
            this.vehicleData.isVehicleUpdated = false;
            this.loadPremiumChanges$(false).subscribe();
          }) 
        ).subscribe((submissionData) => {
          window.location.reload();
        });
      });
  }

  validateRequiredToRateModules(): boolean {
    const excludeBindreq = ['businessDetailsBinding', 'bind', 'brokerBinding'];
    const riskValidationList = Object.keys(this.submissionData.validationList).filter(x => !excludeBindreq.find(r => r === x));
    const invalidModule = riskValidationList
      .find(e => e !== 'uwAnswersConfirmed' && this.submissionData.validationList[e] == false);
    const lstToValidate: Array<any> = [undefined, null];
    return (lstToValidate.indexOf(invalidModule) !== -1);
  }

  triggerIssueEndorsementBackgroundProcess(processType: string) {
    Utils.blockUI();

    const notificationDetails = {
      riskDetailId: this.submissionData.riskDetail.id,
      effectiveDate: this.issuanceForm.get('effectiveDate').value?.singleDate?.jsDate?.toLocaleDateString(),
      endorsementText: this.issuanceForm.value.endorsementText,
      policyChanges: JSON.stringify(this.policyChanges)
    };

    const notificationFilterDTO: NotificationFilterDTO = {
      category: processType,
      notificationDetails: JSON.stringify(notificationDetails),
      riskId: this.submissionData.riskDetail.riskId,
      policyNumber: this.submissionData.riskDetail.policyNumber,
      companyName: this.submissionData.riskDetail.nameAndAddress.businessName,
      oldRiskDetailId: this.submissionData.riskDetail.id
    };
    this.backgroundService.triggerBackgroundProcess(notificationFilterDTO).pipe(take(1)).subscribe(data => {
      NotifUtils.showSuccessWithConfirmationFromUser(BindAndIssueLabelConstants.queueProcessIssueEndorsement, () => {
        //this.policySummaryData.canEditPolicy = false;
        //this.isNewlyIssued = true;
        //this.policySummaryData.updateEstimatedPremium(true);
        //this.resetFormFields();
        //this.router.navigateByUrl(`/policies/${this.submissionData.riskDetail.riskId}/${this.submissionData.riskDetail.id}/${PathConstants.Policy.Summary}`);
      });
    }, (error) => {
      console.error(error);
      NotifUtils.showError(ErrorMessageConstant.issuePolicyErrorMsg);
    });
  } 
  
  triggerResetBackgroundProcess(processType: string) {
    Utils.blockUI();
    NotifUtils.showConfirmMessage(BindAndIssueLabelConstants.areYouSureYouWantToReset, () => {
      const notificationDetails = {
        riskDetailId: this.submissionData.riskDetail.id,
        effectiveDate: this.issuanceForm.get('effectiveDate').value?.singleDate?.jsDate?.toLocaleDateString(),
        endorsementText: this.issuanceForm.value.endorsementText,
        policyChanges: JSON.stringify(this.policyChanges)
      };

      const notificationFilterDTO: NotificationFilterDTO = {
        category: processType,
        notificationDetails: JSON.stringify(notificationDetails),
        riskId: this.submissionData.riskDetail.riskId,
        policyNumber: this.submissionData.riskDetail.policyNumber,
        companyName: this.submissionData.riskDetail.nameAndAddress.businessName,
        oldRiskDetailId: this.submissionData.riskDetail.id
      };
      this.backgroundService.triggerBackgroundProcess(notificationFilterDTO).pipe(take(1)).subscribe(data => {
        NotifUtils.showSuccessWithConfirmationFromUser(BindAndIssueLabelConstants.queueProcessReset, () => {
          //this.policySummaryData.canEditPolicy = false;
          //this.isNewlyIssued = true;
          //this.policySummaryData.updateEstimatedPremium(true);
          //this.resetFormFields();
          //this.router.navigateByUrl(`/policies/${this.submissionData.riskDetail.riskId}/${this.submissionData.riskDetail.id}/${PathConstants.Policy.Summary}`);
        });
      }, (error) => {
        console.error(error);
        NotifUtils.showError(ErrorMessageConstant.issuePolicyErrorMsg);
      });
    });
  }

  triggerIssuanceBackgroundProcess(processType: string) {
    // Utils.blockUI();

    this.clearPremiumChangesTable();

    var effectiveDate = this.issuanceForm.controls["effectiveDate"].value.singleDate.jsDate;
    var effectiveDateText = this.datePipe.transform(effectiveDate, "MM-dd-yyyy");
    var effectiveDate$ = this.updateEndorsementEffectiveDate(this.submissionData.riskDetail.id, effectiveDateText);

    const notificationDetails = {
      riskDetailId: this.submissionData.riskDetail.id,
      policyChanges: JSON.stringify(this.policyChanges)
    };

    const notificationFilterDTO: NotificationFilterDTO = {
      category: processType,
      riskId: this.submissionData.riskDetail.riskId,
      policyNumber: this.submissionData.riskDetail.policyNumber,
      companyName: this.submissionData.riskDetail.nameAndAddress.businessName,
      oldRiskDetailId: this.submissionData.riskDetail.id,
      notificationDetails: JSON.stringify(notificationDetails)
    };
    this.backgroundService.triggerBackgroundProcess(notificationFilterDTO).pipe(take(1)).subscribe(data => {
      NotifUtils.showSuccessWithConfirmationFromUser(BindAndIssueLabelConstants.queueProcessIssuance, () => {
        //this.policySummaryData.canEditPolicy = false;
        //this.isNewlyIssued = true;
        //this.policySummaryData.updateEstimatedPremium(true);
        //this.resetFormFields();
        //this.router.navigateByUrl(`/policies/${this.submissionData.riskDetail.riskId}/${this.submissionData.riskDetail.id}/${PathConstants.Policy.Summary}`);
      });
    }, (error) => {
      console.error(error);
      NotifUtils.showError(ErrorMessageConstant.issuePolicyErrorMsg);
    });
  }

  triggerIssuanceDefaultBackgroundProcess(processType: string){
    // Utils.blockUI();

    this.clearPremiumChangesTable();
    
    const notificationDetails = {
      policyChanges: JSON.stringify(this.policyChanges)
    };

    const notificationFilterDTO: NotificationFilterDTO = {
      category: processType,
      riskId: this.submissionData.riskDetail.riskId,
      policyNumber: this.submissionData.riskDetail.policyNumber,
      companyName: this.submissionData.riskDetail.nameAndAddress.businessName,
      oldRiskDetailId: this.submissionData.riskDetail.id,
      notificationDetails: JSON.stringify(notificationDetails)
    };
    this.backgroundService.triggerBackgroundProcess(notificationFilterDTO).pipe(take(1)).subscribe(data => {
      NotifUtils.showSuccessWithConfirmationFromUser(BindAndIssueLabelConstants.queueProcessIssuanceDefault, () => {
        //this.policySummaryData.canEditPolicy = false;
        //this.isNewlyIssued = true;
        //this.policySummaryData.updateEstimatedPremium(true);
        //this.resetFormFields();
        //this.router.navigateByUrl(`/policies/${this.submissionData.riskDetail.riskId}/${this.submissionData.riskDetail.id}/${PathConstants.Policy.Summary}`);
      });
    }, (error) => {
      console.error(error);
      NotifUtils.showError(ErrorMessageConstant.issuePolicyErrorMsg);
    });
  }

  processEndorsement$(riskId: string, processType: string): Observable<any> {
    const endorsementProcessQueueDTO: EndorsementProcessQueueDTO = {
      riskId: riskId,
      processType: processType,
      riskDetailId: this.submissionData.riskDetail.id,
      effectiveDate: this.issuanceForm.get('effectiveDate').value?.singleDate?.jsDate?.toLocaleDateString(),
      endorsementText: this.issuanceForm.value.endorsementText,
      policyChanges: this.issuanceForm.value.policyChanges
    };

    return this.endorsementService.QueueProcessEndorsement$(endorsementProcessQueueDTO);
  }

  processIssuanceReset$(riskId: string): Observable<any> {
    const endorsementProcessQueueDTO: EndorsementProcessQueueDTO = {
      riskId: riskId,
      processType: 'Reset',
      riskDetailId: this.submissionData.riskDetail.id,
      effectiveDate: this.issuanceForm.get('effectiveDate').value?.singleDate?.jsDate?.toLocaleDateString(),
      endorsementText: this.issuanceForm.value.endorsementText,
      policyChanges: this.issuanceForm.value.policyChanges
    };

    return this.endorsementService.QueueProcessEndorsement$(endorsementProcessQueueDTO);
  }

  loadPremiumChanges$(forIssuance?: boolean): Observable<any> {
    if (forIssuance == null)
      forIssuance = true;

    this.isLoadingChanges = true;
    this.hasError = false;

    return this.endorsementService.GetEndorsementPremiumChanges$(this.submissionData.riskDetail.id).pipe(take(1))
      .pipe(
        tap((data) => this.premiumChangeList = data),
        catchError((error) => {
          console.error(error);
          this.hasError = true;
          return throwError(error);
        }),
        finalize(() => {
          //this.populateFormFields();
          this.raterApiData.preloadRater(true).subscribe();
        })
      );
  }

  get isFormValid() {
    return FormUtils.validateForm(this.issuanceForm);
  }

  updateEndorsementEffectiveDate(riskDetailId: string, effectiveDate: string, updateByButton = false): Observable<any> {
    if (updateByButton) {
      return this.endorsementService.updateEffectiveDateByButton(riskDetailId, effectiveDate)
      .pipe(
        tap((data) => {
          this.submissionData.riskDetail.endorsementEffectiveDate = data;
          this._endorsementEffectiveDate.next(data);
        })
      );
    }
    return this.endorsementService.updateEffectiveDate(riskDetailId, effectiveDate)
      .pipe(
        tap((data) => {
          this.submissionData.riskDetail.endorsementEffectiveDate = data;
          this._endorsementEffectiveDate.next(data);
        })
      );
  }

  clearPolicyChanges() {
    this.policyChanges = null;
    this.issuanceForm.controls['endorsementText'].patchValue(null);
    this.issuanceForm.controls['policyChanges'].patchValue(null);
  }

  clearPremiumChangesTable() {
    this.premiumChangeList = [];
  }

  //Corrects the premium values when premium difference is 0.03
  setCorrectPremiumChangesValues() {
    let tempVehicleChangeList: any = [];

    console.log("Resetting premium values....");

    this.premiumChangeList.vehicleChangeList.forEach((vehiclePremiumChange: any) => {
      if (vehiclePremiumChange.endorsementPendingChange.premiumDifference >= -0.03 &&
        vehiclePremiumChange.endorsementPendingChange.premiumDifference <= 0.03) {
        vehiclePremiumChange.endorsementPendingChange.premiumBeforeEndorsement =
          vehiclePremiumChange.endorsementPendingChange.premiumAfterEndorsement;
        vehiclePremiumChange.endorsementPendingChange.premiumDifference = 0;
      }
      tempVehicleChangeList.push(vehiclePremiumChange);
    });

    this.premiumChangeList.vehicleChangeList = tempVehicleChangeList;

    console.log("Resetting premium values done");
  }

  hasEndorsementEffectiveDatePrompted$() {
    if (this.endorsementEffectiveDatePrompted == null) {
      var riskId = this.submissionData.riskDetail.riskId;
      return this.endorsementService.hasEffectiveDatePrompted$(riskId);
    }
    return of(this.endorsementEffectiveDatePrompted);
  }

  updateEndorsementEffectiveDatePrompt$(prompted: boolean) {
    const riskId = this.submissionData.riskDetail.riskId;
    return this.endorsementService.updateEffectiveDatePrompted$(riskId, prompted);
  }

  isPolicyLocked$(notificationStatus: string, processType: string = null) {

    const notificationFilterDTO: NotificationFilterDTO = {
      category: processType,
      riskId: this.submissionData.riskDetail.riskId,
      oldRiskDetailId: this.submissionData.riskDetail.id,
      notificationStatus: notificationStatus
    };
    return this.notificationService.GetNotificationByFilter(notificationFilterDTO)
    .pipe(
      tap((data) => {
        this.policySummaryData.policyNotification = data;
      })
    );
  }

  isIssuanceFailed$(notificationStatus: string, processType: string = null) {

    const notificationFilterDTO: NotificationFilterDTO = {
      category: processType,
      riskId: this.submissionData.riskDetail.riskId,
      oldRiskDetailId: this.submissionData.riskDetail.id,
      notificationStatus: notificationStatus
    };
    return this.notificationService.GetNotificationByFilter(notificationFilterDTO)
    .pipe(
      tap((data) => {
        this.issuanceFailedNotification = data;
      })
    );
  }

  isIssuanceLoadSuccessful$(notificationStatus: string, processType: string = null) {

    const notificationFilterDTO: NotificationFilterDTO = {
      category: processType,
      riskId: this.submissionData.riskDetail.riskId,
      oldRiskDetailId: this.submissionData.riskDetail.id,
      notificationStatus: notificationStatus
    };
    return this.notificationService.GetNotificationByFilter(notificationFilterDTO).pipe(
      tap((data) => {
        this.issuanceNotification = data;
      })
    );
  }

  isIssuanceDefaultLoadSuccessful$(notificationStatus: string, processType: string = null) {

    const notificationFilterDTO: NotificationFilterDTO = {
      category: processType,
      riskId: this.submissionData.riskDetail.riskId,
      oldRiskDetailId: this.submissionData.riskDetail.id,
      notificationStatus: notificationStatus
    };
    return this.notificationService.GetNotificationByFilter(notificationFilterDTO).pipe(
      tap((data) => {
        this.issuanceDefaultNotification = data;
      })
    );
  }

  get getIsIssuanceFailed() {
    return this.isIssuanceFailed;
  }

}
