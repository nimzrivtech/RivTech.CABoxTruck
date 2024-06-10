import { Injectable } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { LimitsService } from '../../services/coverage-limits.service';
import { catchError, take, tap, switchMap  } from 'rxjs/operators';
import Utils from '../../../../shared/utilities/utils';
import { RiskCoverageDTO, SaveRiskCoverageDTO } from '../../../../shared/models/submission/limits/riskCoverageDto';
import { SubmissionData } from '../submission.data';
import { ToastrService } from 'ngx-toastr';
import { SummaryData } from '../summary.data';
import { LimitsDefaults, LimitsDefaultValues } from '../../../../shared/constants/limits-defaults.constants';
import FormUtils from '../../../../shared/utilities/form.utils';
import { LayoutService } from '../../../../core/services/layout/layout.service';
import { RiskSpecificsData } from '../risk-specifics.data';
import { createSubmissionDetailsMenuItems } from '../../components/submission-details/submission-details-navitems';
import { SelectItem } from '../../../../shared/models/dynamic/select-item';
import { ErrorMessageConstant } from '../../../../shared/constants/error-message.constants';
import { Observable, of } from 'rxjs';
import { FormsService } from '../../services/forms.service';

export interface LimitsForm {
  limitsPageForm?: FormGroup;
}

@Injectable(
  { providedIn: 'root' }
)
export class LimitsData {
  // General
  readonly RATER_VERSION_SUPPORT = 7;

  isGeneralLiability: boolean = false;
  isCargo: boolean = false;
  isRefrigeration: boolean = false;
  isCollision: boolean = false;
  isComprehensive: boolean = false;
  isTrailerType: boolean = false;

  isUmBiAvailable: boolean;
  isUmPdAvailable: boolean;
  isUimBiAvailable: boolean;
  isUimPdAvailable: boolean;
  isPipAvailable: boolean;

  // Hide UM/UIM PD
  // isDisablePD: boolean = true;
  showPD: boolean = false;

  // Limits dropdown lookup
  biLimitList: any[];
  pdLimitList: any[];
  liabDeductibleList: any[];
  umBiLimitList: any[];
  umPdLimitList: any[];
  uimBiLimitList: any[];
  uimPdLimitList: any[];
  medpayList: any[];
  pipList: any[];
  CauseOfLossList: any[];
  ComprehensiveList: any[];
  CollisionList: any[];
  glList: any[];
  cargoList: any[];
  refBreakList: any[];
  trailerInterchangeList: any[];
  nonOwnedTrailerList: any[];
  trailerTypeList: any[];
  pollutionCovList: any[];
  disabledList: any[] = [];

  bComp: any;
  bColl: any;
  bCargo: any;
  bRef: any;

  symbolList: any[] = [
    { label: '7, 8 & 9', value: '1' },
    { label: '7 & 8', value: '13' },
    { label: '7 & 9', value: '2' },
    { label: '2, 8 & 9', value: '3' },
    { label: '1) Any Auto', value: '4' },
    { label: '2) All Owned Autos', value: '5' },
    { label: '3) Owned Private Passenger Autos', value: '6' },
    { label: '4) Owned Autos Other Than Private Passenger', value: '7' },
    { label: '5) All Owned Autos Which Require No-Fault Coverage', value: '8' },
    { label: '6) Owned Autos Subject to Compulsory UM Law', value: '9' },
    { label: '7) Autos Specified on  Schedule', value: '10' },
    { label: '8) Hired Autos', value: '11' },
    { label: '9) Non-Owned Autos', value: '12' },
  ];

  get alSymbolList(): any[] {
    var symbolList = this.symbolList;
  
    if(this.raterVersionSupport()) {
      symbolList = symbolList.concat([
        { label: '10 - Use 2, 8 & 9 Pricing', value: '14' },
        { label: '10 - Use 7, 8 & 9 Pricing', value: '15' }
      ]);
    }
  
    return symbolList;
  };

  hasRiskCoverage: boolean = true;

  isVehicle: boolean = false;

  // All forms
  limitsForm: LimitsForm;

  oldLimitsData: RiskCoverageDTO = null; // reference to previous limit page data. 

  disableUnInsuredMotoristBI = false;
  disableUnInsuredMotoristPD = false;
  disableUnderinsuredMotoristBI = false;
  disableUnderinsuredMotoristPD = false;
  disableMedicalPayments = false;
  disablePersonalInjuryProtection = false;
  disableCompDeductible = false;
  disableCollDeductible = false;
  disableCargo = false;
  disableRefCargo = false;
  disableTrailerInterchange = false;
  disableNonOwnedTrailer = false;

  constructor(
    private limitService: LimitsService,
    private formBuilder: FormBuilder,
    public summaryData: SummaryData,
    public submissionData: SubmissionData,
    private toastr: ToastrService,
    private layoutService: LayoutService,
    private riskSpecificsData: RiskSpecificsData,
    private formsService: FormsService
  ) { }

  get coverage() {
    return this.submissionData.getCoverage(this.isVehicle);
  }

  get hasAutoPhysicalDamage() {
    return this.coverage?.compDeductibleId > LimitsDefaultValues.compId
      || this.coverage?.fireDeductibleId > LimitsDefaultValues.compId
      || this.coverage?.collDeductibleId > LimitsDefaultValues.colId;
  }

  get hasGeneralLiability() {
    return this.coverage?.glbiLimitId != undefined &&
            this.coverage?.glbiLimitId != LimitsDefaultValues.glId &&
            this.coverage?.glbiLimitId != LimitsDefaultValues.glv7Id;
  }

  get hasComprehensive() {
    return this.coverage?.compDeductibleId > LimitsDefaultValues.compId || this.coverage?.fireDeductibleId > LimitsDefaultValues.compId;
  }

  get hasCollision() {
    return this.coverage?.collDeductibleId > LimitsDefaultValues.colId;
  }

  get hasCargo() {
    return this.coverage?.cargoLimitId > LimitsDefaultValues.cargoId;
  }

  get hasRefrigeration() {
    return this.coverage?.refCargoLimitId && this.coverage.refCargoLimitId !== LimitsDefaultValues.refId;
  }

  get hasTrailerInterchange() {
    return this.coverage?.trailerInterchangeLimitId && this.coverage.trailerInterchangeLimitId !== LimitsDefaultValues.triId;
  }

  get hasUninsuredMotoristBI() {
    return this.hasUninsuredMotoristSingle || this.hasUninsuredMotoristSplit;
  }

  get hasUninsuredMotoristSingle() {
    return this.coverage?.umbiLimitId && LimitsDefaultValues.umBISingleLimitIds.some(limit => limit == this.coverage?.umbiLimitId) && 
        this.coverage?.umbiLimitId !== LimitsDefaultValues.umBILimitId;
  }

  get hasUninsuredMotoristSplit() {
    return this.coverage?.umbiLimitId && LimitsDefaultValues.umBISplitLimitIds.some(limit => limit == this.coverage?.umbiLimitId) && 
        this.coverage?.umbiLimitId !== LimitsDefaultValues.umBILimitId;
  }

  get hasTXPIPLimits() {
    return this.coverage?.pipLimitId && LimitsDefaultValues.txpipLimitIds.some(limit => limit == this.coverage?.pipLimitId);
  }

  get hasTXUMUIMLimits() {
    return (
      this.coverage?.uimbiLimitId || 
      this.coverage?.umbiLimitId || 
      this.coverage?.umpdLimitId
      ) 
      && 
      (
        (this.coverage?.uimbiLimitId >= 278 && this.coverage?.uimbiLimitId <= 286) ||
        (this.coverage?.umbiLimitId >= 434 && this.coverage?.umbiLimitId <= 442) ||
        (this.coverage?.umpdLimitId >= 532 && this.coverage?.umpdLimitId <= 538)
      );
  }

  get hasTXSplitUMUIMLimits() {
    return (
      this.coverage?.uimbiLimitId || 
      this.coverage?.umbiLimitId
      ) 
      && 
      (
        LimitsDefaultValues.txSplitumuimLimitIds.some(limit => limit == this.coverage?.umbiLimitId) ||
        LimitsDefaultValues.txSplitumuimLimitIds.some(limit => limit == this.coverage?.uimbiLimitId)
      );
  }

  get hasNJPIPLimits() {
    return this.coverage?.pipLimitId && (this.coverage?.pipLimitId >= 119 && this.coverage?.pipLimitId <= 129);
  }

  get hasNJUMUIMLimits() {
    return (
      this.coverage?.uimbiLimitId || 
      this.coverage?.uimpdLimitId || 
      this.coverage?.umbiLimitId || 
      this.coverage?.umpdLimitId
      ) 
      && 
      (
        (this.coverage?.uimbiLimitId >= 231 && this.coverage?.uimbiLimitId <= 242) ||
        (this.coverage?.uimpdLimitId >= 288 && this.coverage?.uimpdLimitId <= 297) ||
        (this.coverage?.umbiLimitId >= 376 && this.coverage?.umbiLimitId <= 387) ||
        (this.coverage?.umpdLimitId >= 499 && this.coverage?.umpdLimitId <= 508)
      );
  }

  get hasNJSplitUMUIMLimits() {
    return (
      this.coverage?.uimbiLimitId || 
      this.coverage?.umbiLimitId
      ) 
      && 
      (
        LimitsDefaultValues.njSplitumLimitIds.some(limit => limit == this.coverage?.umbiLimitId) ||
        LimitsDefaultValues.njSplituimLimitIds.some(limit => limit == this.coverage?.uimbiLimitId)
      );
  }

  get hasMOUMLimits() {
    return (
      this.coverage?.umbiLimitId
      ) 
      && 
      (
        (this.coverage?.umbiLimitId >= 365 && this.coverage?.umbiLimitId <= 374)
      );
  }

  get hasMOUIMLimits() {
    return (
      this.coverage?.uimbiLimitId
      ) 
      && 
      (
        (this.coverage?.uimbiLimitId >= 221 && this.coverage?.uimbiLimitId <= 229)
      );
  }

  get hasGAUMLimits() {
    return (
      this.coverage?.umbiLimitId || 
      this.coverage?.umpdLimitId
      ) 
      && 
      (
        (this.coverage?.umbiLimitId >= 334 && this.coverage?.umbiLimitId <= 343) ||
        (this.coverage?.umpdLimitId >= 449 && this.coverage?.umpdLimitId <= 472)
      );
  }

  get hasPASplitUMLimits() {
    return ( 
      this.coverage?.umbiLimitId
      ) 
      && 
      (
        LimitsDefaultValues.paSplitumLimitIds.some(limit => limit == this.coverage?.umbiLimitId)
      );
  }

  get hasPASplitUIMLimits() {
    return ( 
      this.coverage?.uimbiLimitId
      ) 
      && 
      (
        LimitsDefaultValues.paSplituimLimitIds.some(limit => limit == this.coverage?.uimbiLimitId)
      );
  }

  get hasPAUMLimits() {
    return ( 
      this.coverage?.umbiLimitId
      ) 
      && 
      (
        (this.coverage?.umbiLimitId >= 410 && this.coverage?.umbiLimitId <= 421)
      );
  }

  get hasPAUIMLimits() {
    return ( 
      this.coverage?.uimbiLimitId
      ) 
      && 
      (
        (this.coverage?.uimbiLimitId >= 265 && this.coverage?.uimbiLimitId <= 276)
      );
  }

  get hasFLUMLimits() {
    return ( 
      this.coverage?.umbiLimitId
      ) 
      && 
      (
        (this.coverage?.umbiLimitId >= 552 && this.coverage?.umbiLimitId <= 566)
      );
  }

  get hasORUMLimits() {
    return ( 
      this.coverage?.umbiLimitId ||
      this.coverage?.umpdLimitId
      ) 
      && 
      (
        (this.coverage?.umbiLimitId >= 400 && this.coverage?.umbiLimitId <= 408) ||
        (this.coverage?.umpdLimitId == 520)
      );
  }

  get hasOHUMLimits() {
    return ( 
      this.coverage?.umbiLimitId ||
      this.coverage?.umpdLimitId ||
      this.coverage?.uimbiLimitId
      ) 
      && 
      (
        (this.coverage?.umbiLimitId >= 389 && this.coverage?.umbiLimitId <= 398) ||
        (this.coverage?.umpdLimitId >= 510 && this.coverage?.umpdLimitId <= 519) ||
        (this.coverage?.uimbiLimitId >= 244 && this.coverage?.uimbiLimitId <= 253)
      );
  }

  get hasCOUMLimits() {
    return ( 
      this.coverage?.umbiLimitId ||
      this.coverage?.umpdLimitId
      ) 
      && 
      (
        (this.coverage?.umbiLimitId >= 324 && this.coverage?.umbiLimitId <= 332) ||
        (this.coverage?.umbiLimitId >= 632 && this.coverage?.umbiLimitId <= 633) ||
        (this.coverage?.umpdLimitId >= 446 && this.coverage?.umpdLimitId <= 447)
      );
  }

  get hasCOMPLimits() {
    return this.coverage?.medPayLimitId >= 111 && this.coverage.medPayLimitId <= 116;
  }

  get hasINUMLimits() {
    return ( 
      this.coverage?.umbiLimitId ||
      this.coverage?.umpdLimitId
      ) 
      && 
      (
        (this.coverage?.umbiLimitId >= 345 && this.coverage?.umbiLimitId <= 353) ||
        (this.coverage?.umbiLimitId >= 647 && this.coverage?.umbiLimitId <= 648) ||
        (this.coverage?.umpdLimitId >= 474 && this.coverage?.umpdLimitId <= 487) ||
        (this.coverage?.umpdLimitId >= 677 && this.coverage?.umpdLimitId <= 680)
      );
  }

  get hasINUIMLimits() {
    return ( 
      this.coverage?.umbiLimitId ||
      this.coverage?.umpdLimitId
      ) 
      && 
      (
        (this.coverage?.uimbiLimitId >= 202 && this.coverage?.uimbiLimitId <= 209) ||
        (this.coverage?.uimbiLimitId >= 744 && this.coverage?.uimbiLimitId <= 745)
      );
  }

  get hasVASplitUMLimits() {
    return ( 
      this.coverage?.umbiLimitId
      ) 
      && 
      (
        LimitsDefaultValues.vaSplitumLimitIds.some(limit => limit == this.coverage?.umbiLimitId)
      );
  }

  get hasVAUMLimits() {
    return ( 
      this.coverage?.umbiLimitId ||
      this.coverage?.uimpdLimitId
      ) 
      && 
      (
        (this.coverage?.umbiLimitId >= 665 && this.coverage?.uimbiLimitId <= 676) ||
        (this.coverage?.umpdLimitId >= 540 && this.coverage?.umpdLimitId <= 550) ||
        (this.coverage?.umpdLimitId >= 699 && this.coverage?.umpdLimitId <= 699)
      );
  }

  get hasTNUMLimits() {
    return ( 
      this.coverage?.umbiLimitId ||
      this.coverage?.umpdLimitId
      ) 
      && 
      (
        (this.coverage?.umbiLimitId >= 423 && this.coverage?.umbiLimitId <= 432) ||
        (this.coverage?.umbiLimitId >= 662 && this.coverage?.umbiLimitId <= 663) ||
        (this.coverage?.umpdLimitId >= 522 && this.coverage?.umpdLimitId <= 530) ||
        (this.coverage?.umpdLimitId >= 697 && this.coverage?.umpdLimitId <= 698)
      );
  }

  get hasILUMLimits() {
    return ( 
      this.coverage?.umbiLimitId ||
      this.coverage?.umpdLimitId
      ) 
      && 
      (
        (this.coverage?.umbiLimitId >= 635 && this.coverage?.umbiLimitId <= 646) ||
        (this.coverage?.umpdLimitId >= 681 && this.coverage?.umpdLimitId <= 681)
      );
  }

  get hasILUIMLimits() {
    return ( 
      this.coverage?.uimbiLimitId
      ) 
      && 
      (
        (this.coverage?.uimbiLimitId >= 732 && this.coverage?.uimbiLimitId <= 743)
      );
  }

  get hasAZUIMLimits() {
    return ( 
      this.coverage?.uimbiLimitId
      ) 
      && 
      (
        (this.coverage?.uimbiLimitId >= 700 && this.coverage?.uimbiLimitId <= 728)
      );
  }

  //TODO: Update when specs is updated
  get hasAZUMLimits() {
    return ( 
      this.coverage?.umbiLimitId
      ) 
      && 
      (
        (this.coverage?.umbiLimitId >= 603 && this.coverage?.umbiLimitId <= 631)
      );
  }

  get hasMSUMLimits() {
    return ( 
      this.coverage?.umbiLimitId ||
      this.coverage?.umpdLimitId
      ) 
      && 
      (
        (this.coverage?.umbiLimitId >= 650 && this.coverage?.umbiLimitId <= 661) ||
        (this.coverage?.umpdLimitId >= 684 && this.coverage?.umpdLimitId <= 693)
      );
  }

  get hasSCSplitUMLimits() {
    return ( 
      this.coverage?.umbiLimitId
      ) 
      && 
      (
        LimitsDefaultValues.scSplitumLimitIds.some(limit => limit == this.coverage?.umbiLimitId)
      );
  }

  get hasSCSplitUIMLimits() {
    return ( 
      this.coverage?.uimbiLimitId
      ) 
      && 
      (
        LimitsDefaultValues.scSplituimLimitIds.some(limit => limit == this.coverage?.uimbiLimitId)
      );
  }

  get hasSCUMLimits() {
    return ( 
      this.coverage?.umbiLimitId,
      this.coverage?.umpdLimitId
      ) 
      && 
      (
        (this.coverage?.umbiLimitId >= 794 && this.coverage?.umbiLimitId <= 821) ||
        (this.coverage?.umpdLimitId >= 694 && this.coverage?.umpdLimitId <= 696)
      );
  }

  get hasSCUIMLimits() {
    return ( 
      this.coverage?.uimbiLimitId,
      this.coverage?.uimpdLimitId
      ) 
      && 
      (
        (this.coverage?.uimbiLimitId >= 760 && this.coverage?.uimbiLimitId <= 787) ||
        (this.coverage?.uimpdLimitId >= 788 && this.coverage?.uimpdLimitId <= 790)
      );
  }

  get hasSymbol10() {
    return (this.coverage?.alSymbolId && this.coverage?.alSymbolId == 14 ||
      this.coverage?.alSymbolId && this.coverage?.alSymbolId == 15);
  }

  get hasPOC() {
    return ( 
        this.coverage?.pollutionCovLimitId
      ) 
      && 
      (
        this.coverage?.pollutionCovLimitId >= 880 && this.coverage?.pollutionCovLimitId <= 881
      );
  }

  get cityState() {
    return this.submissionData?.riskDetail?.nameAndAddress?.state;
  }

  initiateFormFields() {
    // this.isDisablePD = true;
    this.Reset();
    this.limitsForm = {};
    this.limitsForm.limitsPageForm = this.formBuilder.group({
      riskDetailId: [null],
      autoBILimitId: [null, [Validators.required]],
      autoPDLimitId: [null],
      umbiLimitId: [null],
      umpdLimitId: [null],
      uimbiLimitId: [null],
      uimpdLimitId: [null],
      medPayLimitId: [null, [Validators.required]],
      pipLimitId: [null],
      liabilityDeductibleId: [null, [Validators.required]],
      compDeductibleId: [null, [Validators.required]],
      collDeductibleId: [null, [Validators.required]],
      glbiLimitId: [null, [Validators.required]],
      cargoLimitId: [null, [Validators.required]],
      refCargoLimitId: [null, [Validators.required]],
      trailerInterchangeLimitId: [null, [Validators.required]],
      nonOwnedTrailerLimitId: [null, [Validators.required]],
      // trailerTypeLimitId: [null],
      pollutionCovLimitId: [null, [Validators.required]],

      comprehensiveFire: [true],

      alSymbolId: [null, [Validators.required]],
      pdSymbolId: [null], // Validators.required
      umbiLimitSymbolId: [null], // Validators.required
      umpdLimitSymbolId: [null], // Validators.required
      uimbiLimitSymbolId: [null], // Validators.required
      uimpdLimitSymbolId: [null], // Validators.required
      medPayLimitSymbolId: [null], // Validators.required
      pipLimitSymbolId: [null], // Validators.required
      liabilityDeductibleSymbolId: [null], // Validators.required
      compDeductibleSymbolId: [null], // Validators.required
      collDeductibleSymbolId: [null], // Validators.required
      glbiLimitSymbolId: [null],
      cargoLimitSymbolId: [null], // Validators.required
      refCargoLimitSymbolId: [null], // Validators.required
      trailerInterchangeLimitSymbolId: [null], // Validators.required
      nonOwnedTrailerLimitSymbolId: [null], // Validators.required

      alPremiumInputSymbol: [null],
      pdPremiumInputSymbol: [null],
      glPremiumInputSymbol: [null],
      cargoPremiumInputSymbol: [null],
    });
  }

  populatePolicyLimitsFields() {
    this.Reset();
    // Initial submission load patch
    if (this.submissionData?.riskDetail?.riskCoverage == null && this.submissionData?.riskDetail?.riskCoverages != null && this.submissionData?.riskDetail?.riskCoverages?.length > 0) {
        const bindOptionNumber = Number(this.submissionData?.riskDetail.binding?.bindOptionId ?? 1);
        const option1 = this.submissionData.riskDetail.riskCoverages.find(x => x.optionNumber === bindOptionNumber);
        this.submissionData.riskDetail.riskCoverage = option1;
    }
    if (this.submissionData?.riskDetail?.riskCoverage) {
      this.limitsForm.limitsPageForm.patchValue(this.submissionData?.riskDetail?.riskCoverage);
      if (this.submissionData?.riskDetail?.riskCoverage?.compDeductibleId == null && this.submissionData?.riskDetail?.riskCoverage?.fireDeductibleId) {
        this.limitsForm.limitsPageForm.controls['comprehensiveFire'].patchValue(false);
        this.limitsForm.limitsPageForm.controls['compDeductibleId'].patchValue(this.submissionData.riskDetail.riskCoverage.fireDeductibleId);
        this.limitsForm.limitsPageForm.controls['compDeductibleSymbolId'].patchValue(this.submissionData.riskDetail.riskCoverage.fireDeductibleSymbolId); // always use option 1
      }

      if (this.submissionData?.riskDetail?.riskCoverage?.compDeductibleId && this.submissionData?.riskDetail?.riskCoverage?.fireDeductibleId == null && this.submissionData.riskDetail.riskCoverage.cargoLimitId !== 1) { this.isComprehensive = true; }
      if (this.submissionData?.riskDetail?.riskCoverage?.cargoLimitId && this.submissionData?.riskDetail?.riskCoverage?.cargoLimitId !== 1) { this.isCargo = true; }
      if (this.submissionData?.riskDetail?.riskCoverage?.glbiLimitId && this.submissionData?.riskDetail?.riskCoverage?.glbiLimitId !== 1) { this.isGeneralLiability = true; }
      if (this.submissionData?.riskDetail?.riskCoverage?.refCargoLimitId && this.submissionData?.riskDetail?.riskCoverage?.refCargoLimitId !== 1) { this.isRefrigeration = true; }
      if (this.submissionData?.riskDetail?.riskCoverage?.collDeductibleId && this.submissionData?.riskDetail?.riskCoverage?.collDeductibleId !== 1) { this.isCollision = true; }

      this.isTrailerType = (this.submissionData?.riskDetail?.riskCoverage?.trailerInterchangeLimitId && this.submissionData?.riskDetail?.riskCoverage?.trailerInterchangeLimitId !== LimitsDefaultValues.triId) ||
                           (this.submissionData?.riskDetail?.riskCoverage?.nonOwnedTrailerLimitId && this.submissionData?.riskDetail?.riskCoverage?.nonOwnedTrailerLimitId !== LimitsDefaultValues.noId)
      this.bComp = this.submissionData?.riskDetail?.riskCoverage.compDeductibleId ?? this.submissionData?.riskDetail?.riskCoverage?.fireDeductibleId;
      this.bColl = this.submissionData?.riskDetail?.riskCoverage.collDeductibleId;
      this.bCargo = this.submissionData?.riskDetail?.riskCoverage.cargoLimitId;
      this.bRef = this.submissionData?.riskDetail?.riskCoverage.refCargoLimitId;
      // console.log('backup', this.bComp, this.bColl, this.bCargo, this.bRef );
    }
  }

  populateLimitsFields() {
    this.Reset();
    // Initial submission load patch
    if (this.submissionData?.riskDetail?.riskCoverage == null && this.submissionData?.riskDetail?.riskCoverages != null && this.submissionData?.riskDetail?.riskCoverages?.length > 0) {
      const option1 = this.submissionData.riskDetail.riskCoverages.find(x => x.optionNumber === 1);
      this.submissionData.riskDetail.riskCoverage = option1;
    }
    if (this.submissionData?.riskDetail?.riskCoverage) {
      this.limitsForm.limitsPageForm.patchValue(this.submissionData?.riskDetail?.riskCoverage);
      if (this.submissionData?.riskDetail?.riskCoverage?.compDeductibleId == null && this.submissionData?.riskDetail?.riskCoverage?.fireDeductibleId) {
        this.limitsForm.limitsPageForm.controls['comprehensiveFire'].patchValue(false);
        this.limitsForm.limitsPageForm.controls['compDeductibleId'].patchValue(this.submissionData.riskDetail.riskCoverage.fireDeductibleId);
        this.limitsForm.limitsPageForm.controls['compDeductibleSymbolId'].patchValue(this.submissionData.riskDetail.riskCoverage.fireDeductibleSymbolId);
      }

      if (this.submissionData?.riskDetail?.riskCoverage?.compDeductibleId && this.submissionData?.riskDetail?.riskCoverage?.fireDeductibleId == null && this.submissionData.riskDetail.riskCoverage.cargoLimitId !== 1) { this.isComprehensive = true; }
      if (this.submissionData?.riskDetail?.riskCoverage?.cargoLimitId && this.submissionData?.riskDetail?.riskCoverage?.cargoLimitId !== 1) { this.isCargo = true; }
      if (this.submissionData?.riskDetail?.riskCoverage?.glbiLimitId && this.submissionData?.riskDetail?.riskCoverage?.glbiLimitId !== 1) { this.isGeneralLiability = true; }
      if (this.submissionData?.riskDetail?.riskCoverage?.refCargoLimitId && this.submissionData?.riskDetail?.riskCoverage?.refCargoLimitId !== 1) { this.isRefrigeration = true; }
      if (this.submissionData?.riskDetail?.riskCoverage?.collDeductibleId && this.submissionData?.riskDetail?.riskCoverage?.collDeductibleId !== 1) { this.isCollision = true; }

      this.isTrailerType = (this.submissionData?.riskDetail?.riskCoverage?.trailerInterchangeLimitId && this.submissionData?.riskDetail?.riskCoverage?.trailerInterchangeLimitId !== LimitsDefaultValues.triId) ||
                           (this.submissionData?.riskDetail?.riskCoverage?.nonOwnedTrailerLimitId && this.submissionData?.riskDetail?.riskCoverage?.nonOwnedTrailerLimitId !== LimitsDefaultValues.noId)
      this.bComp = this.submissionData?.riskDetail?.riskCoverage.compDeductibleId ?? this.submissionData?.riskDetail?.riskCoverage?.fireDeductibleId;
      this.bColl = this.submissionData?.riskDetail?.riskCoverage.collDeductibleId;
      this.bCargo = this.submissionData?.riskDetail?.riskCoverage.cargoLimitId;
      this.bRef = this.submissionData?.riskDetail?.riskCoverage.refCargoLimitId;
      // console.log('backup', this.bComp, this.bColl, this.bCargo, this.bRef );
    }

  }

  setDefaults() {
    const state = this.cityState;
    const limitDefault = LimitsDefaults.find(x => x.state === state);
    if (limitDefault != null) {
      this.limitsForm.limitsPageForm.controls['umbiLimitId'].patchValue(limitDefault.umbiLimitId);
      this.limitsForm.limitsPageForm.controls['umpdLimitId'].patchValue(limitDefault.umpdLimitId);
      this.limitsForm.limitsPageForm.controls['uimbiLimitId'].patchValue(limitDefault.uimbiLimitId);
      this.limitsForm.limitsPageForm.controls['uimpdLimitId'].patchValue(limitDefault.uimpdLimitId);
      this.limitsForm.limitsPageForm.controls['pipLimitId'].patchValue(limitDefault.pipLimitId);

      if (this.submissionData.riskDetail?.riskCoverage != null) {
        this.submissionData.riskDetail.riskCoverage.umbiLimitId = limitDefault.umbiLimitId;
        this.submissionData.riskDetail.riskCoverage.umpdLimitId = limitDefault.umpdLimitId;
        this.submissionData.riskDetail.riskCoverage.uimbiLimitId = limitDefault.uimbiLimitId;
        this.submissionData.riskDetail.riskCoverage.uimpdLimitId = limitDefault.uimpdLimitId;
        this.submissionData.riskDetail.riskCoverage.pipLimitId = limitDefault.pipLimitId;
      }

      if (this.submissionData.riskDetail.riskCoverages !== null && this.submissionData.riskDetail.riskCoverages.length > 0) {
        this.submissionData.riskDetail.riskCoverages.forEach(item => {
          item.umbiLimitId = limitDefault.umbiLimitId;
          item.umpdLimitId = limitDefault.umpdLimitId;
          item.uimbiLimitId = limitDefault.uimbiLimitId;
          item.uimpdLimitId = limitDefault.uimpdLimitId;
          item.pipLimitId = limitDefault.pipLimitId;
        });
      }
    }

    // Set autobi and medpay ALL to 23 and 1
    this.limitsForm.limitsPageForm.controls['autoBILimitId'].patchValue(LimitsDefaultValues.autoBILimitId);
    this.limitsForm.limitsPageForm.controls['autoPDLimitId'].patchValue(LimitsDefaultValues.autoPDSingleLimitId);
    this.limitsForm.limitsPageForm.controls['medPayLimitId'].patchValue(LimitsDefaultValues.medPayLimitId);
    this.limitsForm.limitsPageForm.controls['liabilityDeductibleId'].patchValue(LimitsDefaultValues.liabilityLimitId);

    if (this.submissionData.riskDetail?.riskCoverage != null) {
      this.submissionData.riskDetail.riskCoverage.autoBILimitId = LimitsDefaultValues.autoBILimitId;
      this.submissionData.riskDetail.riskCoverage.autoPDLimitId = LimitsDefaultValues.autoPDSingleLimitId;
      this.submissionData.riskDetail.riskCoverage.medPayLimitId = LimitsDefaultValues.medPayLimitId;
    }

    if (this.submissionData.riskDetail.riskCoverages !== null && this.submissionData.riskDetail.riskCoverages.length > 0) {
      this.submissionData.riskDetail.riskCoverages.forEach(item => {
        item.autoBILimitId = LimitsDefaultValues.autoBILimitId;
        item.autoPDLimitId = LimitsDefaultValues.autoPDSingleLimitId;
        item.medPayLimitId = LimitsDefaultValues.medPayLimitId;
      });
    }

    this.limitsForm.limitsPageForm.controls['compDeductibleId'].patchValue(LimitsDefaultValues.compId);
    this.limitsForm.limitsPageForm.controls['collDeductibleId'].patchValue(LimitsDefaultValues.colId);

    this.limitsForm.limitsPageForm.controls['glbiLimitId'].patchValue(LimitsDefaultValues.glId);
    this.limitsForm.limitsPageForm.controls['cargoLimitId'].patchValue(LimitsDefaultValues.cargoId);
    this.limitsForm.limitsPageForm.controls['refCargoLimitId'].patchValue(LimitsDefaultValues.refId);

    this.limitsForm.limitsPageForm.controls['alSymbolId'].patchValue(LimitsDefaultValues.defaultSymbol);
    this.limitsForm.limitsPageForm.controls['pdSymbolId'].patchValue(LimitsDefaultValues.defaultSymbol);
    this.limitsForm.limitsPageForm.controls['umbiLimitSymbolId'].patchValue(LimitsDefaultValues.defaultSymbol);
    this.limitsForm.limitsPageForm.controls['umpdLimitSymbolId'].patchValue(LimitsDefaultValues.defaultSymbol);
    this.limitsForm.limitsPageForm.controls['uimbiLimitSymbolId'].patchValue(LimitsDefaultValues.defaultSymbol);
    this.limitsForm.limitsPageForm.controls['uimpdLimitSymbolId'].patchValue(LimitsDefaultValues.defaultSymbol);
    this.limitsForm.limitsPageForm.controls['medPayLimitSymbolId'].patchValue(LimitsDefaultValues.defaultSymbol);
    this.limitsForm.limitsPageForm.controls['pipLimitSymbolId'].patchValue(LimitsDefaultValues.defaultSymbol);

    this.limitsForm.limitsPageForm.controls['compDeductibleSymbolId'].patchValue(LimitsDefaultValues.defaultSymbol);
    this.limitsForm.limitsPageForm.controls['collDeductibleSymbolId'].patchValue(LimitsDefaultValues.defaultSymbol);
    this.limitsForm.limitsPageForm.controls['cargoLimitSymbolId'].patchValue(LimitsDefaultValues.defaultSymbol);
    this.limitsForm.limitsPageForm.controls['refCargoLimitSymbolId'].patchValue(LimitsDefaultValues.defaultSymbol);

    if (this.raterVersionSupport()) {
      this.limitsForm.limitsPageForm.controls['trailerInterchangeLimitId'].patchValue(LimitsDefaultValues.triId);
      this.limitsForm.limitsPageForm.controls['nonOwnedTrailerLimitId'].patchValue(LimitsDefaultValues.noId);
      // this.limitsForm.limitsPageForm.controls['trailerTypeLimitId'].patchValue(LimitsDefaultValues.trtId);
      this.limitsForm.limitsPageForm.controls['pollutionCovLimitId'].patchValue(LimitsDefaultValues.pocId);
      this.limitsForm.limitsPageForm.controls['trailerInterchangeLimitSymbolId'].patchValue(LimitsDefaultValues.defaultSymbol);
      this.limitsForm.limitsPageForm.controls['nonOwnedTrailerLimitSymbolId'].patchValue(LimitsDefaultValues.defaultSymbol);  
    }

    if (this.raterVersionSupport(7)) {
      this.limitsForm.limitsPageForm.controls['glbiLimitId'].patchValue(LimitsDefaultValues.glv7Id);  
    }

    if (this.raterVersionSupport(8)) {
      this.limitsForm.limitsPageForm.controls['liabilityDeductibleId'].patchValue(LimitsDefaultValues.liabilityLimitv8Id);  
    }
  }

  retrieveDropDownValues(isInitial: boolean = false) {
    this.disabledList = [];
    this.umBiLimitList = [];
    this.umPdLimitList = [];
    this.uimBiLimitList = [];
    this.uimPdLimitList = [];
    this.pipList = [];

    this.isUmBiAvailable = false;
    this.isUmPdAvailable = false;
    this.isUimBiAvailable = false;
    this.isUimPdAvailable = false;
    this.isPipAvailable = false;
    const state = this.cityState;
    // Autoliability/Property
    //Utils.blockUI();
    this.limitService.getLimits(state, this.submissionData.riskDetail.raterVersionId).pipe(take(1)).subscribe(data => {
      if (data.umbiLimits && data.umbiLimits.length > 0) {
        this.umBiLimitList = this.customConvertEnumerableListToSelectItemList(data.umbiLimits);
        this.isUmBiAvailable = true;
      }

      if (data.umpdLimits && data.umpdLimits.length > 0) {
        this.umPdLimitList = this.customConvertEnumerableListToSelectItemList(data.umpdLimits);
        this.isUmPdAvailable = true;
      }

      if (data.uimbiLimits && data.uimbiLimits.length > 0) {
        this.uimBiLimitList = this.customConvertEnumerableListToSelectItemList(data.uimbiLimits);
        this.isUimBiAvailable = true;
      }

      if (data.uimpdLimits && data.uimpdLimits.length > 0) {
        this.uimPdLimitList = this.customConvertEnumerableListToSelectItemList(data.uimpdLimits);
        this.isUimPdAvailable = true;
      }

      if (data.pipLimits && data.pipLimits.length > 0) {
        this.pipList = (['NJ'].includes(state)) ? 
          this.customConvertEnumerableListToSelectItemList(data.pipLimits.filter(x => x.id == 124)) : 
          this.customConvertEnumerableListToSelectItemList(data.pipLimits);
        this.isPipAvailable = true;
      }

      if (data.medPayLimits && data.medPayLimits.length > 0) {
        if (['FL','OR','VA'].includes(state)) {
          this.disabledList.push('medPayLimits');
        } else {
          this.medpayList = this.customConvertEnumerableListToSelectItemList(data.medPayLimits);
        }
      }

      this.biLimitList = this.customConvertEnumerableListToSelectItemList(data.autoBILimits);
      this.pdLimitList = this.customConvertEnumerableListToSelectItemList(data.autoPDLimits);
      this.liabDeductibleList = this.customConvertEnumerableListToSelectItemList(data.liabilityDeductibleLimits);

      this.ComprehensiveList = this.customConvertEnumerableListToSelectItemList(data.comprehensiveLimits);
      this.CollisionList = this.customConvertEnumerableListToSelectItemList(data.collisionLimits);

      this.glList = this.sortGlList(this.customConvertEnumerableListToSelectItemList(data.glLimits));
      this.cargoList = this.customConvertEnumerableListToSelectItemList(data.cargoLimits);
      this.refBreakList = this.customConvertEnumerableListToSelectItemList(data.refLimits);

      if (this.raterVersionSupport()) {
        this.trailerInterchangeList = this.customConvertEnumerableListToSelectItemList(data.trailerInterchangeLimits);
        this.nonOwnedTrailerList = this.customConvertEnumerableListToSelectItemList(data.nonOwnedTrailerLimits);
        this.trailerTypeList = this.customConvertEnumerableListToSelectItemList(data.trailerTypeLimits);
        this.pollutionCovList = this.customConvertEnumerableListToSelectItemList(data.pollutionCovLimits);  
      }

      this.setRequiredFields();
      this.checkAndRequireCompAndCollSymbols();

      // Manual Update of menu badge because of this async http method
      if (!isInitial) {
        this.submissionData.validationList.limits = this.checkLimitsSection();
        this.layoutService.updateMenu(createSubmissionDetailsMenuItems(this.submissionData.riskDetail.id, this.submissionData.validationList,
          this.riskSpecificsData.hiddenNavItems));
      }

      this.checkAndSetPdDisplayAndValue();
      //this.checkAndSetCargoAndReeferSymbolValue(!this.submissionData.isIssued);
      //Utils.unblockUI();
      this.disableSymbolAll();
    });
  }

  public disabledField(field: string) {
    return this.disabledList?.some(x => x == field);
  }

  checkAndSetPdDisplayAndValue(isChange: boolean = false) {
    const alValue = this.limitsForm.limitsPageForm.controls['autoBILimitId'].value;
    const alSymbolValue = this.limitsForm.limitsPageForm.controls['alSymbolId'].value;
    if (alValue == null || alValue === '') {
      this.showPD = false;
      this.resetField('autoPDLimitId');
      this.resetField('pdSymbolId');
      this.limitsForm.limitsPageForm.controls['autoPDLimitId'].patchValue(null);
      this.limitsForm.limitsPageForm.controls['pdSymbolId'].patchValue(null);
      return;
    }

    const alLabel = this.biLimitList.find(x => x.value == alValue)?.label;

    if (alLabel) {
      if (alLabel.toString().indexOf('/') !== -1) {
        this.showPD = true;
        this.setAsRequired('autoPDLimitId');
        this.setAsRequired('pdSymbolId');
        if (isChange) {
          this.limitsForm.limitsPageForm.controls['autoPDLimitId'].patchValue(LimitsDefaultValues.autoPDSplitLimitId);
          this.limitsForm.limitsPageForm.controls['pdSymbolId'].patchValue(LimitsDefaultValues.defaultSymbol);
        }
      } else {
        this.showPD = false;
        this.resetField('autoPDLimitId');
        this.resetField('pdSymbolId');
        const pdValue = this.pdLimitList.find(x => x.label == alLabel)?.value;
        this.limitsForm.limitsPageForm.controls['autoPDLimitId'].patchValue(pdValue);
        this.limitsForm.limitsPageForm.controls['pdSymbolId'].patchValue(alSymbolValue);
      }
    }
  }

  checkAndSetCargoAndReeferSymbolValue(isChange: boolean = false) {
    if(isChange) {
      const alSymbolValue = this.limitsForm.limitsPageForm.controls['alSymbolId'].value;
      const cargoLimitSymbolId = this.limitsForm.limitsPageForm.controls['cargoLimitId'].value;
      const refCargoLimitSymbolId = this.limitsForm.limitsPageForm.controls['refCargoLimitId'].value;
  
      if(cargoLimitSymbolId != LimitsDefaultValues.cargoId)
        this.limitsForm.limitsPageForm.controls['cargoLimitSymbolId'].patchValue(alSymbolValue);
      else
        this.limitsForm.limitsPageForm.controls['cargoLimitSymbolId'].patchValue('');
  
      if(refCargoLimitSymbolId != LimitsDefaultValues.refId)
        this.limitsForm.limitsPageForm.controls['refCargoLimitSymbolId'].patchValue(alSymbolValue);
      else
        this.limitsForm.limitsPageForm.controls['refCargoLimitSymbolId'].patchValue('');
    }
  }

  checkAndRequireCompAndCollSymbols() {
    const compLimit = this.limitsForm.limitsPageForm.controls['compDeductibleId'].value;
    const collLimit = this.limitsForm.limitsPageForm.controls['collDeductibleId'].value;
    const hasComprehensiveInput = (compLimit && !(compLimit == 76 || compLimit == '76'));
    const hasCollisionInput = (collLimit && !(collLimit == 65 || compLimit == '65'));

    if (this.raterVersionSupport()) {
      if (hasComprehensiveInput) { 
        this.setAsRequired('compDeductibleSymbolId'); 
      } else { 
        this.resetField('compDeductibleSymbolId');
      }
  
      if (hasCollisionInput && !hasComprehensiveInput) { 
        this.setAsRequired('collDeductibleSymbolId'); 
      } else if (hasCollisionInput && hasComprehensiveInput){ 
        const collSymbol = this.limitsForm.limitsPageForm.controls['collDeductibleSymbolId'].value;
        this.resetField('collDeductibleSymbolId');
        this.limitsForm.limitsPageForm.controls['collDeductibleSymbolId'].patchValue(collSymbol);
      } else {
        this.resetField('collDeductibleSymbolId');
      }
    }
  }

  // Remove coma in value
  customConvertEnumerableListToSelectItemList(list: any[]): SelectItem[] {
    const convertedList: SelectItem[] = [];
    const noneList: SelectItem[] = [];
    if (list) {
        for (const item of list) {
          if (item.limitDisplay != 'NONE') {
            convertedList.push({
              label: item.limitDisplay,
              value: item.id
            });
          } else {
            noneList.push({
              label: item.limitDisplay,
              value: item.id
            });
          }
        }
    }

    return convertedList.concat(noneList);
  }

  setRequiredFields() {
    if (this.isUmBiAvailable) { this.setAsRequired('umbiLimitId'); } else { this.resetField('umbiLimitId'); }
    if (this.isUmPdAvailable) { this.setAsRequired('umpdLimitId'); } else { this.resetField('umpdLimitId'); }
    if (this.isUimBiAvailable) { this.setAsRequired('uimbiLimitId'); } else { this.resetField('uimbiLimitId'); }
    if (this.isUimPdAvailable) { this.setAsRequired('uimpdLimitId'); } else { this.resetField('uimpdLimitId'); }
    if (this.isPipAvailable) { this.setAsRequired('pipLimitId'); } else { this.resetField('pipLimitId'); }
    if (this.raterVersionSupport()) { this.setAsRequired('trailerInterchangeLimitId'); } else { this.resetField('trailerInterchangeLimitId'); }
    if (this.raterVersionSupport()) { this.setAsRequired('nonOwnedTrailerLimitId'); } else { this.resetField('nonOwnedTrailerLimitId'); }
    // if (this.raterVersionSupport()) { this.setAsRequired('trailerTypeLimitId'); } else { this.resetField('trailerTypeLimitId'); }
    if (this.raterVersionSupport()) { this.setAsRequired('pollutionCovLimitId'); } else { this.resetField('pollutionCovLimitId'); }
  }

  Reset() {
    this.isGeneralLiability = false;
    this.isCargo = false;
    this.isRefrigeration = false;
    this.isCollision = false;
    this.isComprehensive = false;
    this.showPD = false;
    this.isTrailerType = false;
  }

  get f() { return this.limitsForm.limitsPageForm.controls; }

  public resetField(fieldName: string) {
    this.f[fieldName].reset();
    this.f[fieldName].clearValidators();
    this.f[fieldName].updateValueAndValidity();
  }

  private setAsRequired(fieldName: string) {
    this.f[fieldName].setValidators(Validators.required);
    this.f[fieldName].updateValueAndValidity();
  }

  // Services
  saveRiskCoverageLimit(callbacAfterSave?: Function) : Observable<any> {
    const option = (this.submissionData.isIssued) ? this.submissionData.riskDetail?.binding?.bindOptionId ?? 1 : 1;
    this.isTrailerType = ((this.limitsForm.limitsPageForm.value.trailerInterchangeLimitId && this.limitsForm.limitsPageForm.value.trailerInterchangeLimitId !== LimitsDefaultValues.triId &&
                           this.limitsForm.limitsPageForm.value.trailerInterchangeLimitId !== '822') || 
                          (this.limitsForm.limitsPageForm.value.nonOwnedTrailerLimitId && this.limitsForm.limitsPageForm.value.nonOwnedTrailerLimitId !== LimitsDefaultValues.noId &&
                            this.limitsForm.limitsPageForm.value.nonOwnedTrailerLimitId !== '839'));
    const coverageLimit = new SaveRiskCoverageDTO({
      riskDetailId: this.submissionData.riskDetail.id,
      autoBILimitId: this.limitsForm.limitsPageForm.value.autoBILimitId ? parseInt(this.limitsForm.limitsPageForm.value.autoBILimitId) : null,
      autoPDLimitId: this.limitsForm.limitsPageForm.value.autoPDLimitId ? parseInt(this.limitsForm.limitsPageForm.value.autoPDLimitId) : null,
      umbiLimitId: this.limitsForm.limitsPageForm.value.umbiLimitId ? parseInt(this.limitsForm.limitsPageForm.value.umbiLimitId) : null,
      umpdLimitId: this.limitsForm.limitsPageForm.value.umpdLimitId ? parseInt(this.limitsForm.limitsPageForm.value.umpdLimitId) : null,
      uimbiLimitId: this.limitsForm.limitsPageForm.value.uimbiLimitId ? parseInt(this.limitsForm.limitsPageForm.value.uimbiLimitId) : null,
      uimpdLimitId: this.limitsForm.limitsPageForm.value.uimpdLimitId ? parseInt(this.limitsForm.limitsPageForm.value.uimpdLimitId) : null,
      medPayLimitId: this.limitsForm.limitsPageForm.value.medPayLimitId ? parseInt(this.limitsForm.limitsPageForm.value.medPayLimitId) : null,
      pipLimitId: this.limitsForm.limitsPageForm.value.pipLimitId ? parseInt(this.limitsForm.limitsPageForm.value.pipLimitId) : null,
      liabilityDeductibleId: this.limitsForm.limitsPageForm.value.liabilityDeductibleId ? parseInt(this.limitsForm.limitsPageForm.value.liabilityDeductibleId) : null,
      collDeductibleId: this.limitsForm.limitsPageForm.value.collDeductibleId ? parseInt(this.limitsForm.limitsPageForm.value.collDeductibleId) : null,
      glbiLimitId: this.limitsForm.limitsPageForm.value.glbiLimitId ? parseInt(this.limitsForm.limitsPageForm.value.glbiLimitId) : null,
      cargoLimitId: this.limitsForm.limitsPageForm.value.cargoLimitId ? parseInt(this.limitsForm.limitsPageForm.value.cargoLimitId) : null,
      refCargoLimitId: this.limitsForm.limitsPageForm.value.refCargoLimitId ? parseInt(this.limitsForm.limitsPageForm.value.refCargoLimitId) : null,
      compDeductibleId: this.limitsForm.limitsPageForm.value.compDeductibleId && this.limitsForm.limitsPageForm.value.comprehensiveFire ? parseInt(this.limitsForm.limitsPageForm.value.compDeductibleId) : null,
      fireDeductibleId: this.limitsForm.limitsPageForm.value.compDeductibleId && !this.limitsForm.limitsPageForm.value.comprehensiveFire ? parseInt(this.limitsForm.limitsPageForm.value.compDeductibleId) : null,
      trailerInterchangeLimitId: this.limitsForm.limitsPageForm.value.trailerInterchangeLimitId ? parseInt(this.limitsForm.limitsPageForm.value.trailerInterchangeLimitId) : null,
      nonOwnedTrailerLimitId: this.limitsForm.limitsPageForm.value.nonOwnedTrailerLimitId ? parseInt(this.limitsForm.limitsPageForm.value.nonOwnedTrailerLimitId) : null,
      trailerTypeLimitId: this.isTrailerType ? (this.submissionData.riskDetail.riskCoverage?.trailerTypeLimitId || this.submissionData.riskDetail.riskCoverage?.trailerTypeLimitId == 0) ? this.submissionData.riskDetail.riskCoverage?.trailerTypeLimitId : 859  : null,
      pollutionCovLimitId: this.limitsForm.limitsPageForm.value.pollutionCovLimitId ? parseInt(this.limitsForm.limitsPageForm.value.pollutionCovLimitId) : null,

      alSymbolId: this.limitsForm.limitsPageForm.value.alSymbolId ? parseInt(this.limitsForm.limitsPageForm.value.alSymbolId) : null,
      pdSymbolId: this.limitsForm.limitsPageForm.value.pdSymbolId ? parseInt(this.limitsForm.limitsPageForm.value.pdSymbolId) : null,
      liabilityDeductibleSymbolId: this.limitsForm.limitsPageForm.value.liabilityDeductibleSymbolId ? parseInt(this.limitsForm.limitsPageForm.value.liabilityDeductibleSymbolId) : null,
      medPayLimitSymbolId: this.limitsForm.limitsPageForm.value.medPayLimitSymbolId ? parseInt(this.limitsForm.limitsPageForm.value.medPayLimitSymbolId) : null,
      pipLimitSymbolId: this.limitsForm.limitsPageForm.value.pipLimitSymbolId ? parseInt(this.limitsForm.limitsPageForm.value.pipLimitSymbolId) : null,
      umbiLimitSymbolId: this.limitsForm.limitsPageForm.value.umbiLimitSymbolId ? parseInt(this.limitsForm.limitsPageForm.value.umbiLimitSymbolId) : null,
      umpdLimitSymbolId: this.limitsForm.limitsPageForm.value.umpdLimitSymbolId ? parseInt(this.limitsForm.limitsPageForm.value.umpdLimitSymbolId) : null,
      uimbiLimitSymbolId: this.limitsForm.limitsPageForm.value.uimbiLimitSymbolId ? parseInt(this.limitsForm.limitsPageForm.value.uimbiLimitSymbolId) : null,
      uimpdLimitSymbolId: this.limitsForm.limitsPageForm.value.uimpdLimitSymbolId ? parseInt(this.limitsForm.limitsPageForm.value.uimpdLimitSymbolId) : null,
      collDeductibleSymbolId: this.limitsForm.limitsPageForm.value.collDeductibleSymbolId &&
        this.limitsForm.limitsPageForm.value.collDeductibleId != LimitsDefaultValues.colId ? parseInt(this.limitsForm.limitsPageForm.value.collDeductibleSymbolId) : null,
      glbiLimitSymbolId: this.limitsForm.limitsPageForm.value.glbiLimitSymbolId ? parseInt(this.limitsForm.limitsPageForm.value.glbiLimitSymbolId) : null,
      cargoLimitSymbolId: this.limitsForm.limitsPageForm.value.cargoLimitSymbolId 
        && this.limitsForm.limitsPageForm.value.cargoLimitSymbolId != LimitsDefaultValues.cargoId ? parseInt(this.limitsForm.limitsPageForm.value.cargoLimitSymbolId) : null,
      refCargoLimitSymbolId: this.limitsForm.limitsPageForm.value.refCargoLimitSymbolId 
        && this.limitsForm.limitsPageForm.value.refCargoLimitSymbolId != LimitsDefaultValues.refId ? parseInt(this.limitsForm.limitsPageForm.value.refCargoLimitSymbolId) : null,
      trailerInterchangeLimitSymbolId: this.limitsForm.limitsPageForm.value.trailerInterchangeLimitSymbolId ? parseInt(this.limitsForm.limitsPageForm.value.trailerInterchangeLimitSymbolId) : null,
      nonOwnedTrailerLimitSymbolId: this.limitsForm.limitsPageForm.value.nonOwnedTrailerLimitSymbolId ? parseInt(this.limitsForm.limitsPageForm.value.nonOwnedTrailerLimitSymbolId) : null,

      compDeductibleSymbolId: this.limitsForm.limitsPageForm.value.compDeductibleSymbolId && this.limitsForm.limitsPageForm.value.comprehensiveFire &&
        this.limitsForm.limitsPageForm.value.compDeductibleId != LimitsDefaultValues.compId ? parseInt(this.limitsForm.limitsPageForm.value.compDeductibleSymbolId) : null,
      fireDeductibleSymbolId: this.limitsForm.limitsPageForm.value.compDeductibleSymbolId && !this.limitsForm.limitsPageForm.value.comprehensiveFire &&
        this.limitsForm.limitsPageForm.value.compDeductibleId != LimitsDefaultValues.compId ? parseInt(this.limitsForm.limitsPageForm.value.compDeductibleSymbolId) : null,
      optionNumber: Number(option)
    });
    this.submissionData.riskDetail.riskCoverage = coverageLimit;
    console.log(this.submissionData.riskDetail.riskCoverage);

    // Set Quote Options 1
    if (this.submissionData.riskDetail.riskCoverages != null && this.submissionData.riskDetail.riskCoverages?.length > 0) {
      const option1 = this.submissionData.riskDetail.riskCoverages.find(x => x.optionNumber === Number(option));
      const index = this.submissionData.riskDetail.riskCoverages.indexOf(option1);
      this.submissionData.riskDetail.riskCoverages[index].autoBILimitId = coverageLimit.autoBILimitId;
      this.submissionData.riskDetail.riskCoverages[index].autoPDLimitId = coverageLimit.autoPDLimitId;
      this.submissionData.riskDetail.riskCoverages[index].umbiLimitId = coverageLimit.umbiLimitId;
      this.submissionData.riskDetail.riskCoverages[index].umpdLimitId = coverageLimit.umpdLimitId;
      this.submissionData.riskDetail.riskCoverages[index].uimbiLimitId = coverageLimit.uimbiLimitId;
      this.submissionData.riskDetail.riskCoverages[index].uimpdLimitId = coverageLimit.uimpdLimitId;
      this.submissionData.riskDetail.riskCoverages[index].medPayLimitId = coverageLimit.medPayLimitId;
      this.submissionData.riskDetail.riskCoverages[index].pipLimitId = coverageLimit.pipLimitId;
      this.submissionData.riskDetail.riskCoverages[index].liabilityDeductibleId = coverageLimit.liabilityDeductibleId;
      this.submissionData.riskDetail.riskCoverages[index].collDeductibleId = coverageLimit.collDeductibleId;
      this.submissionData.riskDetail.riskCoverages[index].glbiLimitId = coverageLimit.glbiLimitId;
      this.submissionData.riskDetail.riskCoverages[index].trailerInterchangeLimitId = coverageLimit.trailerInterchangeLimitId;
      this.submissionData.riskDetail.riskCoverages[index].nonOwnedTrailerLimitId = coverageLimit.nonOwnedTrailerLimitId;
      this.submissionData.riskDetail.riskCoverages[index].pollutionCovLimitId = coverageLimit.pollutionCovLimitId;
      this.submissionData.riskDetail.riskCoverages[index].trailerTypeLimitId = coverageLimit.trailerTypeLimitId;
      this.submissionData.riskDetail.riskCoverages[index].cargoLimitId = coverageLimit.cargoLimitId;
      this.submissionData.riskDetail.riskCoverages[index].refCargoLimitId = coverageLimit.refCargoLimitId;
      this.submissionData.riskDetail.riskCoverages[index].compDeductibleId = coverageLimit.compDeductibleId;
      this.submissionData.riskDetail.riskCoverages[index].fireDeductibleId = coverageLimit.fireDeductibleId;
      this.submissionData.riskDetail.riskCoverages[index].alSymbolId = coverageLimit.alSymbolId;
      this.submissionData.riskDetail.riskCoverages[index].pdSymbolId = coverageLimit.pdSymbolId;
      this.submissionData.riskDetail.riskCoverages[index].trailerInterchangeLimitSymbolId = coverageLimit.trailerInterchangeLimitSymbolId;
      this.submissionData.riskDetail.riskCoverages[index].nonOwnedTrailerLimitSymbolId = coverageLimit.nonOwnedTrailerLimitSymbolId;
    }

    this.updateUninsuredMotoristBIForms();
    this.updateRefCargoForms();
    this.updateGLForms();
    this.updateTXFormsForms();
    /*
    Utils.blockUI();
    this.limitService.saveRiskCoverageLimit(coverageLimit).pipe(take(1)).subscribe(data => {
      Utils.unblockUI();
      this.submissionData.riskDetail.riskCoverage = data;
      if (callbacAfterSave) { callbacAfterSave(); }
      console.log('Limits Saved!');
    }, (error) => {
      if (callbacAfterSave) { callbacAfterSave(); }
      Utils.unblockUI();
      this.toastr.error(ErrorMessageConstant.savingErrorMsg);
    });
    */

    return this.limitService.saveRiskCoverageLimit(coverageLimit).pipe(
      take(1),
      tap((data) => {
        this.submissionData.riskDetail.riskCoverage = data;
        console.log('Limits Saved!');
      }),
      catchError((err) => {
        this.toastr.error(ErrorMessageConstant.savingErrorMsg);
        return of(err);
      }),
      switchMap(() => {
        return this.formsService.get(this.submissionData.riskDetail?.id).pipe(
          tap(data => {
            this.submissionData.riskDetail.riskForms = data;
          }));
      })
    ); 
  }

  updateUninsuredMotoristBIForms() {
    if (this.submissionData.riskDetail.riskForms?.length > 0) {
      const form = this.submissionData.riskDetail.riskForms.find(x => x.form?.id === 'UninsuredMotoristCoverageBodilyInjury');
      if (form != null) {
        const index = this.submissionData.riskDetail.riskForms.indexOf(form);
        this.submissionData.riskDetail.riskForms[index].isSelected = false;
        if (this.hasUninsuredMotoristBI) {
          this.submissionData.riskDetail.riskForms[index].isSelected = true;
        }
      }
      const splitform = this.submissionData.riskDetail.riskForms.find(x => x.form?.id === 'SplitBodilyInjuryUninsuredMotoristCoverage');
      if (splitform != null) {
        const index = this.submissionData.riskDetail.riskForms.indexOf(splitform);
        this.submissionData.riskDetail.riskForms[index].isSelected = false;
        if (this.hasUninsuredMotoristBI) {
          this.submissionData.riskDetail.riskForms[index].isSelected = true;
        }
      }
    }
  }

  updateTXFormsForms() {
    if (this.submissionData.riskDetail.riskForms?.length > 0) {
      const form = this.submissionData.riskDetail.riskForms.find(x => x.form?.id === 'TexasPersonalInjuryProtection');
      if (form != null) {
        const index = this.submissionData.riskDetail.riskForms.indexOf(form);
        this.submissionData.riskDetail.riskForms[index].isSelected = false;
        if (this.hasTXPIPLimits) {
          this.submissionData.riskDetail.riskForms[index].isSelected = true;
        }
      }
      const splitform = this.submissionData.riskDetail.riskForms.find(x => x.form?.id === 'TexasUMandUIMCoverage');
      if (splitform != null) {
        const index = this.submissionData.riskDetail.riskForms.indexOf(splitform);
        this.submissionData.riskDetail.riskForms[index].isSelected = false;
        if (this.hasTXUMUIMLimits) {
          this.submissionData.riskDetail.riskForms[index].isSelected = true;
        }
      }
    }
  }

  updateRefCargoForms() {
    if (this.submissionData.riskDetail.riskForms?.length > 0) {
      const form = this.submissionData.riskDetail.riskForms.find(x => x.form?.id === 'RefrigerationCargoEndorsement');
      if (form != null) {
        const index = this.submissionData.riskDetail.riskForms.indexOf(form);
        this.submissionData.riskDetail.riskForms[index].isSelected = false;
        if (this.hasRefrigeration) {
          this.submissionData.riskDetail.riskForms[index].isSelected = true;
        }
      }
    }
  }

  updateGLForms() {
    if (this.submissionData.riskDetail.riskForms?.length > 0) {
      const form = this.submissionData.riskDetail.riskForms.find(x => x.form?.id === 'GeneralLiabilityEndorsement');
      if (form != null) {
        const index = this.submissionData.riskDetail.riskForms.indexOf(form);
        this.submissionData.riskDetail.riskForms[index].isSelected = false;
        if (this.hasGeneralLiability) {
          this.submissionData.riskDetail.riskForms[index].isSelected = true;
        }
      }
    }
  }

  setDefaultRiskCoverageLimit() {
    const coverageLimit = new SaveRiskCoverageDTO({
      riskDetailId: this.submissionData.riskDetail.id,
      autoBILimitId: this.limitsForm.limitsPageForm.value.autoBILimitId ? parseInt(this.limitsForm.limitsPageForm.value.autoBILimitId) : null,
      autoPDLimitId: this.limitsForm.limitsPageForm.value.autoPDLimitId ? parseInt(this.limitsForm.limitsPageForm.value.autoPDLimitId) : null,
      pdSymbolId: this.limitsForm.limitsPageForm.value.pdSymbolId ? parseInt(this.limitsForm.limitsPageForm.value.pdSymbolId) : null,
      umbiLimitId: this.limitsForm.limitsPageForm.value.umbiLimitId ? parseInt(this.limitsForm.limitsPageForm.value.umbiLimitId) : null,
      umpdLimitId: this.limitsForm.limitsPageForm.value.umpdLimitId ? parseInt(this.limitsForm.limitsPageForm.value.umpdLimitId) : null,
      uimbiLimitId: this.limitsForm.limitsPageForm.value.uimbiLimitId ? parseInt(this.limitsForm.limitsPageForm.value.uimbiLimitId) : null,
      uimpdLimitId: this.limitsForm.limitsPageForm.value.uimpdLimitId ? parseInt(this.limitsForm.limitsPageForm.value.uimpdLimitId) : null,
      pipLimitId: this.limitsForm.limitsPageForm.value.pipLimitId ? parseInt(this.limitsForm.limitsPageForm.value.pipLimitId) : null,
      medPayLimitId: this.limitsForm.limitsPageForm.value.medPayLimitId ? parseInt(this.limitsForm.limitsPageForm.value.medPayLimitId) : null,
    });
    Utils.blockUI();
    this.limitService.setDefaultRiskCoverageLimit(coverageLimit).pipe(take(1)).subscribe(data => {
      Utils.unblockUI();
      console.log('Limits default Saved!');
    }, (error) => {
      Utils.unblockUI();
      this.toastr.error(ErrorMessageConstant.savingErrorMsg);
    });
  }

  setCoveragesDisplay() {
    const coverage = this.submissionData?.riskDetail?.riskCoverage;
    if (coverage) {
      this.isCollision = coverage.collDeductibleId && coverage.collDeductibleId !== LimitsDefaultValues.colId;
      this.isComprehensive = (coverage.compDeductibleId && coverage.compDeductibleId !== LimitsDefaultValues.compId) ||
      (coverage.fireDeductibleId && coverage.fireDeductibleId !== LimitsDefaultValues.compId);
      this.isCargo = coverage.cargoLimitId && coverage.cargoLimitId !== LimitsDefaultValues.cargoId;
      this.isRefrigeration = coverage.refCargoLimitId && coverage.refCargoLimitId !== LimitsDefaultValues.refId;
    }
  }

  private sortGlList(glList: any[]) {
    glList = glList.sort((a, b) => b.value === 87 || a.value < b.value ? 1 : -1);
    return glList;
  }


  disableSymbol(fieldName, setDefaults = true) {
    switch (fieldName) {
      case 'autoBILimit':
        if(!this.limitsForm.limitsPageForm.controls['alSymbolId'].value && setDefaults &&
            this.limitsForm.limitsPageForm.controls['autoBILimitId'].value) {
          this.limitsForm.limitsPageForm.controls['alSymbolId'].patchValue(LimitsDefaultValues.defaultSymbol);
        }
        break;
      case 'umbiLimit':
        this.disableUnInsuredMotoristBI = this.umBiLimitList?.find(x => x.value == this.limitsForm.limitsPageForm.value.umbiLimitId)?.label == 'NONE';

        if(this.disableUnInsuredMotoristBI) {
          this.limitsForm.limitsPageForm.controls['umbiLimitSymbolId'].patchValue(null);
        }
        else if(!this.disableUnInsuredMotoristBI && !this.limitsForm.limitsPageForm.controls['umbiLimitSymbolId'].value && setDefaults &&
            this.limitsForm.limitsPageForm.controls['umbiLimitId'].value != null) {
          this.limitsForm.limitsPageForm.controls['umbiLimitSymbolId'].patchValue(LimitsDefaultValues.defaultSymbol);
        }
        break;
      case 'umpdLimit':
        this.disableUnInsuredMotoristPD = this.umPdLimitList?.find(x => x.value == this.limitsForm.limitsPageForm.value.umpdLimitId)?.label == 'NONE';

        if(this.disableUnInsuredMotoristPD) {
          this.limitsForm.limitsPageForm.controls['umpdLimitSymbolId'].patchValue(null);
        }
        else if(!this.disableUnInsuredMotoristPD && !this.limitsForm.limitsPageForm.controls['umpdLimitSymbolId'].value && setDefaults &&
            this.limitsForm.limitsPageForm.controls['umpdLimitId'].value != null) {
          this.limitsForm.limitsPageForm.controls['umpdLimitSymbolId'].patchValue(LimitsDefaultValues.defaultSymbol);
        }
        break;
      case 'uimbiLimit':
        this.disableUnderinsuredMotoristBI = this.uimBiLimitList?.find(x => x.value == this.limitsForm.limitsPageForm.value.uimbiLimitId)?.label == 'NONE';

        if(this.disableUnderinsuredMotoristBI) {
          this.limitsForm.limitsPageForm.controls['uimbiLimitSymbolId'].patchValue(null);
        }
        else if(!this.disableUnderinsuredMotoristBI && !this.limitsForm.limitsPageForm.controls['uimbiLimitSymbolId'].value && setDefaults &&
            this.limitsForm.limitsPageForm.controls['uimbiLimitId'].value != null) {
          this.limitsForm.limitsPageForm.controls['uimbiLimitSymbolId'].patchValue(LimitsDefaultValues.defaultSymbol);
        }
        break;
      case 'uimpdLimit':
        this.disableUnderinsuredMotoristPD = this.uimPdLimitList?.find(x => x.value == this.limitsForm.limitsPageForm.value.uimpdLimitId)?.label == 'NONE';

        if(this.disableUnderinsuredMotoristPD) {
          this.limitsForm.limitsPageForm.controls['uimpdLimitSymbolId'].patchValue(null);
        }
        else if(!this.disableUnderinsuredMotoristPD && !this.limitsForm.limitsPageForm.controls['uimpdLimitSymbolId'].value && setDefaults &&
            this.limitsForm.limitsPageForm.controls['uimpdLimitId'].value != null) {
          this.limitsForm.limitsPageForm.controls['uimpdLimitSymbolId'].patchValue(LimitsDefaultValues.defaultSymbol);
        }
        break;
      case 'medPayLimit':
        this.disableMedicalPayments = this.limitsForm.limitsPageForm.value.medPayLimitId == 110;

        if(this.disableMedicalPayments) {
          this.limitsForm.limitsPageForm.controls['medPayLimitSymbolId'].patchValue(null);
        }
        else if(!this.disableMedicalPayments && !this.limitsForm.limitsPageForm.controls['medPayLimitSymbolId'].value && setDefaults &&
            this.limitsForm.limitsPageForm.controls['medPayLimitId'].value != null) {
          this.limitsForm.limitsPageForm.controls['medPayLimitSymbolId'].patchValue(LimitsDefaultValues.defaultSymbol);
        }
        break;
      case 'pipLimit':
        this.disablePersonalInjuryProtection = this.pipList?.find(x => x.value == this.limitsForm.limitsPageForm.value.pipLimitId)?.label == 'NONE';

        if(this.disablePersonalInjuryProtection) {
          this.limitsForm.limitsPageForm.controls['pipLimitSymbolId'].patchValue(null);
        }
        else if(!this.disablePersonalInjuryProtection && !this.limitsForm.limitsPageForm.controls['pipLimitSymbolId'].value && setDefaults &&
            this.limitsForm.limitsPageForm.controls['pipLimitId'].value != null) {
          this.limitsForm.limitsPageForm.controls['pipLimitSymbolId'].patchValue(LimitsDefaultValues.defaultSymbol);
        }
        break;
      case 'compDeductible':
        this.disableCompDeductible = this.limitsForm.limitsPageForm.value.compDeductibleId == 76;

        if(this.disableCompDeductible) {
          this.limitsForm.limitsPageForm.controls['compDeductibleSymbolId'].patchValue(null);
        }
        else if(!this.disableCompDeductible && !this.limitsForm.limitsPageForm.controls['compDeductibleSymbolId'].value && setDefaults &&
            this.limitsForm.limitsPageForm.controls['compDeductibleId'].value != null) {
          this.limitsForm.limitsPageForm.controls['compDeductibleSymbolId'].patchValue(LimitsDefaultValues.defaultSymbol);
        }
        break;
      case 'collDeductible':
        this.disableCollDeductible = this.limitsForm.limitsPageForm.value.collDeductibleId == 65;

        if(this.disableCollDeductible) {
          this.limitsForm.limitsPageForm.controls['collDeductibleSymbolId'].patchValue(null);
        }
        else if(!this.disableCollDeductible && !this.limitsForm.limitsPageForm.controls['collDeductibleSymbolId'].value && setDefaults &&
            this.limitsForm.limitsPageForm.controls['collDeductibleId'].value != null) {
          this.limitsForm.limitsPageForm.controls['collDeductibleSymbolId'].patchValue(LimitsDefaultValues.defaultSymbol);
        }
        break;
      case 'cargoLimit':
        this.disableCargo = this.cargoList?.find(x => x.value == this.limitsForm.limitsPageForm.value.cargoLimitId)?.label == 'NONE';
        if(this.disableCargo){
          this.limitsForm.limitsPageForm.controls['cargoLimitSymbolId'].patchValue(null);
          this.limitsForm.limitsPageForm.controls['refCargoLimitSymbolId'].patchValue(null);
          this.disableRefCargo = true;
        } else if(!this.limitsForm.limitsPageForm.controls['cargoLimitSymbolId'].value && setDefaults &&
            this.limitsForm.limitsPageForm.controls['cargoLimitId'].value != null) {
          this.limitsForm.limitsPageForm.controls['cargoLimitSymbolId'].patchValue(LimitsDefaultValues.defaultSymbol);
        }
        break;
      case 'refCargoLimit':
        this.disableRefCargo = this.refBreakList?.find(x => x.value == this.limitsForm.limitsPageForm.value.refCargoLimitId)?.label == 'NONE';
        if(this.disableRefCargo){
          this.limitsForm.limitsPageForm.controls['refCargoLimitSymbolId'].patchValue(null);
        } else if(!this.limitsForm.limitsPageForm.controls['refCargoLimitSymbolId'].value && setDefaults &&
            this.limitsForm.limitsPageForm.controls['refCargoLimitId'].value != null) {
          this.limitsForm.limitsPageForm.controls['refCargoLimitSymbolId'].patchValue(LimitsDefaultValues.defaultSymbol);
        }
        break;
      case 'trailerInterchangeLimit':
        if (this.raterVersionSupport()) {
          this.disableTrailerInterchange = this.trailerInterchangeList?.find(x => x.value == this.limitsForm.limitsPageForm.value.trailerInterchangeLimitId)?.label == 'NONE'
          || this.raterVersionSupport(8);
          
          if(this.disableTrailerInterchange) {
            this.limitsForm.limitsPageForm.controls['trailerInterchangeLimitSymbolId'].patchValue(null);
          } else if(!this.limitsForm.limitsPageForm.controls['trailerInterchangeLimitSymbolId'].value && setDefaults &&
              this.limitsForm.limitsPageForm.controls['trailerInterchangeLimitId'].value != null) {
            this.limitsForm.limitsPageForm.controls['trailerInterchangeLimitSymbolId'].patchValue(LimitsDefaultValues.defaultSymbol);
          }
          // this.disableUnderinsuredMotoristBI = this.f['uimbiLimitId'].value == 243;
        }
        break;
      case 'nonOwnedTrailerLimit':
        if (this.raterVersionSupport()) {
          this.disableNonOwnedTrailer = this.nonOwnedTrailerList?.find(x => x.value == this.limitsForm.limitsPageForm.value.nonOwnedTrailerLimitId)?.label == 'NONE'
          || this.raterVersionSupport(8);

          if(this.disableNonOwnedTrailer) {
            this.limitsForm.limitsPageForm.controls['nonOwnedTrailerLimitSymbolId'].patchValue(null);
          }
          else if(!this.limitsForm.limitsPageForm.controls['nonOwnedTrailerLimitSymbolId'].value && setDefaults &&
              this.limitsForm.limitsPageForm.controls['nonOwnedTrailerLimitId'].value != null) {
            this.limitsForm.limitsPageForm.controls['nonOwnedTrailerLimitSymbolId'].patchValue(LimitsDefaultValues.defaultSymbol);
          }
          // this.disableUnderinsuredMotoristBI = this.f['uimbiLimitId'].value == 243;
        }
        break;  
    }
  }

  disableSymbolAll() {
    this.disableSymbol('autoBILimit', false);
    this.disableSymbol('umbiLimit', false);
    this.disableSymbol('umpdLimit', false);
    this.disableSymbol('uimbiLimit', false);
    this.disableSymbol('uimpdLimit', false);
    this.disableSymbol('medPayLimit', false);
    this.disableSymbol('pipLimit', false);
    this.disableSymbol('compDeductible', false);
    this.disableSymbol('collDeductible', false);
    this.disableSymbol('cargoLimit', false);
    this.disableSymbol('refCargoLimit', false);
    if (this.raterVersionSupport()) {
      this.disableSymbol('trailerInterchangeLimit', false);
      this.disableSymbol('nonOwnedTrailerLimit', false);  
    }
  }

  raterVersionSupport(id: number = this.RATER_VERSION_SUPPORT) : boolean {
    return this.submissionData?.riskDetail?.raterVersionId == null ||
           this.submissionData?.riskDetail?.raterVersionId >= id ||
           this.submissionData?.riskDetail?.raterVersionId == 0;
  }

  checkLimitsSection() : boolean {
    this.limitsForm.limitsPageForm.markAllAsTouched();
    //return FormUtils.validateForm(this.LimitsPageForm);

    var isValid = 
    FormUtils.validateFormControl(this.limitsForm.limitsPageForm.controls['autoBILimitId']) 
    && FormUtils.validateFormControl(this.limitsForm.limitsPageForm.controls['medPayLimitId'])
    && FormUtils.validateFormControl(this.limitsForm.limitsPageForm.controls['liabilityDeductibleId'])
    && FormUtils.validateFormControl(this.limitsForm.limitsPageForm.controls['compDeductibleId'])
    && FormUtils.validateFormControl(this.limitsForm.limitsPageForm.controls['collDeductibleId'])
    && FormUtils.validateFormControl(this.limitsForm.limitsPageForm.controls['glbiLimitId'])
    && FormUtils.validateFormControl(this.limitsForm.limitsPageForm.controls['cargoLimitId'])
    && FormUtils.validateFormControl(this.limitsForm.limitsPageForm.controls['refCargoLimitId'])
    && FormUtils.validateFormControl(this.limitsForm.limitsPageForm.controls['alSymbolId'])
    && (!this.raterVersionSupport() || (this.raterVersionSupport() && (!this.hasComprehensive)) || (this.raterVersionSupport() && FormUtils.validateFormControl(this.limitsForm.limitsPageForm.controls['compDeductibleSymbolId'])))
    && (!this.raterVersionSupport() || (this.raterVersionSupport() && (this.hasComprehensive || !this.hasCollision)) || (this.raterVersionSupport() && FormUtils.validateFormControl(this.limitsForm.limitsPageForm.controls['collDeductibleSymbolId'])))
    && (!this.raterVersionSupport() || (this.raterVersionSupport() && FormUtils.validateFormControl(this.limitsForm.limitsPageForm.controls['trailerInterchangeLimitId'])))
    && (!this.raterVersionSupport() || (this.raterVersionSupport() && FormUtils.validateFormControl(this.limitsForm.limitsPageForm.controls['nonOwnedTrailerLimitId'])))
    // && (!this.raterVersionSupport() || !this.isTrailerType || (this.raterVersionSupport() && this.isTrailerType && FormUtils.validateFormControl(this.limitsForm.limitsPageForm.controls['trailerTypeLimitId'])))
    && (!this.raterVersionSupport() || (this.raterVersionSupport() && FormUtils.validateFormControl(this.limitsForm.limitsPageForm.controls['pollutionCovLimitId'])));

    return isValid;

  }
}

