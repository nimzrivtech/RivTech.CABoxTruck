import { Injectable } from '@angular/core';
import { FormBuilder, FormControl, FormGroup } from '@angular/forms';
import { LimitsData } from '../../../modules/submission/data/limits/limits.data';
import { LimitsService } from '../../../modules/submission/services/coverage-limits.service';
import { TransactionSummaryDTO } from '../../../shared/models/policy/billing/transaction-summary.dto';
import { LimitsDTO } from '../../../shared/models/submission/limits/limitsDto';
import Utils from '../../../shared/utilities/utils';
import { Observable, of, Subject, BehaviorSubject } from 'rxjs';
import { catchError, finalize, map, switchMap, take, tap } from 'rxjs/operators';
import { AuthService } from '../../../core/services/auth.service';
import { RiskService } from '../../../core/services/risk/risk.service';
import { SubmissionData } from '../../../modules/submission/data/submission.data';
import { SelectItem } from '../../../shared/models/dynamic/select-item';
import { RiskDTO } from '../../../shared/models/risk/riskDto';
import { BillingService } from '@app/core/services/billing/billing.service';
import { PolicyHistoryDTO } from '@app/shared/models/policy/PolicyHistoryDto';
import { PolicyHistoryData } from './policy-history.data';
import { VehicleService } from '@app/modules/submission/services/vehicle.service';
import { VehicleDto } from '@app/shared/models/submission/Vehicle/VehicleDto';
import { PolicySummaryHeaderConstants } from '@app/shared/constants/policy-header.constants';
import { PolicyContactsService } from '@app/modules/submission/services/policy-contacts.service';
import { PolicyContactsDTO } from '@app/shared/models/submission/PolicyContactsDto';
import { NotificationDTO } from '@app/shared/models/notification/notification.dto';

@Injectable(
  { providedIn: 'root' }
)
export class PolicySummaryData {
  name: string = 'Business Name';
  userFullName: string = '';
  address: string = 'Business Address';
  policyStatus: string = 'Quoted';
  policySubStatus: string = 'Quoted';
  formType: string = '-';
  quoteNumber: string = '-';
  pendingCancellationDate: Date | undefined;
  pendingCancellationNoticeDate: Date | undefined;
  effectiveDate: string = '02/03/2021';
  agency: string = 'JE Brown Associates';
  producer: string = '';
  code: string = 'TX12345678';
  agencyStatus: string = 'Active';
  coverageA: number = 200000;
  creditOrderStatus: string = 'Ordered-Score Received';
  estimatedPremium: number = 52;
  taxes: number = 1;
  fees: number = 27;
  quoteStatus: string = '';
  riskDetailId: string;
  entityId: string;
  parentRiskDetailId: string;
  endorsementRiskDetailId: string;
  policyNumber: string;

  pageType: string;

  form: FormGroup;
  statusList: SelectItem[] = [];
  assignedToList: SelectItem[] = [];

  canEditPolicy: boolean = false;
  canEditActions: boolean = false;
  isPreviouslyEdited: boolean = false;
  public isPolicyLocked: boolean = false;
  public isPolicyFailed: boolean = false;
  public isEndorsementIssuancePending: boolean = false;
  // isPolicyLockedBS = new Subject<boolean>();
  // get isPolicyLocked$() { return this.isPolicyLockedBS.asObservable(); }
  public policyNotification: NotificationDTO;
  insuredForm: FormGroup;
  autoLiabilityCoverages = [];
  physicalDamageCoverages = [];
  glmtcCoverages = [];
  vehicles: VehicleDto[] = [];

  disableRewriteAction: boolean = false;
  lastIssuedEffectiveDate: Date;
  hideActionButtonIfRewrite: boolean = false;

  billingPremiumRetrieve: Subject<TransactionSummaryDTO> = new Subject<TransactionSummaryDTO>();

  powerUnitsCount: number;
  trailerCount: number;
  otherCount: number;

  shouldShowCommission: boolean = false;
  brokerCommisionAL: number;
  brokerCommisionPD: number;

  constructor(private fb: FormBuilder,
              private submissionData: SubmissionData,
              private riskService: RiskService,
              private authService: AuthService,
              private limitsData: LimitsData,
              private billingService: BillingService,
              private policyHistoryData: PolicyHistoryData,
              private vehicleService: VehicleService,
              private limitService: LimitsService,
              private policyContactService: PolicyContactsService) { }

  initiateFormFields() {
    this.insuredForm = this.insuredFormSection();
    this.coverageSummary();
  }

  insuredFormSection(): FormGroup {
    const nameAndAddressData = this.submissionData?.riskDetail?.nameAndAddress;
    const mailingAddress = `${nameAndAddressData?.businessAddress}, ${nameAndAddressData?.city}, ${nameAndAddressData?.state}, ${nameAndAddressData?.zipCode}`;
    const phoneNumber = nameAndAddressData?.phoneExt ?? '000000000';
    const emailAddress = nameAndAddressData?.email ?? 'N/A';
    return new FormGroup({
      mailingAddress: new FormControl(mailingAddress),
      mobilePhone: new FormControl(phoneNumber),
      email: new FormControl(emailAddress)
    });
  }

  renderCoveragesSummary(data: LimitsDTO) {
    const riskDetail = this.submissionData?.riskDetail;
    const bindOption = Number(riskDetail?.binding?.bindOptionId ?? 1);
    const riskCoverage = riskDetail?.riskCoverages.find(riskCov => riskCov?.optionNumber === bindOption);
    const manuscriptAL = riskDetail?.riskManuscripts?.filter(manuscript => manuscript?.premiumType === 'AL');
    const manuscriptPD = riskDetail?.riskManuscripts?.filter(manuscript => manuscript?.premiumType === 'PD');
    const manuscriptALPremium = manuscriptAL?.reduce((sum, current) => {
      let premium = 0;
      if (current.isProrate && current.proratedPremium !== 0) {
        premium = current.proratedPremium;
      } else if (!current.isProrate && current.deletedDate === null) {
        premium = current.premium;
      }
      return sum + premium;
    }, 0);

    const additionalPremium = + riskCoverage.riskCoveragePremium.additionalInsuredPremium
                              + riskCoverage.riskCoveragePremium.waiverOfSubrogationPremium
                              + riskCoverage.riskCoveragePremium.primaryNonContributoryPremium;
    const autoBILimitPremium = this.vehicles.reduce((sum, current) => sum + current.autoLiabilityPremium, 0);

    let coverageALSummary = [];
    let coveragePDSummary = [];
    let coverageGLMTCSummary = [];
    
    //#region AutoLiability
    const autoBILimit = {
      name: 'Bodily Injury and Property Damage Liability',
      symbol: this.limitsData.alSymbolList?.find(symbol => symbol.value == riskCoverage?.alSymbolId)?.label,
      limit: data?.autoBILimits?.find(limit => limit?.id == riskCoverage?.autoBILimitId)?.limitDisplay,
      premium: autoBILimitPremium + additionalPremium + manuscriptALPremium
    };
    coverageALSummary.push(autoBILimit);

    const umbiLimit = data?.umbiLimits?.find(limit => limit?.id == riskCoverage?.umbiLimitId);
    if (umbiLimit?.limitDisplay !== 'NONE' && riskCoverage?.umbiLimitId !== null) {
      coverageALSummary.push({
        name: 'Uninsured Motorist BI',
        symbol: this.limitsData.symbolList?.find(symbol => symbol.value == riskCoverage?.umbiLimitSymbolId)?.label,
        limit: umbiLimit?.limitDisplay,
        premium: 'Included in BI'
      });
    }

    const umpdLimit = data?.umpdLimits?.find(limit => limit?.id == riskCoverage?.umpdLimitId);
    if (umpdLimit?.limitDisplay !== 'NONE' && riskCoverage?.umpdLimitId !== null) {
      coverageALSummary.push({
        name: 'Uninsured Motorist PD',
        symbol: this.limitsData.symbolList?.find(symbol => symbol.value == riskCoverage?.umpdLimitSymbolId)?.label,
        limit: umpdLimit?.limitDisplay,
        premium: 'Included in BI'
      });
    }

    const uimbiLimit = data?.uimbiLimits?.find(limit => limit?.id == riskCoverage?.uimbiLimitId);
    if (uimbiLimit?.limitDisplay !== 'NONE' && riskCoverage?.uimbiLimitId !== null) {
      coverageALSummary.push({
        name: 'Underinsured Motorist BI',
        symbol: this.limitsData.symbolList?.find(symbol => symbol.value == riskCoverage?.uimbiLimitSymbolId)?.label,
        limit: uimbiLimit?.limitDisplay,
        premium: 'Included in BI'
      });
    }

    const uimpdLimit = data?.uimpdLimits?.find(limit => limit?.id == riskCoverage?.uimpdLimitId);
    if (uimpdLimit?.limitDisplay !== 'NONE' && riskCoverage?.uimpdLimitId !== null) {
      coverageALSummary.push({
        name: 'Underinsured Motorist PD',
        symbol: this.limitsData.symbolList?.find(symbol => symbol.value == riskCoverage?.uimpdLimitSymbolId)?.label,
        limit: uimpdLimit?.limitDisplay,
        premium: 'Included in BI'
      });
    }

    const medPayLimits = data?.medPayLimits?.find(limit => limit?.id == riskCoverage?.medPayLimitId);
    if (medPayLimits?.limitDisplay !== 'NONE' && riskCoverage?.medPayLimitId !== null) {
      coverageALSummary.push({
        name: 'Medical Payments',
        symbol: this.limitsData.symbolList?.find(symbol => symbol.value == riskCoverage?.medPayLimitSymbolId)?.label,
        limit: medPayLimits?.limitDisplay,
        premium: 'Included in BI'
      });
    }

    const pipLimit = data?.pipLimits?.find(limit => limit?.id == riskCoverage?.pipLimitId);
    if (pipLimit?.limitDisplay !== 'NONE' && riskCoverage?.pipLimitId !== null) {
      coverageALSummary.push({
        name: 'Personal Injury Projection\n(Or Equivalent No Fault Coverage)\nfor First Party Drivers & Passengers',
        symbol: this.limitsData.symbolList?.find(symbol => symbol.value == riskCoverage?.pipLimitSymbolId)?.label,
        limit: pipLimit?.limitDisplay,
        premium: 'Included in BI'
      });
    }

    const liabDeductible = data?.liabilityDeductibleLimits?.find(limit => limit?.id == riskCoverage?.liabilityDeductibleId);
    if (liabDeductible?.limitDisplay !== 'NONE' && riskCoverage?.liabilityDeductibleId !== null) {
      coverageALSummary.push({
        name: 'Liability Deductible',
        symbol: '',
        limit: liabDeductible?.limitDisplay,
        premium: 'Included in BI'
      });
    }

    if (this.limitsData.raterVersionSupport()) {
      const trailerInterchangeAL = data?.trailerInterchangeLimits?.find(limit => limit?.id == riskCoverage?.trailerInterchangeLimitId);
      const trailerInterchangePremiumAL = ((riskCoverage?.riskCoveragePremium?.proratedTrailerInterchangePremiumAL ?? 0) !== 0 || riskDetail.status === 'Canceled') ? 
        (riskCoverage?.riskCoveragePremium?.proratedTrailerInterchangePremiumAL ?? 0) : (riskCoverage?.riskCoveragePremium?.trailerInterchangeAL ?? 0);
      if ((trailerInterchangeAL?.limitDisplay !== 'NONE' && riskCoverage?.trailerInterchangeLimitId !== null) ||
          (trailerInterchangePremiumAL ?? 0 !== 0)) {
        coverageALSummary.push({
          name: 'Trailer Interchange',
          symbol: this.limitsData.symbolList?.find(symbol => symbol.value == riskCoverage?.trailerInterchangeLimitSymbolId)?.label,
          limit: trailerInterchangeAL?.limitDisplay,
          premium: trailerInterchangePremiumAL
        });
      }
  
      const nonOwnedTrailer = data?.nonOwnedTrailerLimits?.find(limit => limit?.id == riskCoverage?.nonOwnedTrailerLimitId);
      const nonOwnedTrailerPremium = ((riskCoverage?.riskCoveragePremium?.proratedNonOwnedTrailerPremium ?? 0) !== 0 || riskDetail.status === 'Canceled') ? 
      (riskCoverage?.riskCoveragePremium?.proratedNonOwnedTrailerPremium ?? 0) : (riskCoverage?.riskCoveragePremium?.nonOwnedTrailerPremium ?? 0);
      if ((nonOwnedTrailer?.limitDisplay !== 'NONE' && riskCoverage?.nonOwnedTrailerLimitId !== null) ||
          (nonOwnedTrailerPremium ?? 0 !== 0)) {
        coverageALSummary.push({
          name: 'Non-Owned Trailer',
          symbol: this.limitsData.symbolList?.find(symbol => symbol.value == riskCoverage?.nonOwnedTrailerLimitSymbolId)?.label,
          limit: nonOwnedTrailer?.limitDisplay,
          premium: nonOwnedTrailerPremium
        });
      }  
    }

    //#endregion

    //#region physical damage
    const compDeductible = data?.comprehensiveLimits?.find(limit => limit?.id == riskCoverage?.compDeductibleId);
    const compDeductiblePremium = 
      (riskCoverage?.compDeductibleId !== null && riskCoverage?.fireDeductibleId === null) ? 
        this.vehicles.reduce((sum, current) => sum + current.comprehensivePremium, 0) : 0;
    if ((compDeductible?.limitDisplay !== 'No Coverage' && riskCoverage?.compDeductibleId !== null) ||
        (compDeductiblePremium ?? 0) !== 0) {
      coveragePDSummary.push({
        name: 'Comprehensive',
        symbol: this.limitsData.symbolList?.find(symbol => symbol.value == riskCoverage?.compDeductibleSymbolId)?.label,
        deductible: riskCoverage?.compDeductibleId === 76 ? compDeductible?.limitDisplay : 'See Vehicles',
        premium: compDeductiblePremium
      });
    }

    const ftDeductible = data?.comprehensiveLimits?.find(limit => limit?.id == riskCoverage?.fireDeductibleId);
    const ftDeductiblePremium =
      (riskCoverage?.fireDeductibleId !== null && riskCoverage?.compDeductibleId === null) ?
        this.vehicles.reduce((sum, current) => sum + current.comprehensivePremium, 0) : 0;
    if ((ftDeductible?.limitDisplay !== 'No Coverage' && riskCoverage?.fireDeductibleId !== null) ||
        (ftDeductiblePremium ?? 0) !== 0) {
      coveragePDSummary.push({
        name: 'Fire / Theft',
        symbol: this.limitsData.symbolList?.find(symbol => symbol.value == riskDetail?.riskCoverages?.find(riskCov => riskCov?.optionNumber === 1)?.fireDeductibleSymbolId)?.label,
        deductible: riskCoverage?.fireDeductibleId === 76 ? ftDeductible?.limitDisplay : 'See Vehicles',
        premium: ftDeductiblePremium
      });
    }

    const collDeductible = data?.collisionLimits?.find(limit => limit?.id == riskCoverage?.collDeductibleId);
    const collDeductiblePremium = this.vehicles.reduce((sum, current) => sum + current.collisionPremium, 0);
    if ((collDeductible?.limitDisplay !== 'No Coverage' && riskCoverage?.collDeductibleId !== null) ||
        (collDeductiblePremium ?? 0) !== 0) {
      coveragePDSummary.push({
        name: 'Collision',
        symbol: this.limitsData.symbolList?.find(symbol => symbol.value == riskCoverage?.collDeductibleSymbolId)?.label,
        deductible: riskCoverage?.collDeductibleId === 65 ? collDeductible?.limitDisplay : 'See Vehicles',
        premium: collDeductiblePremium
      });
    }

    // from forms
    const riskForms = riskDetail?.riskForms;
    const hiredPhysicalDamage = riskForms?.find(form => form?.form?.id === 'HiredPhysicalDamage' && form?.isSelected === true);
    if (hiredPhysicalDamage) {
      const details = JSON.parse(hiredPhysicalDamage?.other);
      coveragePDSummary.push({
        name: 'Hired Physical Damage',
        symbol: '',
        limits: details?.physdamLimit ?? 0,
        deductible: details?.deductible ?? 0,
        premium: details?.estimatedAdditionalPremium ?? 0
      });
    }

    const trailerInterchange = riskForms?.find(form => form?.form?.id === 'TrailerInterchangeCoverage' && form?.isSelected === true);
    if (trailerInterchange) {
      const details = JSON.parse(trailerInterchange?.other);
      coveragePDSummary.push({
        name: 'Trailer Interchange Coverage',
        symbol: '',
        limits: (Number(details?.compreLimitOfInsurance ?? 0) +
                 Number(details?.lossLimitOfInsurance ?? 0) +
                 Number(details?.collisionLimitOfInsurance ?? 0) +
                 Number(details?.fireLimitOfInsurance ?? 0) +
                 Number(details?.fireTheftLimitOfInsurance ?? 0)),
        deductible: (Number(details?.compreDeductible ?? 0) +
                    Number(details?.lossDeductible ?? 0) +
                    Number(details?.collisionDeductible ?? 0) +
                    Number(details?.fireDeductible ?? 0) +
                    Number(details?.fireTheftDeductible ?? 0)),
        premium: (Number(details?.comprePremium ?? 0) +
                  Number(details?.lossPremium ?? 0) +
                  Number(details?.collisionPremium ?? 0) +
                  Number(details?.firePremium ?? 0) +
                  Number(details?.fireTheftPremium ?? 0))
      });
    }

    const manuscriptPDPremium = manuscriptPD?.reduce((sum, current) => {
      let premium = 0;
      if (current.isProrate && current.proratedPremium !== 0) {
        premium = current.proratedPremium;
      } else if (!current.isProrate && current.deletedDate === null) {
        premium = current.premium;
      }
      return sum + premium;
    }, 0);
    if (manuscriptPD.length > 0 && manuscriptPDPremium != 0) {
      coveragePDSummary.push({
        name: 'Manuscript',
        symbol: '',
        deductible: '',
        premium: manuscriptPDPremium
      });
    }
    //#endregion

    //#region gl & cargo
    const glLimit = data?.glLimits?.find(limit => limit?.id == riskCoverage?.glbiLimitId);
    const glLimitPremium = ((riskCoverage?.riskCoveragePremium?.proratedGeneralLiabilityPremium ?? 0) !== 0 || riskDetail.status === 'Canceled') ? 
      (riskCoverage?.riskCoveragePremium?.proratedGeneralLiabilityPremium ?? 0) : (riskCoverage?.riskCoveragePremium?.generalLiabilityPremium ?? 0);
    if ((glLimit?.limitDisplay !== 'NONE' && riskCoverage?.glbiLimitId !== null) ||
        (glLimitPremium ?? 0 !== 0)) {
      coverageGLMTCSummary.push({
        name: 'General Liability',
        limit: glLimit?.limitDisplay,
        premium: glLimitPremium
      });
    }

    const cargoLimit = data?.cargoLimits?.find(limit => limit?.id == riskCoverage?.cargoLimitId);
    const cargoPremium = this.vehicles.reduce((sum, current) => sum + current.cargoPremium, 0)
    if ((cargoLimit?.limitDisplay !== 'NONE' && riskCoverage?.cargoLimitId !== null) ||
        (cargoPremium ?? 0 !== 0)) {
      coverageGLMTCSummary.push({
        name: 'Motor Truck Cargo',
        limit: cargoLimit?.limitDisplay,
        premium: cargoPremium
      });
    }

    // const refLimits = data?.refLimits?.find(limit => limit?.id == riskCoverage?.refCargoLimitId);
    // if (refLimits?.limitDisplay !== 'NONE' && riskCoverage?.refCargoLimitId !== null) {
    //   const vehicles = riskDetail?.vehicles.filter(item => item.options?.split(',')?.includes(`${bindOption}`));
    //   const refrigerationPremium = vehicles.reduce((refPrem, current) => refPrem + current.refrigerationPremium, 0);
    //   coverageGLMTCSummary.push({
    //     name: 'Refrigeration',
    //     limit: refLimits?.limitDisplay,
    //     premium:  refrigerationPremium
    //   });
    // }
    //#endregion

    this.autoLiabilityCoverages = coverageALSummary;
    this.physicalDamageCoverages = coveragePDSummary;
    this.glmtcCoverages = coverageGLMTCSummary;
  }

  reset() {
    this.name = 'Business Name';
    this.address = 'Business Address';
    this.policyStatus = 'Quoted';
    this.formType = '-';
    this.quoteNumber = '';
    this.effectiveDate = '';
    this.agency = '';
    this.producer = '';
    this.code = 'TX12345678';
    this.agencyStatus = 'Active';
    this.coverageA = 200000;
    this.creditOrderStatus = 'Ordered-Score Received';
    this.estimatedPremium = null;
    this.taxes = null;
    this.fees = null;
    this.quoteStatus = '';
    this.powerUnitsCount = 0;
    this.trailerCount = 0;
    this.otherCount = 0;
    this.initialize();
    // this.setUserDetails();
  }

  initialize() {
    this.setEnableEditPolicy(false);
    this.canEditActions = true;
    this.form = this.fb.group({
      status: [null],
      assignedToId: [null],
    });

    // TODO: Temp as data sets will come from generic or user mgmt services
    this.statusList = [
      {value: 1, label: 'Endorsement'},
      {value: 2, label: 'Limits'},
      {value: 3, label: 'Vehicle Change'},
      {value: 4, label: 'Driver Change'},
      {value: 5, label: 'Interests'},
      {value: 5, label: 'Form Selection'},
    ];

    this.populateAssignedToList();
  }

  updateStatus(status: string, assignedToId: number) {
    const riskDetailId = this.submissionData.riskDetail?.id;
    if (riskDetailId) {
      const risk = new RiskDTO();
      risk.id = riskDetailId;
      risk.riskId = this.submissionData.riskDetail?.riskId;
      risk.status = status;
      risk.assignedToId = assignedToId;
      this.riskService.updateStatus(risk).subscribe(() => {
      });
    }
  }

  get isPendingEndorsement() {
    return this.policySubStatus === PolicySummaryHeaderConstants.policySubStatusPendingEndorsement;
  }

  get getIsPolicyLocked() {
    let isPolicyLocked = this.isPolicyLocked;
    return isPolicyLocked;
  }

  get getIsPolicyFailed() {
    let isPolicyFailed = this.isPolicyFailed;
    return isPolicyFailed;
  }

  get getIsEndorsementIssuancePending() {
    let isEndorsementIssuancePending = this.isEndorsementIssuancePending;
    return isEndorsementIssuancePending;
  }

  updateSubtatusForPendingEndorsement() {
    const riskDetailId = this.submissionData.riskDetail?.id;

    if(this.policyStatus == PolicySummaryHeaderConstants.policyStatusCancelled)
      return; // Cannot update substatus when policy is already canceled.

    if (riskDetailId) {
      this.riskService.updateEndorsementStatus(riskDetailId).subscribe((data) => {
        this.policySubStatus = data;
      });
    }
  }

  updateSubtatusForPendingEndorsement$() {
    const riskDetailId = this.submissionData.riskDetail?.id;

    if(this.policyStatus == PolicySummaryHeaderConstants.policyStatusCancelled || !riskDetailId){
      return of({}); // Cannot update substatus when policy is already canceled.
    }

    return this.riskService.updateEndorsementStatus(riskDetailId).pipe(
      tap((data) => this.policySubStatus = data)
    );
  }

  updatePendingEndorsement$() {
    const riskDetailId = this.submissionData.riskDetail?.id;
    if (riskDetailId) {
      return this.riskService.updateEndorsementStatus(riskDetailId).pipe(
        tap((data) => {
          this.policySubStatus = data;
        })
      )
    }
  }

  updatePendingEndorsementObservable() : Observable<any> {
    const riskDetailId = this.submissionData.riskDetail?.id;
    if (riskDetailId) {
      return this.riskService.updateEndorsementStatus(riskDetailId)
        .pipe(
          tap((data) => {
            this.policySubStatus = data;
          }),
        );
    }
    return of({});
  }

  updateEstimatedPremium(fromEndorsement: boolean = false) {
    const risk = this.submissionData.riskDetail;
    const bindingOption = parseInt(risk.binding.bindOptionId) - 1;
    const riskCoveragePremium = risk?.riskCoverages[bindingOption]?.riskCoveragePremium;
    if (fromEndorsement) {
      this.getBillingSummary();
    } else {
      this.estimatedPremium = riskCoveragePremium?.premium;
      this.taxes = (riskCoveragePremium?.taxesAndStampingFees ?? 0);
      this.fees = riskCoveragePremium?.riskMgrFeeAL + riskCoveragePremium?.riskMgrFeePD;
    }
  }

  updatePolicyStatus() {
    const riskDetail = this.submissionData.riskDetail;
    this.policyStatus = riskDetail.status;
    this.policySubStatus = riskDetail.subStatus;
    console.log(this.policySubStatus);
  }

  getBillingSummary() {
    this.billingService.getSummary(this.submissionData.riskDetail.riskId).subscribe(result => {
      if (result.paymentPlan && result.totalPremium) {
        this.estimatedPremium = result.premiumAmount;
        this.fees = result.billingSummaryDetails.find(x => x.description === 'Fee').written;
        this.taxes = result.billingSummaryDetails.find(x => x.description === 'Tax').written;
      }
    });
  }

  get userId(): number {
    const user = this.assignedToList.find(a => a.label.toLowerCase() === this.userFullName.toLowerCase());
    return user ? user.value : this.assignedToList.find(a => a.label === 'Jeff Russell')?.value;
  }

  private setUserDetails() {
    this.authService.getUserDetails().subscribe(u => {
      this.userFullName = `${u.firstName.trim()} ${u.lastName.trim()}`;
    });
  }

  coverageSummary() {
    this.vehicles = [];
    const riskDetail = this.submissionData.riskDetail;
    const state = riskDetail?.nameAndAddress?.state;
    this.vehicleService.getByRiskDetailId(riskDetail?.id, true).pipe(
      take(1),
      map((vehicleData) => this.vehicles = vehicleData as VehicleDto[]),
      switchMap(() => this.limitService.getLimits(state, this.submissionData.riskDetail.raterVersionId)),
      tap((limitsData) => this.renderCoveragesSummary(limitsData)),
      catchError(() => of()),
      finalize(() => {
        this.updateSubtatusForPendingEndorsement();
        Utils.unblockUI();
      })
    ).subscribe();
    // render coverage summary
    // Utils.blockUI();
    // this.limitService.getLimits(state)
    //   .pipe(
    //     take(1),
    //     finalize(() => ))
    //   .subscribe(data => );
  }

  coverageSummary$() {
    this.vehicles = [];
    const riskDetail = this.submissionData.riskDetail;
    const state = riskDetail?.nameAndAddress?.state;
    return this.vehicleService.getByRiskDetailId(riskDetail?.id, true).pipe(
      take(1),
      map((vehicleData) => this.vehicles = vehicleData as VehicleDto[]),
      switchMap(() => this.limitService.getLimits(state, this.submissionData.riskDetail.raterVersionId)),
      tap((limitsData) => this.renderCoveragesSummary(limitsData)),
      switchMap(() => this.updatePendingEndorsement$()),
      catchError((err) => { 
        console.log(err);
        return of(err);
      }),
    );
  }

  resetChanges() {
    this.setEnableEditPolicy(!this.canEditPolicy);
  }

  get isPendingCancellationPolicy() {
    const newPendingCancellationNoticeDate = new Date(this.pendingCancellationNoticeDate);
    return this.pendingCancellationNoticeDate !== null;
  }

  toggleEditPolicy() {
    this.setEnableEditPolicy(!this.canEditPolicy);
  }

  reloadActionButtonsValidation() {
    this.policyHistoryData.getEndorsementHistory().subscribe((data: PolicyHistoryDTO[]) => {
      const hasEndorsements = data.filter(history => Number(history.endorsementNumber) > 0 && history.policyStatus === 'Active')?.length > 0;
      const hasRewrite = data.filter(history => history.policyStatus === 'Rewrite').length > 0;
      this.disableRewriteAction = hasEndorsements;
      this.hideActionButtonIfRewrite = hasRewrite;
      this.lastIssuedEffectiveDate = data.sort((a, b) => b.dateCreated > a.dateCreated ? -1 : 1)[data.length - 1]?.effectiveDate; // includes cancellation and reinstatement
    });
  }

  populateAssignedToList() {
    this.policyContactService.getAllAssignedToPolicyContacts().subscribe(data => {
      this.assignedToList = this.convertEnumerableListToSelectItemList(data);
    });
  }

  private convertEnumerableListToSelectItemList(list: PolicyContactsDTO[]): SelectItem[] {
    let convertedList: SelectItem[] = [];

    if(list){
        for (const item of list) {
            convertedList.push({
                label: item.name,
                value: item.id
            });
        }
    }

    return convertedList;
  }

  setVehicleTypeHeader() {
    this.submissionData?.getVehicleTypeCount().subscribe((vehCount) => {
      // Access the properties of the returned object
      this.powerUnitsCount = vehCount.CountPowerUnit;
      this.trailerCount = vehCount.CountTrailer;
      this.otherCount = vehCount.CountOthers;
      
    });
  }

  setEnableEditPolicy(enabled: boolean) {
    this.canEditPolicy = enabled;
    this.isPreviouslyEdited = this.canEditPolicy;
  }

  // updateIsPolicyLocked$() {
  //   return this.isPolicyLocked$.first().pipe(take(1)).subscribe((status) => this.isPolicyLocked = status);
  // }


  async checkForCommission(from: string | null) {
    try {
      let coverage = await this.submissionData.getCoverageCommision(from);

      this.shouldShowCommission = coverage.shouldShowCommission;
      this.brokerCommisionAL = coverage.brokerCommisionAL;
      this.brokerCommisionPD = coverage.brokerCommisionPD;

      console.log("policy checkForCommission", this.shouldShowCommission, typeof this.brokerCommisionAL,  typeof this.brokerCommisionPD);
    } catch (error) {
      console.error("Error in checkForCommission:", error);
      // Handle the error as appropriate
    }
  }

  
}
