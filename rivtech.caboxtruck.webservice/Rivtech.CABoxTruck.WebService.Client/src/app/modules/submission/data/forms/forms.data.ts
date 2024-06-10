import { Injectable } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { FormDTO, FormInfoDTO } from '../../../../shared/models/submission/forms/FormDto';
import { finalize, map, switchMap, take, tap } from 'rxjs/operators';
import { SubmissionData } from '../submission.data';
import { FormsService } from '../../services/forms.service';
import { LimitsData } from '../limits/limits.data';
import { BindingService } from '../../services/binding.service';
import { ApplicantAdditionalInterestService } from '../../services/applicant-additional-interest.service';
import { RiskAdditionalInterestDTO } from '../../../../shared/models/submission/RiskAdditionalInterestDto';
import { TableData } from '../tables.data';
import { fileSizeValidatorMultiple, customFileTypeValidatorMultiple, filenameLengthValidator } from '../../helpers/file.validator';
import Utils from '../../../../shared/utilities/utils';
import { SelectItem } from '@app/shared/models/dynamic/select-item';
import NotifUtils from '@app/shared/utilities/notif-utils';
import { BindingData } from '../binding/binding.data';
import { ErrorMessageConstant } from '../../../../shared/constants/error-message.constants';
import { IAngularMyDpOptions } from 'angular-mydatepicker';
import { SummaryData } from '../summary.data';
import { QuoteLimitsData } from '../quote/quote-limits.data';
import { mcs90Validator } from '../../helpers/forms.validator';
import { BrokerGenInfoData } from '../broker/general-information.data';
import { PolicySummaryData } from '@app/modules/policy/data/policy-summary.data';
import { Observable, of } from 'rxjs';
import { DriverData } from '../driver/driver.data';
import { DocumentApiService } from '@app/modules/documents/document-api.service';

@Injectable()
export class FormsData {
  readonly RATER_VERSION_SUPPORT = 7;
  forms: FormDTO[];
  uploadForm: FormGroup;
  exclusionRadiusForm: FormGroup;
  hiredPhysdamForm: FormGroup;
  driverRequirementForm: FormGroup;
  territoryExclusionForm: FormGroup;
  manuscriptForm: FormGroup;
  trailerInterchangeForm: FormGroup;
  generalLiabilityForm: FormGroup;
  mcs90Form: FormGroup;
  lossPayeeForm: FormGroup;
  paAddCombFPBForm: FormGroup;
  coveredAutoDesignationForm: FormGroup;
  bindOption: string;
  additionalInterests: RiskAdditionalInterestDTO[] = [];
  stateList = [];
  monthsList = [];
  surplusLineNumber: string;
  isPolicyUI = false;
  isEndorsement = false;

  datepickerOptions: IAngularMyDpOptions = {
    dateRange: false,
    dateFormat: 'mm/dd/yyyy',
    disableSince: { year: 0, month: 0, day: 0 },
    disableUntil: { year: 0, month: 0, day: 0 }
  };

  currencySettings: any = {
    align: 'left',
    allowNegative: false,
    precision: 0,
    nullable: true
  };

  constructor(
    private fb: FormBuilder,
    private bindingService: BindingService,
    private bindingData: BindingData,
    private interestService: ApplicantAdditionalInterestService,
    private formsService: FormsService,
    private limitsData: LimitsData,
    private submissionData: SubmissionData,
    private tableData: TableData,
    private summaryData: SummaryData,
    private quoteLimitsData: QuoteLimitsData,
    private brokerData: BrokerGenInfoData,
    private policySummaryData: PolicySummaryData,
    private driverData: DriverData,
    private documentService: DocumentApiService
  ) { }

  initiateFormFields() {
    this.forms = [];

    this.uploadForm = this.fb.group({
      description: [null, Validators.required],
      fileInfo: [null, [customFileTypeValidatorMultiple, fileSizeValidatorMultiple, filenameLengthValidator]],
      fileName: [null, Validators.required],
    });

    this.exclusionRadiusForm = this.fb.group({
      radius: [null, [Validators.required, Validators.min(1)]]
    });

    this.hiredPhysdamForm = this.fb.group({
      estimatedAdditionalPremium: [null, [Validators.required, Validators.min(1)]],
      vehicleDays: [null, [Validators.required, Validators.min(1)]],
      ratePerDay: [null, [Validators.required, Validators.min(1)]],
      grossCombinedWeight: [null, [Validators.required, Validators.min(1)]],
      accidentLimitPerVehicle: [null, [Validators.required, Validators.min(1)]],
      deductible: [null, [Validators.required, Validators.min(1)]],
      physdamLimit: [null, [Validators.required, Validators.min(1)]]
    });

    this.driverRequirementForm = this.fb.group({
      requirement: [null, Validators.required]
    });

    this.territoryExclusionForm = this.fb.group({
      states: [null, Validators.required]
    });

    this.manuscriptForm = this.fb.group({
      premium: [null, [Validators.required, Validators.min(1)]],
      description: [null, Validators.required]
    });

    this.trailerInterchangeForm = this.fb.group({
      compreLimitOfInsurance: [null, [Validators.required, Validators.min(1)]],
      compreDeductible: [null, [Validators.required, Validators.min(1)]],
      comprePremium: [null, [Validators.required, Validators.min(1)]],
      lossLimitOfInsurance: [null, [Validators.required, Validators.min(1)]],
      lossDeductible: [null, [Validators.required, Validators.min(1)]],
      lossPremium: [null, [Validators.required, Validators.min(1)]],
      collisionLimitOfInsurance: [null, [Validators.required, Validators.min(1)]],
      collisionDeductible: [null, [Validators.required, Validators.min(1)]],
      collisionPremium: [null, [Validators.required, Validators.min(1)]],
      fireLimitOfInsurance: [null, [Validators.required, Validators.min(1)]],
      fireDeductible: [null, [Validators.required, Validators.min(1)]],
      firePremium: [null, [Validators.required, Validators.min(1)]],
      fireTheftLimitOfInsurance: [null, [Validators.required, Validators.min(1)]],
      fireTheftDeductible: [null, [Validators.required, Validators.min(1)]],
      fireTheftPremium: [null, [Validators.required, Validators.min(1)]],
    });

    this.generalLiabilityForm = this.fb.group({
      injuryLimit: [null, [Validators.required, Validators.min(1)]],
      medicalExpenses: [null, [Validators.required, Validators.min(1)]],
      damagesRentedToYou: [null, [Validators.required, Validators.min(1)]],
      aggregateLimit: [null, [Validators.required, Validators.min(1)]],
    });

    this.mcs90Form = this.fb.group({
      usDotNumber: [null, Validators.required],
      dateReceived: [null, Validators.required],
      issuedTo: [null, Validators.required],
      state: [null, Validators.required],
      datedAt: [null, Validators.required],
      policyDateDay: [null, [Validators.required, Validators.min(1), Validators.max(31)]],
      policyDateMonth: [null, Validators.required],
      policyDateYear: [null, Validators.required],
      policyNumber: [null, Validators.required],
      effectiveDate: [null, Validators.required],
      insuranceCompany: [null, Validators.required],
      counterSignedBy: [null, Validators.required],
      accidentLimit: [null, [Validators.required, Validators.min(1)]]
    },
    {
      validators: [mcs90Validator(this.brokerData)]
    });

    this.lossPayeeForm = this.fb.group({
      descriptionOfAutos: [null, [Validators.required]]
    });

    this.paAddCombFPBForm = this.fb.group({
      addedFirstPartyBenefits: [false],
      medicalExpenseBenefits: [null, [Validators.required, Validators.min(1)]],
      workLossUpTo: [null, [Validators.required, Validators.min(1)]],
      workLossPerMonth: [null, [Validators.required, Validators.min(1)]],
      funeralExpenseBenefits: [null, [Validators.required, Validators.min(1)]],
      accidentalDeathBenefits: [null, [Validators.required, Validators.min(1)]],
      combinationFirstPartyBenefits: [false],
      maximumTotalLimitAllBenefits: [null, [Validators.required, Validators.min(1)]],
      combFuneralExpenseBenefits: [null, [Validators.required, Validators.min(1)]],
      combAccidentalDeathBenefits: [null, [Validators.required, Validators.min(1)]],
    });

    this.coveredAutoDesignationForm = this.fb.group({
      coveredAutoText: [null, [Validators.required]],
      textForEndorsement: [null, [Validators.required]]
    })
  }

  retrieveDropdownValues() {
    this.bindingService.getBindOption().pipe(take(1)).subscribe(data => {
      this.bindOption = data.find(d => d.id === this.submissionData.riskDetail?.binding?.bindOptionId)?.description ?? 'Option 1';
    });
    this.getStates();
    this.monthsList = ['January', 'February', 'March', 'April', 'May', 'June', 'July', 'August', 'September', 'October', 'November', 'December'];
  }

  populateFields$(isEndorsement: boolean = false) {
    const risk = this.submissionData.riskDetail;
    this.isEndorsement = isEndorsement;
    this.getSurplusLineNumber();

    let forms$: Observable<any>;
    if (risk?.riskForms && risk.riskForms.length > 0) {
      this.forms = risk.riskForms;
      if(this.raterVersionSupport()) { //For rater v7 and up
        forms$ = this.createDefaultFormsNew$(risk.id).pipe(
          map(() => this.forms)
        );
      } else {
        forms$ = of(this.forms);
      }
    } else {
      forms$ = this.createDefaultForms$(risk.id).pipe(
        map(() => this.forms)
      );
    }
    
    return forms$.pipe(
      switchMap(() => this.interestService.getAdditionalInterests(this.submissionData.riskDetail?.id)),
      tap(data => this.additionalInterests = data),
      switchMap(() => this.reloadDriverExclusionForm$()),
      tap(() => this.applyRules())
    );
  }

  reloadDriverExclusionForm$() : Observable<any> {
    return this.driverData.updateDriverExclusionEndorsementForm$(this.submissionData.riskDetail?.id).pipe(
      switchMap(() => this.formsService.getByForm(this.submissionData.riskDetail.id, "DriverExclusion")),
      tap((data) => {
        const driverForm = this.forms.find(x => x.form?.id == 'DriverExclusion');
        if(driverForm != null) {
          this.forms = this.forms.filter(x => x !== driverForm);
          this.forms.push(data as FormDTO);
        }
      })
    );
  }

  ensureMandatorySelected(preForms: any) {
    this.forms.forEach(f => {
      if (f.form.isMandatory) {
        f.isSelected = true;
      }
    });
    this.formsService.postPreForms(preForms).subscribe();
  }

  save() {
    this.formsService.post(this.forms).pipe(take(1)).subscribe(() => { });
  }

  createDefaultForms$(riskId: string) {
    return this.formsService.postDefaults(riskId).pipe(
      tap((data) => this.forms = this.submissionData.riskDetail.riskForms = data)
    );
  }

  createDefaultFormsNew$(riskId: string) {
    return this.formsService.postDefaultsNew(riskId).pipe(
      tap((data) => this.forms = this.submissionData.riskDetail.riskForms = data)
    );
  }
  
  mergeManuscripts() {
    this.forms = this.forms.filter(f => f.form.formNumber !== 'RPCAMANU'); // Remove first merged manuscripts
    const ofacSorOrder = this.forms.find(f => f.form.id === 'OFACNotice')?.form?.sortOrder;
    
    this.submissionData.riskDetail?.riskManuscripts?.forEach(m => {
      if (m.isSelected == true || m.isProrate == true) {
        const formInfoData = {
          id: 'ManuscriptEndorsement',
          name: m.title,
          formNumber: 'RPCAMANU',
          sortOrder: ofacSorOrder - 1
        };
        const formData = {
          id: m.id,
          riskDetailId: this.submissionData.riskDetail?.id,
          isSelected: m.isSelected,
          form: new FormInfoDTO(formInfoData)
        };
        this.forms.push(new FormDTO(formData));
      }
    });
  }

  applyRules() {
    // this.mergeManuscripts();

    const formRules = [
      {
        formId: 'SurplusLinesNotice',
        number: this.surplusLineNumber,
        include: this.hasSurplusLinesNoticeForm,
      },
      {
        formId: 'PrimaryAndNoncontributory',
        include: this.hasPrimaryNonContributory,
        isMandatory: this.hasPrimaryNonContributory,
      },
      {
        formId: 'WaiverOfTransferOfRights',
        include: this.hasWaiverOfSubrogation,
        isMandatory: this.hasWaiverOfSubrogation,
      },
      {
        formId: 'PhysicalDamageLimitOfInsurance',
        include: this.limitsData.hasAutoPhysicalDamage,
        isMandatory: this.limitsData.hasAutoPhysicalDamage,
      },
      {
        formId: 'InstallmentSchedule',
        include: this.isDirectBillInstallments,
        isMandatory: this.isDirectBillInstallments
      },
      {
        formId: 'PhysicalDamageCoverage',
        include: this.limitsData.hasAutoPhysicalDamage,
        isMandatory: this.limitsData.hasAutoPhysicalDamage,
      },
      {
        formId: 'AdditionalInsuredBlanket',
        include: this.hasAdditionaInsuredBlanket,
        isMandatory: this.hasAdditionaInsuredBlanket
      },
      {
        formId: 'DesignatedInsured',
        include: this.hasDesignatedInsured,
        isMandatory: this.hasDesignatedInsured
      },
      {
        formId: 'DriverExclusion',
        include: this.hasDriverExclusions || this.submissionData.riskDetail.hasInitialDriverExclusion == true,
        //isMandatory: this.hasDriverExclusions || this.submissionData.riskDetail.hasInitialDriverExclusion
      },
      {
        formId: 'GeneralLiabilityEndorsement',
        include: this.limitsData.hasGeneralLiability
      },
      {
        formId: 'TrailerInterchangeCoverage',
        include: !this.raterVersionSupport(),
      },
      {
        formId: 'TrailerInterchangeCoverageAL',
        include: this.limitsData.hasTrailerInterchange,
        isMandatory: this.limitsData.hasTrailerInterchange
      },
      {
        formId: 'MotorTruckCargoEndorsement',
        include: this.limitsData.hasCargo,
        isMandatory: this.limitsData.hasCargo,
      },
      {
        formId: 'LossPayeeEndorsement',
        include: this.hasLossPayee,
        // isMandatory: this.hasLossPayee,
      },
      {
        formId: 'CourierCoverageEndorsement',
        include: this.isMainUseCourier
      },
      {
        formId: 'RefrigerationCargoEndorsement',
        include: this.limitsData.hasRefrigeration,
        isMandatory: this.limitsData.hasRefrigeration,
      },
      {
        formId: 'UninsuredMotoristCoverageBodilyInjury',
        include: this.limitsData.hasUninsuredMotoristBI
      },
      {
        formId: 'SplitBodilyInjuryUninsuredMotoristCoverage',
        include: this.limitsData.hasUninsuredMotoristBI
      },
      {
        formId: 'CoveredAutoDesignationSymbol',
        include: this.limitsData.hasSymbol10
      },
      {
        formId: 'TexasChangesCancellationandNonRenewal',
        include: this.isStateTX,
        isMandatory: this.isStateTX
      },
      {
        formId: 'TexasChanges',
        include: this.isStateTX,
        isMandatory: this.isStateTX
      },
      {
        formId: 'TexasPersonalInjuryProtection',
        include: this.isStateTX && this.limitsData.hasTXPIPLimits,
        isMandatory: this.isStateTX && this.limitsData.hasTXPIPLimits
      },
      {
        formId: 'TexasUMandUIMCoverage',
        include: this.isStateTX && this.limitsData.hasTXUMUIMLimits,
        isMandatory: this.isStateTX && this.limitsData.hasTXUMUIMLimits
      },
      {
        formId: 'TexasSplitUninsuredandUnderinsuredMotoristCoverage',
        include: this.isStateTX && this.limitsData.hasTXSplitUMUIMLimits,
        isMandatory: this.isStateTX && this.limitsData.hasTXSplitUMUIMLimits
      },
      {
        formId: 'NewJerseyUMandUIMCoverage',
        include: this.isStateNJ, // && this.limitsData.hasNJUMUIMLimits,
        isMandatory: this.isStateNJ // && this.limitsData.hasNJUMUIMLimits
      },
      {
        formId: 'NewJerseyChanges',
        include: this.isStateNJ,
        isMandatory: this.isStateNJ
      },
      {
        formId: 'NewJerseyPersonalInjuryProtection',
        include: this.isStateNJ, // && this.limitsData.hasNJPIPLimits,
        isMandatory: this.isStateNJ // && this.limitsData.hasNJPIPLimits
      },
      {
        formId: 'NewJerseySplitUninsuredandUnderinsuredMotoristCoverage',
        include: this.isStateNJ && this.limitsData.hasNJSplitUMUIMLimits,
        isMandatory: this.isStateNJ && this.limitsData.hasNJSplitUMUIMLimits
      },
      {
        formId: 'MissouriChanges',
        include: this.isStateMO,
        isMandatory: this.isStateMO
      },
      {
        formId: 'MissouriUninsuredMotoristsCoverage',
        include: this.isStateMO && this.limitsData.hasMOUMLimits,
        isMandatory: this.isStateMO && this.limitsData.hasMOUMLimits
      },
      {
        formId: 'MissouriUnderinsuredMotoristsCoverage',
        include: this.isStateMO && this.limitsData.hasMOUIMLimits,
        isMandatory: this.isStateMO && this.limitsData.hasMOUIMLimits
      },
      {
        formId: 'GeorgiaChanges',
        include: this.isStateGA,
        isMandatory: this.isStateGA 
      },
      {
        formId: 'GeorgiaPolicyHolderNoticeRegardingUMCoverage',
        include: this.isStateGA && this.limitsData.hasGAUMLimits,
        isMandatory: this.isStateGA  && this.limitsData.hasGAUMLimits
      },
      {
        formId: 'GeorgiaUMCoverageReducedbyatFaultLiabilityLimits',
        include: this.isStateGA  
      },
      {
        formId: 'PennsylvaniaChanges',
        include: this.isStatePA,
        isMandatory: this.isStatePA
      },
      {
        formId: 'PennsylvaniaBasicFirstPartyBenefits',
        include: this.isStatePA,
        isMandatory: this.isStatePA
      },
      {
        formId: 'PennsylvaniaSplitUMCoverageLimitsNS',
        include: this.isStatePA && this.limitsData.hasPASplitUMLimits,
        isMandatory: this.isStatePA && this.limitsData.hasPASplitUMLimits
      },
      {
        formId: 'PennsylvaniaSplitUIMCoverageLimitsNS',
        include: this.isStatePA && this.limitsData.hasPASplitUIMLimits,
        isMandatory: this.isStatePA && this.limitsData.hasPASplitUIMLimits
      },
      {
        formId: 'PennsylvaniaUMCoverageLimitsNS',
        include: this.isStatePA && this.limitsData.hasPAUMLimits,
        isMandatory: this.isStatePA && this.limitsData.hasPAUMLimits
      },
      {
        formId: 'PennsylvaniaUIMCoverageLimitsNS',
        include: this.isStatePA && this.limitsData.hasPAUIMLimits,
        isMandatory: this.isStatePA && this.limitsData.hasPAUIMLimits
      },
      {
        formId: 'PennsylvaniaAddCombFirstPartyBenefitsEndorsement',
        include: this.isStatePA,
      },
      {
        formId: 'FloridaChanges',
        include: this.isStateFL,
        isMandatory: this.isStateFL
      },
      {
        formId: 'FloridaPersonalInjuryProtection',
        include: this.isStateFL,
        isMandatory: this.isStateFL
      },
      {
        formId: 'FloridaUninsuredMotoristsCoverageNS',
        include: this.isStateFL && this.limitsData.hasFLUMLimits,
        isMandatory: this.isStateFL && this.limitsData.hasFLUMLimits
      },
      {
        formId: 'OregonChanges',
        include: this.isState('OR'),
        isMandatory: this.isState('OR')
      },
      {
        formId: 'OregonPersonalInjuryProtection',
        include: this.isState('OR'),
        isMandatory: this.isState('OR')
      },
      {
        formId: 'OregonUninsuredMotoristCoverage',
        include: this.isState('OR') && this.limitsData.hasORUMLimits,
        isMandatory: this.isState('OR') && this.limitsData.hasORUMLimits
      },
      {
        formId: 'OhioChanges',
        include: this.isState('OH'),
        isMandatory: this.isState('OH')
      },
      {
        formId: 'OhioUMandUIMCoverage',
        include: this.isState('OH') && this.limitsData.hasOHUMLimits,
        isMandatory: this.isState('OH') && this.limitsData.hasOHUMLimits
      },
      {
        formId: 'ColoradoChanges',
        include: this.isState('CO'),
        isMandatory: this.isState('CO')
      },
      {
        formId: 'ColoradoUMBICoverage',
        include: this.isState('CO') && this.limitsData.hasCOUMLimits,
        isMandatory: this.isState('CO') && this.limitsData.hasCOUMLimits
      },
      {
        formId: 'ColoradoUMPDCoverage',
        include: this.isState('CO') && this.limitsData.hasCOUMLimits,
        isMandatory: this.isState('CO') && this.limitsData.hasCOUMLimits
      },
      {
        formId: 'ColoradoMedicalPaymentCoverage',
        include: this.isState('CO') && this.limitsData.hasCOMPLimits,
        isMandatory: this.isState('CO') && this.limitsData.hasCOMPLimits
      },
      {
        formId: 'IndianaChanges',
        include: this.isState('IN'),
        isMandatory: this.isState('IN')
      },
      {
        formId: 'IndianaUninsuredMotoristCoverage',
        include: this.isState('IN') && this.limitsData.hasINUMLimits,
        isMandatory: this.isState('IN') && this.limitsData.hasINUMLimits
      },
      {
        formId: 'IndianaUnderinsuredMotoristCoverage',
        include: this.isState('IN') && this.limitsData.hasINUIMLimits,
        isMandatory: this.isState('IN') && this.limitsData.hasINUIMLimits
      },
      {
        formId: 'VirginiaSplitUMCoverage',
        include: this.isState('VA') && this.limitsData.hasVASplitUMLimits,
        isMandatory: this.isState('VA') && this.limitsData.hasVASplitUMLimits
      },
      {
        formId: 'VirginiaImportantNoticeUMCoverage',
        include: this.isState('VA') && this.limitsData.hasVAUMLimits,
        isMandatory: this.isState('VA') && this.limitsData.hasVAUMLimits
      },
      {
        formId: 'TennesseeCarolinaChanges',
        include: this.isState('TN'),
        isMandatory: this.isState('TN')
      },
      {
        formId: 'TennesseeUninsuredMotoristCoverage',
        include: this.isState('TN') && this.limitsData.hasTNUMLimits,
        isMandatory: this.isState('TN') && this.limitsData.hasTNUMLimits
      },
      {
        formId: 'IllinoisChanges',
        include: this.isState('IL'),
        isMandatory: this.isState('IL')
      },
      {
        formId: 'IllinoisUMCoverage',
        include: this.isState('IL') && this.limitsData.hasILUMLimits,
        isMandatory: this.isState('IL') && this.limitsData.hasILUMLimits
      },
      {
        formId: 'IllinoisUMPDCoverage',
        include: this.isState('IL') && this.limitsData.hasILUMLimits,
        isMandatory: this.isState('IL') && this.limitsData.hasILUMLimits
      },
      {
        formId: 'IllinoisUIMCoverage',
        include: this.isState('IL') && this.limitsData.hasILUIMLimits,
        isMandatory: this.isState('IL') && this.limitsData.hasILUIMLimits
      },
      {
        formId: 'ArizonaChanges',
        include: this.isState('AZ'),
        isMandatory: this.isState('AZ')
      },
      {
        formId: 'ArizonaUIMCoverage',
        include: this.isState('AZ') && this.limitsData.hasAZUIMLimits,
        isMandatory: this.isState('AZ') && this.limitsData.hasAZUIMLimits
      },
      {
        formId: 'ArizonaUMCoverage',
        include: this.isState('AZ') && this.limitsData.hasAZUMLimits,
        isMandatory: this.isState('AZ') && this.limitsData.hasAZUMLimits
      },
      {
        formId: 'MississippiUninsuredMotoristCoverageNS',
        include: this.isState('MS') && this.limitsData.hasMSUMLimits,
        isMandatory: this.isState('MS') && this.limitsData.hasMSUMLimits
      },
      {
        formId: 'SouthCarolinaChanges',
        include: this.isState('SC'),
        isMandatory: this.isState('SC')
      },
      {
        formId: 'SouthCarolinaSplitUMCoverage',
        include: this.isState('SC') && this.limitsData.hasSCSplitUMLimits,
        isMandatory: this.isState('SC') && this.limitsData.hasSCSplitUMLimits
      },
      {
        formId: 'SouthCarolinaSplitUIMCoverage',
        include: this.isState('SC') && this.limitsData.hasSCSplitUIMLimits,
        isMandatory: this.isState('SC') && this.limitsData.hasSCSplitUIMLimits
      },
      {
        formId: 'SouthCarolinaUMCoverage',
        include: this.isState('SC') && this.limitsData.hasSCUMLimits,
        isMandatory: this.isState('SC') && this.limitsData.hasSCUMLimits
      },
      {
        formId: 'SouthCarolinaUIMCoverage',
        include: this.isState('SC') && this.limitsData.hasSCUIMLimits,
        isMandatory: this.isState('SC') && this.limitsData.hasSCUIMLimits
      },
      {
        formId: 'PollutionCoverage',
        include: this.limitsData.hasPOC,
        isMandatory: this.limitsData.hasPOC
      },
      {
        formId: 'PermissiveExclusionEndorsement',
        include: false,
      },
      {
        formId: 'UnreportedVehicleDeductible',
        include: false,
      },
      {
        formId: 'UnreportedDriverDeductible',
        include: false,
      },
      {
        formId: 'FormFMotorCarrierBIPD',
        include: false,
        isMandatory: false
      },
      {
        formId: 'MotorCarrierEndorsement',
        include: false,
        isMandatory: false
      },
    ];

    const preForms = [];
    this.forms.forEach(f => {
      f.include = true;
      const formRule = formRules.find(fr => fr.formId === f.form?.id);

      if (formRule) {
        const s37forms= ["FormFMotorCarrierBIPD","UnreportedDriverDeductible","UnreportedVehicleDeductible","PermissiveExclusionEndorsement"];
        if(s37forms.includes(f.form?.id)) {
          var isIncludeForm = this.submissionData.isNewFeatureApplicableByQuoteDate('S37OptionalForms');

          if(f.form?.id == "FormFMotorCarrierBIPD") {
            formRule.include = isIncludeForm && this.hasFormE;
            formRule.isMandatory = isIncludeForm && this.hasFormE;
          }
          else
            formRule.include = isIncludeForm
        }
        
        if(f.form?.id == "MotorCarrierEndorsement") {
          var isIncludeForm = this.submissionData.isNewFeatureApplicableByQuoteDate('S38MotorCarrierNumber');
          formRule.include = isIncludeForm && this.isInsuredHasMotorCarrierNo;
          formRule.isMandatory = isIncludeForm && this.isInsuredHasMotorCarrierNo;
        }

        if (formRule.number !== undefined) {
          f.form.formNumber = formRule.number;
        }
        if (formRule.isMandatory !== undefined) {
          f.form.isMandatory = formRule.isMandatory;
        }
        if (formRule.include !== undefined) {
          f.include = formRule.include;
          if(f.form?.id !== "DriverExclusion" && f.form?.id !== "GeorgiaUMCoverageReducedbyatFaultLiabilityLimits") {
            f.isSelected = formRule.include ? f.isSelected : false;
          }
          else if(f.form?.id == "GeorgiaUMCoverageReducedbyatFaultLiabilityLimits")
          {
            f.isSelected = this.limitsData.hasGAUMLimits;
          }

          preForms.push(f);
        }
      }
    });

    console.log('hasInitialDriverExclusion', this.submissionData.riskDetail.hasInitialDriverExclusion);

    this.ensureMandatorySelected(preForms);
    this.forms = this.forms.sort(this.sorter);


    /*
    const driverForm = this.forms.find(x => x.form?.id == 'DriverExclusion');
    //let driverFormIndex = this.forms.indexOf(driverForm);
    if (driverForm != null && driverForm.include == false) 
    {
      driverForm.isSelected = null;
      driverForm.include = false;
      console.log('driverForm', driverForm);
      this.updateRiskForm(driverForm)
      this.forms = this.forms.filter(x => x.form?.id !== 'DriverExclusion');
    } else if (driverForm.other != null) {
      driverForm.include = true;
      this.updateRiskForm(driverForm);
    }*/

    // Remove forms on display
    if (!this.limitsData.hasUninsuredMotoristBI) {
      const umbiForm = this.forms.find(x => x.form?.id == 'DriveUninsuredMotoristCoverageBodilyInjuryrExclusion' || x.form?.id == 'SplitBodilyInjuryUninsuredMotoristCoverage');
      if (umbiForm) {
        let index = this.forms.indexOf(umbiForm);
        this.forms[index].isSelected = false;
        this.forms[index].include = false;
        console.log('umbiForm', this.forms[index]);
        this.updateRiskForm(this.forms[index]);
      }
      this.forms = this.forms.filter(x => x.form?.id !== 'UninsuredMotoristCoverageBodilyInjury' && x.form?.id !== 'SplitBodilyInjuryUninsuredMotoristCoverage');
    }
  }

  private get hasPrimaryNonContributory(): boolean {
    return this.submissionData.riskDetail?.blanketCoverage?.isPrimaryNonContributory ||
      this.additionalInterests.some(ai => ai.isPrimaryNonContributory && ai.removeProcessDate === null);
  }

  private get isMainUseCourier(): boolean {
    return +this.submissionData.riskDetail?.businessDetails?.mainUseId === 1;
  }

  private get hasWaiverOfSubrogation(): boolean {
    return this.submissionData.riskDetail?.blanketCoverage?.isWaiverOfSubrogation ||
      this.additionalInterests.some(ai => ai.isWaiverOfSubrogation && ai.removeProcessDate === null);
  }

  private get isDirectBillInstallments() {
    return +this.submissionData.riskDetail?.binding?.paymentTypesId === 2;
  }

  private get hasAdditionaInsuredBlanket() {
    return this.submissionData.riskDetail?.blanketCoverage?.isAdditionalInsured
      || this.additionalInterests.some(ai => ai.additionalInterestTypeId === 1 && ai.removeProcessDate === null);
  }

  private get isStateTX() {
    return this.submissionData.riskDetail.nameAndAddress.state == 'TX';
  }

  private get isStateNJ() {
    return this.submissionData.riskDetail.nameAndAddress.state == 'NJ';
  }

  private get isStateMO() {
    return this.submissionData.riskDetail.nameAndAddress.state == 'MO';
  }

  private get isStateGA() {
    return this.submissionData.riskDetail.nameAndAddress.state =='GA';
  }

  private get isStatePA() {
    return this.submissionData.riskDetail.nameAndAddress.state == 'PA';
  }

  private get isStateFL() {
    return this.submissionData.riskDetail.nameAndAddress.state == 'FL';
  }

  private get hasDesignatedInsured() {
    return this.additionalInterests.some(ai => ai.additionalInterestTypeId === 12 && ai.removeProcessDate === null);
  }

  private get hasLossPayee() {
    return this.additionalInterests.some(ai => ai.additionalInterestTypeId === 2 && ai.removeProcessDate === null);
  }

  private get hasDriverExclusions() {
    const form = this.forms.find(x => x.form.id == "DriverExclusion");
    return form?.other != null;
  }

  private get hasSurplusLinesNoticeForm() {
    const slForms = ['AL', 'AZ', 'CA', 'CO', 'DE', 'GA', 'IL', 'KY', 'MD', 'NC',
    'NJ', 'NV', 'OH', 'OR', 'PA', 'SC', 'TN', 'TX', 'VA', 'WA','MO','GA'];
    return slForms.includes(this.submissionData.riskDetail?.nameAndAddress?.state);
  }

  private get hasFormE() {
    const filingIds = this.submissionData.riskDetail?.filingsInformation?.requiredFilingIds;
    if(filingIds)
      return filingIds.some(id => id == 9 || id == 11);

    return false;
  }

  private get isInsuredHasMotorCarrierNo(){
    return this.submissionData.riskDetail?.filingsInformation?.isInsuredHasMotorCarrierNo;
  }

  private isState(state) {
    return this.submissionData.riskDetail.nameAndAddress.state == state;
  }

  private getSurplusLineNumber() {
    const bindSurplusNum = this.submissionData.riskDetail?.binding?.surplusLIneNum;
    if (!bindSurplusNum) {
      this.setDefaultSurplusLineNumber();
    }
    this.surplusLineNumber = bindSurplusNum;
  }

  sort(targetElement) {
    this.tableData.sort(targetElement, this.forms, 1);
  }

  private sorter(a: any, b: any) {
    if (a.form.sortOrder === b.form.sortOrder) {
      return a.form.createdDate < b.form.createdDate ? -1 : 1;
    }
    return a.form.sortOrder < b.form.sortOrder ? -1 : 1;
  }

  viewForm(riskForm?: FormDTO) {
    Utils.blockUI();
    if (!riskForm) {
      riskForm = new FormDTO();
      riskForm.riskDetailId = this.submissionData.riskDetail.id;
    }
    const riskForms = [];
    riskForms.push(riskForm);

    this.generateForms(riskForms);
  }

  viewForms(riskForms?: FormDTO[]) {
    const excludeFromPacket = []; // Contains pop-up and mapping info only;
    const generalLiabilityForm = riskForms.filter(rf => rf.form?.formNumber === 'RPCA030'); //Get GL Form
    riskForms = riskForms.filter(rf => rf.isSelected
        && !excludeFromPacket.includes(rf.form.formNumber));

    if (generalLiabilityForm !== null && generalLiabilityForm.length != 0)  
    {
      if (!generalLiabilityForm[0].isSelected)
        riskForms.push(generalLiabilityForm[0]);
    }

    this.generateForms(riskForms);
  }

  viewInvoice(invoice: any) {
    this.formsService.generateInvoice(this.submissionData.riskDetail?.riskId, invoice).subscribe(result => {
      Utils.unblockUI();
      // const quoteFileUrl = decodeURIComponent(result);
      this.documentService.getDocumentSASURL(result).pipe().take(1).subscribe(data => {
        const win = window.open(data, '_blank');
        win.focus();
      });
    });
  }

  generateForms(riskForms: FormDTO[]) {
    this.formsService.generate(riskForms).subscribe(result => {
      if (result !== null) {
        Utils.unblockUI();
        // const quoteFileUrl = decodeURIComponent(result);
        this.documentService.getDocumentSASURL(result).pipe().take(1).subscribe(data => {
          const win = window.open(data, '_blank');
          win.focus();
        });
      } else {
        NotifUtils.showError(ErrorMessageConstant.packetGenerationErrorMsg);
      }
    }, (error) => {
      console.error(error);
      NotifUtils.showError(ErrorMessageConstant.packetGenerationErrorMsg);
    });
  }

  updateRiskForm(riskForm?: FormDTO, fromEndorsement: boolean = false) {
    if (!riskForm) { return; }
    if (this.submissionData?.isIssued && !this.isPolicyUI) { return; }

    // if (!riskForm.isSelected && this.isSubPhysicalDamageForm(riskForm?.form?.id)) {
    //   riskForm.other = null; // reset forms if unselected
    // }

    Utils.blockUI();
    this.formsService.updateForm(riskForm)
    .pipe(
      finalize(() => {
        if (this.isSubPhysicalDamageForm(riskForm?.form?.id)) {
          this.refreshQuoteOptionsAndSummaryHeader();
        }
        if (fromEndorsement) { this.policySummaryData.updateSubtatusForPendingEndorsement(); }
        Utils.unblockUI();
      }), take(1))
    .subscribe(result => {
      // console.log(`${riskForm.form?.name} selected successfully`);
    }, (error) => {
      console.log(error);
    });
  }

  getStates() {
    this.formsService.getStates().pipe(take(1)).subscribe(data => {
      this.stateList = data.map(s => ({
        value: s.stateCode?.toString(),
        label: s.fullStateName?.toString()?.replace('[E]', '')
      })) as SelectItem[];
    });
  }

  deleteRiskForm(riskFormId: string) {
    if (!riskFormId) { return; }

    Utils.blockUI();
    this.formsService.removeForm(riskFormId)
      .pipe(finalize(() => Utils.unblockUI()), take(1))
      .subscribe(result => {
        const riskForms = this.forms.filter(form => form.id !== riskFormId);
        this.submissionData.riskDetail.riskForms = riskForms.map(x => Object.assign({}, x));
        this.forms = this.submissionData.riskDetail.riskForms;
      }, (error) => {
        console.log(error);
        NotifUtils.showError('There was an error trying to delete the record. Please try again');
      });
  }

  updateDefaultStates() {
    const territoryExcForm = this.forms?.find(f => f.form.id === 'TerritoryExclusion');
    if (!territoryExcForm) { return; }

    const statesDefault = JSON.parse(territoryExcForm?.other)?.statesDefault;
    if (statesDefault === undefined || statesDefault) {
      this.formsService.updateDefaultStates(this.submissionData.riskDetail?.id).subscribe((data) => {
        territoryExcForm.other = data?.other;
      });
    }
  }

  // TODO: Refactor, this is dirty and redundant
  setDefaultSurplusLineNumber() {
    const riskDetail = this.submissionData.riskDetail;
    const policyState = riskDetail?.nameAndAddress?.state;
    const creditedOfficeId = riskDetail?.binding?.creditedOfficeId ?? '3';
    this.bindingService.getSurplusLineDefault(creditedOfficeId, policyState).subscribe(data => {
      this.surplusLineNumber = data?.licenseNumber;
      this.bindingData.formBinding?.patchValue({ surplusLIneNum: this.surplusLineNumber });
      if (riskDetail.binding) {
        riskDetail.binding.surplusLIneNum = this.surplusLineNumber;
      }
    });
  }

  public refreshQuoteOptionsAndSummaryHeader(): void {
    this.quoteLimitsData.loadQuoteOptions().subscribe(data => {
      if (this.submissionData.isIssued && this.isPolicyUI) {
        this.policySummaryData.getBillingSummary();
      } else {
        const bindOptionId = 1;
        const riskCoverage = data?.riskCoverages.find(coverage => coverage.optionNumber == bindOptionId);
        this.summaryData.estimatedPremium = riskCoverage?.riskCoveragePremium?.premium;
      }
    });
  }

  isSubPhysicalDamageForm(formId: string): boolean {
    return formId === 'HiredPhysicalDamage' || formId === 'TrailerInterchangeCoverage';
  }

  raterVersionSupport(id: number = this.RATER_VERSION_SUPPORT): boolean {
    return this.submissionData?.riskDetail?.raterVersionId == null ||
      this.submissionData?.riskDetail?.raterVersionId >= id ||
      this.submissionData?.riskDetail?.raterVersionId == 0;
  }
}
