import { Injectable } from '@angular/core';
import { FormArray, FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { DatePipe } from '@angular/common';
import { SelectItem } from '../../../shared/models/dynamic/select-item';
import { GeneralLiabilityCargoDto } from '../../../shared/models/submission/riskspecifics/general-liability-cargoDto';
import { MaintenanceQuestionDto } from '../../../shared/models/submission/riskspecifics/maintenance-safety/maintenance-questionDto';
import { MaintenanceSafetyDto } from '../../../shared/models/submission/riskspecifics/maintenance-safety/maintenance-safetyDto';
import { RiskDTO } from '../../../shared/models/risk/riskDto';
import { ToastrService } from 'ngx-toastr';
import { take, tap } from 'rxjs/operators';
import Utils from '../../../shared/utilities/utils';
import { SubmissionData } from './submission.data';
import { RiskSpecificMaintenanceSafetyService } from '../services/riskspecific-maintenance-safety.service';
import { RiskSpecificGeneralLiabilityCargoService } from '../services/riskspecific-general-liability-cargo.service';
import { LimitsDefaultValues } from '../../../shared/constants/limits-defaults.constants';
import { RiskSpecificsConstants } from '../../../shared/constants/risk-specifics/risk-specifics.constants';
import { PagerService } from '../../../core/services/pager.service';
import { ErrorMessageConstant } from '../../../shared/constants/error-message.constants';
import { of } from 'rxjs';

export interface RiskSpecificsForms {
  generalInformationForm?: FormGroup;
  driverHiringCriteriaForm?: FormGroup;
  driverInformationForm?: FormGroup;
  maintenanceForm?: FormGroup;
  safetyDevicesForm?: FormGroup;
  cargoForm?: FormGroup;
  commoditiesForm?: FormGroup;
  generalLiabilityForm?: FormGroup;
  underwritingQuestionsForm?: FormGroup;
}
export interface RiskSpecificsDropDowns {
  safetyMeetingsList?: SelectItem[];
  garagingTypeList?: SelectItem[];
  vehicleMaintenanceList?: SelectItem[];
  companyPracticeList?: SelectItem[];
  safetyDeviceCategoryList?: SelectItem[];
  commoditiesList?: SelectItem[];
}

@Injectable(
  { providedIn: 'root'}
)
export class RiskSpecificsData {
  public riskSpecificsForms: RiskSpecificsForms;
  public riskSpecificsDropdownsList: RiskSpecificsDropDowns;
  public radiusOfOperationNotEqualToHundred: boolean = false;
  private _generalInformationForm: FormGroup;
  commoditiesHauled: any[] = [];
  hasPhysicalDamageCoverage: boolean;
  hasGLCoverage: boolean;
  hasGLCargoCoverage: boolean;
  hasGLRefrigerationCoverage: boolean;
  isInitialMaintenanceAndSatefy: boolean = true;
  isInitialGeneralLiabAndCargo: boolean = true;
  isEitherGLAndMTCDisabled: boolean = false;

  pager: any = {};
  currentPage: number = 0;
  pagedItems: any[];
  loading: boolean;

  constructor(
    private formBuilder: FormBuilder,
    public datepipe: DatePipe,
    private toastr: ToastrService,
    public submissionData: SubmissionData,
    private maintenanceSafetyService: RiskSpecificMaintenanceSafetyService,
    private pagerService: PagerService,
    private generalLiabilityCargoService: RiskSpecificGeneralLiabilityCargoService
  ) { }

  initiateFormFields(risk: RiskDTO) {
    this.riskSpecificsForms = {};
    this.isEitherGLAndMTCDisabled = false;

    this.riskSpecificsForms.generalInformationForm = this.formBuilder.group({
      submissionType: new FormControl(''),
      requestedQuoteDate: new FormControl('', [Validators.required]),
      effectiveDate: new FormControl('', [Validators.required]),
      expirationDate: new FormControl('', [Validators.required]),
      isNewVenture: new FormControl(''),
      hoursOfService: new FormControl(null),
      daysOfService: new FormControl(null),
      shiftsPerDay: new FormControl(null),
      zeroToFifty: new FormControl(null, [Validators.required]),
      fiftyToHundred: new FormControl(null, [Validators.required]),
      twoHundredPlus: new FormControl(null, [Validators.required]),
      anyOperationalExposureOver300RadiusOperation: new FormControl('NO', [Validators.required]),
      percentageOfVehiclesOwnerOperated: new FormControl(null, [Validators.required])
    });

    this.riskSpecificsForms.driverInformationForm = this.formBuilder.group({
      totalDrivers: new FormControl(null, [Validators.required]),
      transportationOperation: new FormControl(null),
      isBusinessPrincipalDriverOnPolicy: new FormControl(true, [Validators.required]),
      vehicleOperatedForPersonalUse: new FormControl(false, [Validators.required]),
      volunteersUsedInBusiness: new FormControl(false, [Validators.required]),
      percentageOfStaff: new FormControl(null, [Validators.required]),
      hireFromOthersForUse: new FormControl(false),
      annualCostOfHireFromOthersForUse: new FormControl(null, [Validators.required]),
      hireFromOthersWithoutDriver: new FormControl(false),
      annualCostOfHireFromOthersWithoutDriver: new FormControl(null, [Validators.required]),
      leaseToOthersForUse: new FormControl(false),
      annualIncomeDerivedUse: new FormControl(null, [Validators.required]),
      leaseToOthersWithoutDriver: new FormControl(false),
      annualIncomeDerivedLease: new FormControl(null, [Validators.required]),
      assumedLiabilityByContractAgreement: new FormControl(false, [Validators.required]),
      driversTakeVehiclesHome: new FormControl(false, [Validators.required]),
      vehiclesOwnedByApplicant: new FormControl(true, [Validators.required]),
      addOrDeleteVehiclesDuringPolicyTerm: new FormControl(null, [Validators.required]),
      decsribeAddOrDeleteVehiclesDuringPolicyTerm: new FormControl(null, [Validators.required]),
      pastYearDriverHired: new FormControl(null, [Validators.required]),
      pastYearDriverTerminated: new FormControl(null, [Validators.required]),
      currentMVRspulled: new FormControl(null, [Validators.required]),
      driversConsideredIndependentContractors: new FormControl(null, [Validators.required])
    });

    this.riskSpecificsForms.driverHiringCriteriaForm = this.formBuilder.group({
      driversDriveSimilarVehicleCommerciallyForMoreThan2Years: new FormControl(null, [Validators.required]),
      driversHave5YearsDrivingExperience: new FormControl(null, [Validators.required]),
      agreeToReportAllDriversToRivington: new FormControl(true, [Validators.required]),
      familyMembersUnder21PrimaryDriversOfCompanyAuto: new FormControl(false, [Validators.required]),
      vehicleOperatedForPersonalUse: new FormControl(false, [Validators.required]),
      driversProperlyLicensedDOTcompliant: new FormControl(true, [Validators.required]),
      isDisciplinaryPlanDocumentedDrivers: new FormControl(false, [Validators.required]),
      isThereDriverIncentiveProgram: new FormControl(false, [Validators.required]),
      backgroundCheckIncludes: new FormControl(''),
      driverTrainingIncludes: new FormControl(null)
    });

    this.riskSpecificsForms.underwritingQuestionsForm = this.formBuilder.group({
      provideWorkersCompensationForAllEmployees: new FormControl(false),
      workersCompensationCarrier: new FormControl(''),
      provideWorkersCompensationForAllEmployeesExplain: new FormControl('', Validators.required),
      equipmentOperatedUnderApplicantsAuthorityScheduledOnApplicantsDriverVehicleSchedule: new FormControl(false),
      equipmentOperatedUnderApplicantsAuthorityScheduledOnApplicantsDriverVehicleScheduleExplain: new FormControl('', Validators.required),
      inPast4yearsInsuranceObtainedAssignedRiskPlan: new FormControl(false),
      inPast4yearsInsuranceObtainedAssignedRiskPlanExplain: new FormControl(''),
      companyProvidedNoticeOfCancellationNonRenewalOrCanceledRefusedRenew: new FormControl(false),
      companyProvidedNoticeOfCancellationNonRenewalOrCanceledRefusedRenewExplain: new FormControl(''),
      filedOrContemplatedFilingBankruptcy: new FormControl(false),
      filedOrContemplatedFilingBankruptcyExplain: new FormControl(''),
      operatingAuthoritySuspendedRevoked: new FormControl(false),
      operatingAuthoritySuspendedRevokedExplain: new FormControl(''),
      confirm: new FormControl('', [Validators.required])
    });

    let data;
    if (risk?.maintenanceSafety?.maintenanceQuestion) {
      data = risk.maintenanceSafety.maintenanceQuestion;
      if (data.garagingType) {
        data.garagingType.forEach(type => {
          type.label = this.riskSpecificsDropdownsList.garagingTypeList.filter(x => x.value === type.value)[0].label;
        });
      }
      if (data.vehicleMaintenance) {
        data.vehicleMaintenance.forEach(type => {
          type.label = this.riskSpecificsDropdownsList.vehicleMaintenanceList.filter(x => x.value === type.value)[0].label;
        });
      }
      if (data.companyPractice) {
        data.companyPractice.forEach(type => {
          type.label = this.riskSpecificsDropdownsList.companyPracticeList.filter(x => x.value === type.value)[0].label;
        });
      }
    }
    this.riskSpecificsForms.maintenanceForm = this.formBuilder.group({
      safetyMeetings: [this.setInitialValue(data?.safetyMeetings, null)],
      personInCharge: [this.setInitialValue(data?.personInCharge, '')],
      garagingType: [this.setInitialValue(data?.garagingType, null)],
      vehicleMaintenance: [this.setInitialValue(data?.vehicleMaintenance, null), Validators.required],
      isMaintenanceProgramManagedByCompany: [this.setInitialValue(data?.isMaintenanceProgramManagedByCompany, true), Validators.required],
      provideCompleteMaintenanceOnVehicles: [this.setInitialValue(data?.provideCompleteMaintenanceOnVehicles, true), Validators.required],
      areDriverFilesAvailableForReview: [this.setInitialValue(data?.areDriverFilesAvailableForReview, true), Validators.required],
      areAccidentFilesAvailableForReview: [this.setInitialValue(data?.areAccidentFilesAvailableForReview, true), Validators.required],
      companyPractice: [this.setInitialValue(data?.companyPractice, null), Validators.required],
    });

    if (risk?.maintenanceSafety?.maintenanceQuestion) {
      data = risk.maintenanceSafety.safetyDevices;
      if (data.safetyDeviceCategory) {
        data.safetyDeviceCategory.forEach(type => {
          type.label = this.riskSpecificsDropdownsList.safetyDeviceCategoryList.filter(x => x.value === type.value)[0].label;
        });
      }
    }
    this.riskSpecificsForms.safetyDevicesForm = this.formBuilder.group({
      safetyDeviceCategory: [this.setInitialValue(data?.safetyDeviceCategory, null), Validators.required],
      safetyDevice: new FormArray([])
    });
    if (risk?.maintenanceSafety?.safetyDevices) {
      const formValue = this.riskSpecificsForms.safetyDevicesForm.get('safetyDevice') as FormArray;
      data.safetyDevice.forEach(value => {
        const form = this.safetyDeviceForm(null);
        form.patchValue(value);
        form.patchValue({categoryLabel: this.riskSpecificsDropdownsList.safetyDeviceCategoryList.find(s => s.value === value.safetyDeviceCategoryId)?.label});
        formValue.push(form);
      });
      this.riskSpecificsForms.safetyDevicesForm.setControl('safetyDevice', formValue);
      // this.riskSpecificsForms.safetyDevicesForm.get('safetyDevice').patchValue(data.safetyDevice);
    }

    if (risk?.generalLiabilityCargo?.generalLiability) { data = risk.generalLiabilityCargo.generalLiability; }
    this.riskSpecificsForms.generalLiabilityForm = this.formBuilder.group({
      contractuallyRequiredToCarryGeneralLiabilityInsurance: [this.setInitialValue(data?.contractuallyRequiredToCarryGeneralLiabilityInsurance, false), Validators.required],
      haveOperationsOtherThanTrucking: [this.setInitialValue(data?.haveOperationsOtherThanTrucking, false), Validators.required],
      haveOperationsOtherThanTruckingExplain: [this.setInitialValue(data?.haveOperationsOtherThanTruckingExplain, '')],
      operationsAtStorageLotOrImpoundYard: [this.setInitialValue(data?.operationsAtStorageLotOrImpoundYard, false), Validators.required],
      anyStorageOfGoods: [this.setInitialValue(data?.anyStorageOfGoods, false), Validators.required],
      anyWarehousing: [this.setInitialValue(data?.anyWarehousing, false), Validators.required],
      anyStorageOfVehiclesForOthers: [this.setInitialValue(data?.anyStorageOfVehiclesForOthers, false), Validators.required],
      anyLeasingSpaceToOthers: [this.setInitialValue(data?.anyLeasingSpaceToOthers, false), Validators.required],
      anyFreightForwarding: [this.setInitialValue(data?.anyFreightForwarding, false), Validators.required],
      anyStorageOfFuelsAndOrChemicals: [this.setInitialValue(data?.anyStorageOfFuelsAndOrChemicals, false), Validators.required],
      hasApplicantHadGeneralLiabilityLoss: [this.setInitialValue(data?.hasApplicantHadGeneralLiabilityLoss, false)],
      hasApplicantHadGeneralLiabilityLossExplain: [this.setInitialValue(data?.hasApplicantHadGeneralLiabilityLossExplain, '')],
      glAnnualPayroll: [this.setInitialValue(data?.glAnnualPayroll, 0), [Validators.min(1)]],
    });

    if (risk?.generalLiabilityCargo?.cargo) { data = risk.generalLiabilityCargo.cargo; }
    this.riskSpecificsForms.cargoForm = this.formBuilder.group({
      areRequiredToCarryCargoInsurance: [this.setInitialValue(data?.areRequiredToCarryCargoInsurance, false), Validators.required],
      requireRefrigerationBreakdownCoverage: [this.setInitialValue(data?.requireRefrigerationBreakdownCoverage, false), Validators.required],
      haulHazardousMaterials: [this.setInitialValue(data?.haulHazardousMaterials, false), Validators.required],
      haveRefrigeratedUnits: [this.setInitialValue(data?.haveRefrigeratedUnits, false), Validators.required],
      areCommoditiesStoredInTruckOvernight: [this.setInitialValue(data?.areCommoditiesStoredInTruckOvernight, false), Validators.required],
      areCommoditiesStoredInTruckOvernightExplain: [this.setInitialValue(data?.areCommoditiesStoredInTruckOvernightExplain, '')],
      hadCargoLoss: [this.setInitialValue(data?.hadCargoLoss, false), Validators.required],
      hadCargoLossExplain: [this.setInitialValue(data?.hadCargoLossExplain, '')],
    });

    this.pagedItems = [];
    this.commoditiesHauled = risk?.generalLiabilityCargo?.commoditiesHauled ?? [];

    this.riskSpecificsForms.commoditiesForm = this.formBuilder.group({
      id: [''],
      riskDetailId: [''],
      commodity: ['', Validators.required],
      percentageOfCarry: ['', [Validators.required, Validators.min(0.1)]],
      totalPercentageOfCarry: ['']
    }, { validators: this.validatePercentageOfCarry });

    this.subscribeGeneralInformationForm();
  }

  populateMaintenanceSafetyForm() {
    const risk = this.submissionData.riskDetail;
    let data;
    if (risk?.maintenanceSafety?.maintenanceQuestion) {
      data = risk.maintenanceSafety.maintenanceQuestion;
    }
    this.riskSpecificsForms.maintenanceForm = this.formBuilder.group({
      safetyMeetings: [this.setInitialValue(data?.safetyMeetings, null)],
      personInCharge: [this.setInitialValue(data?.personInCharge, '')],
      garagingType: [this.setInitialValue(data?.garagingType, null)],
      vehicleMaintenance: [this.setInitialValue(data?.vehicleMaintenance, null), Validators.required],
      isMaintenanceProgramManagedByCompany: [this.setInitialValue(data?.isMaintenanceProgramManagedByCompany, true), Validators.required],
      provideCompleteMaintenanceOnVehicles: [this.setInitialValue(data?.provideCompleteMaintenanceOnVehicles, true), Validators.required],
      areDriverFilesAvailableForReview: [this.setInitialValue(data?.areDriverFilesAvailableForReview, true), Validators.required],
      areAccidentFilesAvailableForReview: [this.setInitialValue(data?.areAccidentFilesAvailableForReview, true), Validators.required],
      companyPractice: [this.setInitialValue(data?.companyPractice, null), Validators.required],
    });

    if (risk?.maintenanceSafety?.maintenanceQuestion) {
      data = risk.maintenanceSafety.safetyDevices;
    }
    this.riskSpecificsForms.safetyDevicesForm = this.formBuilder.group({
      safetyDeviceCategory: [this.setInitialValue(data?.safetyDeviceCategory, null), Validators.required],
      safetyDevice: new FormArray([])
    });
    if (risk?.maintenanceSafety?.safetyDevices) {
      const formValue = this.riskSpecificsForms.safetyDevicesForm.get('safetyDevice') as FormArray;
      data.safetyDevice.forEach(value => {
        const form = this.safetyDeviceForm(null);
        form.patchValue(value);
        formValue.push(form);
      });
      this.riskSpecificsForms.safetyDevicesForm.setControl('safetyDevice', formValue);
      // this.riskSpecificsForms.safetyDevicesForm.get('safetyDevice').patchValue(data.safetyDevice);
    }
  }

  populateCommoditiesHauled() {
    this.pagedItems = [];
    this.commoditiesHauled = this.submissionData.riskDetail?.generalLiabilityCargo?.commoditiesHauled ?? [];
  }

  populateGeneralLiabilityForm() {
    this.riskSpecificsForms.generalLiabilityForm.patchValue(this.submissionData.riskDetail?.generalLiabilityCargo?.generalLiability);
  }

  setBusinessRules() {
    let coverage = this.submissionData.riskDetail?.riskCoverage;
    if (!coverage) {
      const riskCoverages = this.submissionData.riskDetail?.riskCoverages;
      if (riskCoverages) {
        coverage = riskCoverages[0];
      }
    }
    this.hasPhysicalDamageCoverage = coverage?.compDeductibleId > LimitsDefaultValues.compId || coverage?.compDeductibleSymbolId > 0
      || coverage?.fireDeductibleId > LimitsDefaultValues.compId || coverage?.fireDeductibleSymbolId > 0
      || coverage?.collDeductibleId > LimitsDefaultValues.colId || coverage?.collDeductibleSymbolId > 0;

    this.hasGLCoverage = coverage?.glbiLimitId != undefined && coverage?.glbiLimitId != LimitsDefaultValues.glId && coverage?.glbiLimitId != LimitsDefaultValues.glv7Id;
    this.hasGLCargoCoverage = coverage?.cargoLimitId > LimitsDefaultValues.cargoId;
    this.hasGLRefrigerationCoverage = coverage?.refCargoLimitId > LimitsDefaultValues.refId;

  }

  get hiddenNavItems(): string[] {
    this.setBusinessRules();
    const defaultHiddenNavItem: string[] = this.hasGLCoverage || this.hasGLCargoCoverage || this.hasGLRefrigerationCoverage ? [] : ['General Liability & Cargo'];
    if (this.submissionData?.riskDetail?.businessDetails?.yearsInBusiness < 1) {
      defaultHiddenNavItem.push('Historical Coverage'); // append to array
      defaultHiddenNavItem.push('Claims History'); // append to array
    }
    return defaultHiddenNavItem;
  }

  get shouldShowCargoSection(): boolean {
    return this.hasGLCargoCoverage || this.hasGLRefrigerationCoverage;
  }

  retrieveDropDownValues() {
    this.riskSpecificsDropdownsList = {};

    // maintenance section
    this.riskSpecificsDropdownsList.safetyMeetingsList = [
      { label: 'Weekly', value: 1 },
      { label: 'Bi-Weekly', value: 2 },
      { label: 'Monthly', value: 3 },
      { label: 'Quarterly', value: 4 },
      { label: 'Semi-Annually', value: 5 },
      { label: 'Annually', value: 6 },
      { label: 'Does Not Occur', value: 7 },
    ];

    this.riskSpecificsDropdownsList.garagingTypeList = [
      { label: 'Indoor', value: 1 },
      { label: 'Outdoor', value: 2 },
      { label: 'Fenced', value: 3 },
      { label: 'Lighted', value: 4 },
      { label: 'Security Guard', value: 5 },
    ];

    this.riskSpecificsDropdownsList.vehicleMaintenanceList = [
      { label: 'A service record for each vehicle', value: 1 },
      { label: 'Controlled and frequent inspections', value: 2 },
      { label: 'Vehicle daily condition reports', value: 3 },
    ];

    this.riskSpecificsDropdownsList.companyPracticeList = [
      { label: 'Written maintenance program', value: 1 },
      { label: 'Written driver-training program', value: 2 },
      { label: 'Written safety program', value: 3 },
      { label: 'Written accident reporting procedures', value: 4 }
    ];

    // safety device section
    this.riskSpecificsDropdownsList.safetyDeviceCategoryList = [
      { label: 'None', value: 1 },
      { label: 'Cameras', value: 2 },
      { label: 'Geographic Driving History Data', value: 3 },
      { label: 'Mileage Tracking Device', value: 4 },
      { label: 'Brake Warning System', value: 5 },
      { label: 'Distracted Driving Warning System', value: 6 },
      { label: 'Speed Warning System', value: 7 },
      { label: 'Monitoring of Harsh Braking/Speeding/Accelerating via Telematics Device', value: 8 },
      { label: 'Any Active Accident Avoidance Technology', value: 9 },
      { label: 'Any Passive Accident Avoidance Technology', value: 10 }

    ];

    this.riskSpecificsDropdownsList.commoditiesList = [
      { label: 'BAKED GOODS', value: 1 },
      { label: 'BEER WINE (NO LIQUOR)', value: 2 },
      { label: 'BEVERAGES', value: 3 },
      { label: 'CANNED GOODS', value: 4 },
      { label: 'DAIRY', value: 5 },
      { label: 'EGGS', value: 6 },
      { label: 'FLOUR', value: 7 },
      { label: 'FOOD (FROZEN/NOT SEAFOOD)', value: 8 },
      { label: 'FRUITS', value: 9 },
      { label: 'ICE CREAM', value: 10 },
      { label: 'MEATS / DRESSED POULTRY', value: 11 },
      { label: 'OILS (EDIBE)', value: 12 },
      { label: 'SALT', value: 13 },
      { label: 'SEAFOOD (FRESH)', value: 14 },
      { label: 'SEAFOOD (FROZEN)', value: 15 },
      { label: 'SPICES', value: 16 },
      { label: 'TEA/COFFEE', value: 17 },
      { label: 'VEGETABLES', value: 18 },
      { label: 'OTHER FOOD & BEVERAGES', value: 19 },
      { label: 'COTTON (NON-GINNED)', value: 20 },
      { label: 'HIDE & SKINS', value: 21 },
      { label: 'RAW FURS', value: 22 },
      { label: 'TEXTILES', value: 23 },
      { label: 'OTHER TEXTILES/SKINS/FURS', value: 24 },
      { label: 'FEED', value: 25 },
      { label: 'FERTILIZER', value: 26 },
      { label: 'FLOWERS (CUT)', value: 27 },
      { label: 'GRAIN', value: 28 },
      { label: 'HAY', value: 29 },
      { label: 'MULCH / TOP SOIL AND FILL', value: 30 },
      { label: 'PLANTS/SHRUBS/TREES (NOT TEMP CONTROLLED)', value: 31 },
      { label: 'PLANTS/SHRUBS/TREES(TEMP CONTROLLED)', value: 32 },
      { label: 'SEEDS', value: 33 },
      { label: 'OTHER FARMING/AGRICULTURE', value: 34 },
      { label: 'CATTLE', value: 35 },
      { label: 'POULTRY (LIVE)', value: 36 },
      { label: 'SWINE', value: 37 },
      { label: 'OTHER LIVESTOCK', value: 38 },
      { label: 'RAW TOBACCO LEAF (BULK)', value: 39 },
      { label: 'CHEMICALS - NON ACIDIC', value: 40 },
      { label: 'CHEMICALS - NON CORROSIVE', value: 41 },
      { label: 'CHEMICALS - NON FLAMMABLE/ NON EXPLOSIVE', value: 42 },
      { label: 'CLEANING SUPPLIES & COMPOUNDS', value: 43 },
      { label: 'DYES, INK AND PAINTS, NON-HAZARDOUS', value: 44 },
      { label: 'LIQUIDS - NON CHEMICAL OR NON PETROLEUM', value: 45 },
      { label: 'OTHER CHEMICALS', value: 46 },
      { label: 'ALUMINUM', value: 47 },
      { label: 'COAL', value: 48 },
      { label: 'IRON (RAW)', value: 49 },
      { label: 'METAL PRODUCTS', value: 50 },
      { label: 'MINERALS, NON-PRECIOUS', value: 51 },
      { label: 'ORE', value: 52 },
      { label: 'STEEL', value: 53 },
      { label: 'ZINC', value: 54 },
      { label: 'OTHER METALS/COAL', value: 55 },
      { label: 'COILED STEEL', value: 56 },
      { label: 'SCRAP METAL', value: 57 },
      { label: 'ASPHALT', value: 58 },
      { label: 'CEMENT (BULK)', value: 59 },
      { label: 'CEMENT (DRY AND BAGGED)', value: 60 },
      { label: 'GRAVEL & ROCK', value: 61 },
      { label: 'MARBLE/GRANITE/OTHER STONE SLABS', value: 62 },
      { label: 'SAND', value: 63 },
      { label: 'STONE', value: 64 },
      { label: 'OTHER CONSTRUCTION MATERIALS(RAW)', value: 65 },
      { label: 'LOGS', value: 66 },
      { label: 'BOTTLES (PLASTIC)', value: 67 },
      { label: 'CONTAINERIZED FREIGHT', value: 68 },
      { label: 'GLASS PRODUCTS', value: 69 },
      { label: 'PACKING MATERIALS & SUPPLIES', value: 70 },
      { label: 'PAPER AND PAPER PRODUCTS', value: 71 },
      { label: 'PLASTIC PRODUCTS', value: 72 },
      { label: 'PRINTED MATERIAL', value: 73 },
      { label: 'RUBBER PRODUCTS (NOT TIRES)', value: 74 },
      { label: 'OTHER PAPER/PLASTIC/GLASS', value: 75 },
      { label: 'RECYCLING MATERIALS', value: 76 },
      { label: 'APPLIANCES', value: 77 },
      { label: 'CARPET (NOT ORIENTAL)', value: 78 },
      { label: 'CASKETS', value: 79 },
      { label: "CD'S/DVD'S/VIDEO GAMES/TAPES", value: 80 },
      { label: 'CLOTHING & SHOES (NON-DESIGNER)', value: 81 },
      { label: 'COSMETICS/PERFUME', value: 82 },
      { label: 'DEPT. STORE MDSE', value: 83 },
      { label: 'FURNITURE (NEW)', value: 84 },
      { label: 'MUSICAL INSTRUMENTS', value: 85 },
      { label: 'OFFICE EQUIPMENT', value: 86 },
      { label: 'PHARMACEUTICALS (OVER THE COUNTER)', value: 87 },
      { label: 'SPAS/HOT TUBS', value: 88 },
      { label: 'SPORTING GOODS', value: 89 },
      { label: 'TOILET & SOAP PRODUCTS', value: 90 },
      { label: 'TOYS', value: 91 },
      { label: 'OTHER CONSUMER GOODS', value: 92 },
      { label: 'HOUSEHOLD GOODS (MOVER)', value: 93 },
      { label: 'AIRCRAFT ENGINES', value: 94 },
      { label: 'AIRCRAFT PARTS (NOT ENGINES)', value: 95 },
      { label: 'AUTO ACCESSORIES/PARTS (NOT TIRES)', value: 96 },
      { label: 'AUTOMOBILES', value: 97 },
      { label: 'RECREATIONAL VEHICLES/GOLF CARTS', value: 98 },
      { label: 'TIRES', value: 99 },
      { label: 'OTHER AUTOS/AIRCRAFTS', value: 100 },
      { label: 'TRAVEL/TRAILERS', value: 101 },
      { label: 'BOATS', value: 102 },
      { label: 'COMMUNICATIONS EQPMNT', value: 103 },
      { label: 'CONSTRUCTION EQUIPMENT', value: 104 },
      { label: 'ELECTRICAL EQUIPMENT', value: 105 },
      { label: 'MACHINERY', value: 106 },
      { label: 'MEDICAL INSTRUMENTS', value: 107 },
      { label: 'OILFIELD EQUIPMENT', value: 108 },
      { label: 'SCIENTIFIC INSTRUMENTS & EQUIPMENT', value: 109 },
      { label: 'OTHER MACHINERY/EQUIPMENT', value: 110 },
      { label: 'ELECTRICAL SUPPLIES', value: 111 },
      { label: 'LUMBER', value: 112 },
      { label: 'PIPE', value: 113 },
      { label: 'PLUMBING SUPPLIES', value: 114 },
      { label: 'TOOLS', value: 115 },
      { label: 'WIRE (NOT FIBER OPTIC)', value: 116 },
      { label: 'WOOD PRODUCTS (NOT FURNITURE & CASKETS)', value: 117 },
      { label: 'OTHER BUILDING SUPPLIES', value: 118 },
      { label: 'MOBILE/MODULAR HOMES', value: 119 },
      { label: 'ANIMAL BY PRODUCTS', value: 120 },
      { label: 'RESINS', value: 121 },
      { label: 'OTHER MISC.', value: 122 },
      { label: 'GARBAGE', value: 123 },
      { label: 'BUSINESS DOCUMENTS/NON-NEGOTIABLE SECURITIES', value: 124 }


    ];
  }

  private setInitialValue(obj: any, val: any) {
    return (obj == null || obj.length === 0) ? val : obj;
  }

  private subscribeGeneralInformationForm() {
    this._generalInformationForm = this.riskSpecificsForms.generalInformationForm;
    // on input of effectiveDate, set expirationDate = effectiveDate + 1yr
    this._generalInformationForm.get('effectiveDate').valueChanges.subscribe(effectiveDate => {
      const expirationDate = new Date(effectiveDate);
      expirationDate.setFullYear(expirationDate.getFullYear() + 1);
      this._generalInformationForm.get('expirationDate').setValue(this.datepipe.transform(expirationDate, 'yyyy-MM-dd'));
    });
    // validate radiusOperation fields == 100
    this._generalInformationForm.get('zeroToFifty').valueChanges.subscribe(() => this.checkIfRadiusOfOperationEqualToHundred());
    this._generalInformationForm.get('fiftyToHundred').valueChanges.subscribe(() => this.checkIfRadiusOfOperationEqualToHundred());
    this._generalInformationForm.get('twoHundredPlus').valueChanges.subscribe(() => this.checkIfRadiusOfOperationEqualToHundred());
  }

  private checkIfRadiusOfOperationEqualToHundred() {
    this.radiusOfOperationNotEqualToHundred =
      (this._generalInformationForm.get('zeroToFifty').value +
        this._generalInformationForm.get('fiftyToHundred').value +
        this._generalInformationForm.get('twoHundredPlus').value
      ) !== 100;
  }

  getGenLiabilityCargoFormValue(): GeneralLiabilityCargoDto {
    const generalLiabilityCargo = new GeneralLiabilityCargoDto();
    const glForm = this.riskSpecificsForms.generalLiabilityForm;
    const cargoForm = this.riskSpecificsForms.cargoForm;
    if (!glForm.get('haveOperationsOtherThanTrucking').value) {
      glForm.get('haveOperationsOtherThanTruckingExplain').setValue('');
    }
    if (!glForm.get('hasApplicantHadGeneralLiabilityLoss').value) {
      glForm.get('hasApplicantHadGeneralLiabilityLossExplain').setValue('');
    }
    if (!cargoForm.get('areCommoditiesStoredInTruckOvernight').value) {
      cargoForm.get('areCommoditiesStoredInTruckOvernightExplain').setValue('');
    }
    if (!cargoForm.get('hadCargoLoss').value) {
      cargoForm.get('hadCargoLossExplain').setValue('');
    }
    generalLiabilityCargo.generalLiability = glForm.value;
    generalLiabilityCargo.cargo = cargoForm.value;
    return generalLiabilityCargo;
  }

  resetGeneralLiabilityCargo() {
    const sectionIds: number[] = [];
    const generalLiabilityCargo = this.submissionData.riskDetail.generalLiabilityCargo;
    if (!generalLiabilityCargo) { return; }
    this.isInitialGeneralLiabAndCargo = false; // to trigger validation when updating only the limits
    if (!this.hasGLCoverage) { // && generalLiabilityCargo.generalLiability
      sectionIds.push(RiskSpecificsConstants.sections.generalLiability);
      generalLiabilityCargo.generalLiability = null;
    }
    if (!this.hasGLCargoCoverage && !this.hasGLRefrigerationCoverage && generalLiabilityCargo.cargo) {
      sectionIds.push(RiskSpecificsConstants.sections.cargo);
      generalLiabilityCargo.cargo = null;
      generalLiabilityCargo.commoditiesHauled = [];
      this.commoditiesHauled = [];
    }

    const riskDetailId = this.submissionData.riskDetail?.id;
    this.generalLiabilityCargoService.deleteGeneralLiabilityCargo(riskDetailId, sectionIds).subscribe(() => {
      if (generalLiabilityCargo.generalLiability === null ) { this.riskSpecificsForms?.generalLiabilityForm?.reset(); }
      if (generalLiabilityCargo.cargo === null) { this.riskSpecificsForms?.cargoForm?.reset(); }
    });
  }

  resetGeneralLiabilityCargo$() {
    const sectionIds: number[] = [];
    const generalLiabilityCargo = this.submissionData.riskDetail.generalLiabilityCargo;

    if (!generalLiabilityCargo) { return of(); }

    this.isInitialGeneralLiabAndCargo = false; // to trigger validation when updating only the limits
    if (!this.hasGLCoverage) { // && generalLiabilityCargo.generalLiability
      sectionIds.push(RiskSpecificsConstants.sections.generalLiability);
      generalLiabilityCargo.generalLiability = null;
    }
    if (!this.hasGLCargoCoverage && !this.hasGLRefrigerationCoverage && generalLiabilityCargo.cargo) {
      sectionIds.push(RiskSpecificsConstants.sections.cargo);
      generalLiabilityCargo.cargo = null;
      generalLiabilityCargo.commoditiesHauled = [];
      this.commoditiesHauled = [];
    }

    const riskDetailId = this.submissionData.riskDetail?.id;
    return this.generalLiabilityCargoService.deleteGeneralLiabilityCargo(riskDetailId, sectionIds).pipe(
      tap(() => {
        if (generalLiabilityCargo.generalLiability === null ) { this.riskSpecificsForms?.generalLiabilityForm?.reset(); }
        if (generalLiabilityCargo.cargo === null) { this.riskSpecificsForms?.cargoForm?.reset(); }
      })
    );
  }

  resetGeneralLiabilityAndCargoIfDisabledInBindOption(bindOption: number) {
    const sectionIds: number[] = [];
    const riskDetailId = this.submissionData.riskDetail?.id;
    const riskCoverage = this.submissionData.riskDetail?.riskCoverages?.find(x => x.optionNumber == bindOption);
    const generalLiabilityCargo = this.submissionData.riskDetail?.generalLiabilityCargo;
    const hasGLCoverage = Number(riskCoverage?.glbiLimitId ?? 87) !== 87;
    const hasCargoAndRefBrkdown = Number(riskCoverage?.cargoLimitId ?? 53) !== 53 || Number(riskCoverage?.refCargoLimitId ?? 157) !== 157;

    if (!generalLiabilityCargo) { return; }
    if (!hasGLCoverage && generalLiabilityCargo.generalLiability) {
      sectionIds.push(RiskSpecificsConstants.sections.generalLiability);
      generalLiabilityCargo.generalLiability = null;
    }
    if (!hasCargoAndRefBrkdown && generalLiabilityCargo.cargo) {
      sectionIds.push(RiskSpecificsConstants.sections.cargo);
      generalLiabilityCargo.cargo = null;
      generalLiabilityCargo.commoditiesHauled = [];
      this.commoditiesHauled = [];
    }


    this.generalLiabilityCargoService.deleteGeneralLiabilityCargo(riskDetailId, sectionIds).subscribe(() => {
      this.isEitherGLAndMTCDisabled = true;
    });
  }

  getMaintenanceSafetyFormValue(): MaintenanceSafetyDto {
    const maintenanceSafety = new MaintenanceSafetyDto();
    maintenanceSafety.maintenanceQuestion = this.riskSpecificsForms.maintenanceForm.value;
    maintenanceSafety.safetyDevices = this.riskSpecificsForms.safetyDevicesForm.value;
    return maintenanceSafety;
  }

  safetyDeviceForm(item: SelectItem) {
    return new FormGroup({
      safetyDeviceCategoryId: new FormControl(item?.value),
      categoryLabel: new FormControl(item?.label),
      isInPlace: new FormControl(false, [Validators.required]),
      unitDescription: new FormControl(''),
      yearsInPlace: new FormControl(''),
      percentageOfFleet: new FormControl('')
    });
  }

  saveMaintenanceSafety() {
    Utils.blockUI();
    const maintenanceSafetyData = this.getMaintenanceSafetyFormValue();
    if (this.submissionData?.riskDetail?.vehicles?.length < 6) {
      maintenanceSafetyData.maintenanceQuestion = new MaintenanceQuestionDto();
    }
    if (!this.hasPhysicalDamageCoverage) {
      maintenanceSafetyData.maintenanceQuestion.garagingType = [];
    }
    maintenanceSafetyData.maintenanceQuestion = this.ensureNullForEmpty(maintenanceSafetyData.maintenanceQuestion);
    maintenanceSafetyData.safetyDevices = this.ensureNullForEmpty(maintenanceSafetyData.safetyDevices, ['safetyDeviceCategory']);
    maintenanceSafetyData.riskDetailId = this.submissionData.riskDetail.id;
    this.maintenanceSafetyService.saveRiskSpecificMaintenanceSafety(maintenanceSafetyData).pipe(take(1)).subscribe(data => {
      Utils.unblockUI();
      this.submissionData.riskDetail.maintenanceSafety = maintenanceSafetyData;
      console.log('Maintenance Question & Safety Device saved!');
    }, (error) => {
      Utils.unblockUI();
      this.toastr.error(ErrorMessageConstant.savingErrorMsg);
    });
  }

  saveGeneralLiabilityCargo() {
    Utils.blockUI();
    const cargoData = this.getGenLiabilityCargoFormValue();
    cargoData.generalLiability = this.ensureNullForEmpty(cargoData.generalLiability);
    cargoData.cargo = this.ensureNullForEmpty(cargoData.cargo);

    cargoData.riskDetailId = this.submissionData.riskDetail.id;
    cargoData.commoditiesHauled = this.commoditiesHauled;
    this.generalLiabilityCargoService.saveGeneralLiabilityCargo(cargoData).pipe(take(1)).subscribe(data => {
      Utils.unblockUI();
      this.submissionData.riskDetail.generalLiabilityCargo = cargoData;
      console.log('General Liability & Cargo data saved!');
    }, (error) => {
      Utils.unblockUI();
      this.toastr.error(ErrorMessageConstant.savingErrorMsg);
    });
  }

  get isGLCargoPageValid() {
    let isValid = true;
    if (this.hasGLCoverage) {
      isValid = this.riskSpecificsForms.generalLiabilityForm.valid;
    }
    if (this.hasGLCargoCoverage || this.hasGLRefrigerationCoverage) {
      isValid = isValid && this.riskSpecificsForms.cargoForm.valid && this.commoditiesHauled.length > 0;
    }
    return isValid;
  }

  private validatePercentageOfCarry(form: FormGroup) {
    let totalPercentageOfTravel: number;
    const percentageOfTravel = form.controls['percentageOfCarry'].value;
    const currentPercentageOfTravel = form.controls['totalPercentageOfCarry'].value;

    totalPercentageOfTravel = +(Number(currentPercentageOfTravel) + Number(percentageOfTravel)).toFixed(1);

    return totalPercentageOfTravel > 100.0 ? { 'invalidPercentageOfCarry': true } : null;
  }

  private ensureNullForEmpty(object: any, properties?: string[]) {
    const iterator = properties ? properties : Object.keys(object);
    iterator.forEach(prop => {
      if (object[prop] === '' || (Array.isArray(object[prop]) && object[prop].length === 0)) {
        object[prop] = null;
      }
    });
    return object;
  }

  setPage(page: number, data: any[], isNew: boolean = false) {
    if (page < 1) {
      return;
    }

    this.pager = this.pagerService.getPager(data.length, page);

    if (isNew) {
      this.currentPage = this.pager.currentPage = this.pager.totalPages;
      this.pager.endIndex = this.pager.totalItems;
      this.pager.startIndex = ((this.pager.currentPage * this.pager.pageSize) - this.pager.pageSize);
    } else {
      while (this.pager.totalItems <= ((this.pager.currentPage * this.pager.pageSize) - this.pager.pageSize)) {
        this.pager.currentPage--;
        this.pager.startIndex = ((this.pager.currentPage * this.pager.pageSize) - this.pager.pageSize);
      }
      this.currentPage = page;
    }

    this.pagedItems = data.slice(this.pager.startIndex, this.pager.endIndex + 1);
  }

}
