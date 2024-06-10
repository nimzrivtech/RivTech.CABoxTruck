import { Injectable } from '@angular/core';
import { take, map, mergeMap, tap, finalize, catchError, switchMap } from 'rxjs/operators';
import { forkJoin, Observable, of, Subject, throwError } from 'rxjs';
import { AbstractControl, FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { SelectItem } from '../../../../../app/shared/models/dynamic/select-item';
import { VehicleDto } from '../../../../shared/models/submission/Vehicle/VehicleDto';
import { VehicleHeaderDto } from '../../../../shared/models/submission/Vehicle/VehicleHeaderDto';
import { LvAccountCategory, LvMainUse, LvRadius } from '../../../../shared/constants/business-details.options.constants';
import { Options } from '../../../../shared/constants/vehicle-driver.options.constants';
import { VehicleService } from '../../services/vehicle.service';
import { SubmissionData } from '../submission.data';
import { ApplicantNameAddressService } from '../../services/applicant-name-address.service';
import { VehicleModel } from '../../../../shared/models/submission/Vehicle/vehicle.model';
import { LimitsService } from '../../services/coverage-limits.service';
import { PagerService } from '../../../../core/services/pager.service';
import { NameAndAddressDTO } from '../../../../shared/models/submission/applicant/applicant-name-address';
import { LimitsDefaultValues } from '../../../../shared/constants/limits-defaults.constants';
import { data } from 'jquery';
import { LimitsData } from '../limits/limits.data';
import { TableData } from '../tables.data';
import Utils from '@app/shared/utilities/utils';
import { RiskCoverageDTO } from '@app/shared/models/submission/limits/riskCoverageDto';
import { PolicyHistoryData } from '@app/modules/policy/data/policy-history.data';
import { PolicyHistoryDTO } from '@app/shared/models/policy/PolicyHistoryDto';
import { DatasetDTO } from '@app/shared/models/submission/datasetDTO';
import { ToastrService } from 'ngx-toastr';

@Injectable(
  { providedIn: 'root' }
)
export class VehicleData {
  readonly RATER_VERSION_SUPPORT = 7;
  riskDetailId: string = '';
  form: FormGroup;
  vehicleHeaderForm: FormGroup;
  vehicleDropdownsList: VehicleInformationDropDowns = new VehicleInformationDropDowns();

  vehicleInfoList: Array<VehicleModel> = new Array<VehicleModel>();
  pagedItems: VehicleModel[];
  applicant: any = {};
  primaryAddress: any = {};

  searchFilter: string;
  pager: any = {};
  sortOptions: any = {};
  loading: boolean;
  currentPage: number = 1;
  sortField: string;
  order: string;

  policyHistories: PolicyHistoryDTO[] = [];

  isVehicleUpdated: boolean = false;

  isValidVinState: boolean = false;
  isVinUpdated : boolean = false;
  
  currentTotalItems: number = 1;
  showRowsNumber: number = 10;
  isReqStatedAmount = true;

  currentSelectedVehicle: VehicleModel;

  constructor(private formBuilder: FormBuilder,
    private vehicleService: VehicleService,
    private applicantNameAddressService: ApplicantNameAddressService,
    private limitsData: LimitsData,
    private limitService: LimitsService,
    private submissionData: SubmissionData,
    private tableData: TableData,
    private policyHistoryData: PolicyHistoryData,
    private pagerService: PagerService,
    private toasterService: ToastrService ) { }

  initiateFormFields(): FormGroup {
    const defaultValueMainGVW = this.submissionData.raterVersionSupport() ? 5 : 1;

    // Business Details
    this.vehicleHeaderForm = this.formBuilder.group({
        trailerTypeLimitId: [859, Validators.required],
        mainUseDropdown: [1, Validators.required],
        mainGVW: [defaultValueMainGVW, Validators.required],
        secondaryBusinessGroupDropdown: [1, Validators.required],
        accountCategoryDropdown: [1], // Validators.required
        accountCategoryUWDropdown: [1, Validators.required],
      },
      {
        validator : [
          this.validateTrailerType,
          this.validateMainUse,
          this.validateMainGVW,
          this.validateSecondaryBusinessGroup,
          this.validateAccountCategoryUW
        ]
      });
      
    return this.vehicleHeaderForm;
  }

  validateTrailerType(form: FormGroup) {
    const formValue = form.controls['trailerTypeLimitId']?.value;

    return (formValue == null || formValue == 0) ? { 'invalidTrailerTypeLimitId': true } : null;
  }
  
  validateMainUse(form: FormGroup) {
    const formValue = form.controls['mainUseDropdown']?.value;

    return (formValue == null || formValue == 0) ? { 'invalidMainUseDropdown': true } : null;
  }

  validateMainGVW(form: FormGroup) {
    const formValue = form.controls['mainGVW']?.value;

    return (formValue == null || formValue == 0) ? { 'invalidMainGVW': true } : null;
  }

  validateSecondaryBusinessGroup(form: FormGroup) {
    const formValue = form.controls['secondaryBusinessGroupDropdown']?.value;

    return (formValue == null || formValue == 0) ? { 'invalidSecondaryBusinessGroup': true } : null;
  }

  validateAccountCategoryUW(form: FormGroup) {
    const formValue = form.controls['accountCategoryUWDropdown']?.value;

    return (formValue == null || formValue == 0) ? { 'invalidAccountCategoryUWDropdown': true } : null;
  }

  initiateFormGroup(obj: VehicleDto): FormGroup {

    this.form = this.formBuilder.group({
      id: [''],
      options: [''],
      includes: [''],
      riskDetailId: [''],
      year: ['', Validators.required],
      age: [''],
      make: ['', Validators.required],
      model: ['', Validators.required],
      vin: ['', [Validators.required, this.validateVinChars, this.validateVinLength]],
      style: [''],
      vehicleType: ['', Validators.required],
      isAutoLiability: new FormControl({ value: true, disabled: true }),
      grossVehicleWeightId: ['', Validators.required],
      grossVehicleWeightCategoryId: [0],
      vehicleDescriptionId: ['', Validators.required],
      vehicleBusinessClassId: ['', Validators.required],
      isCoverageFireTheft: [true],
      coverageFireTheftDeductibleId: [0],
      isCoverageCollision: [true],
      coverageCollisionDeductibleId: [0],
      statedAmount: [0, [Validators.required, Validators.min(1)]],
      hasCoverageCargo: [true],
      hasCoverageRefrigeration: [true],
      useClassId: ['', Validators.required],
      garagingAddressId: ['', Validators.required],
      primaryOperatingZipCode: ['', Validators.required],
      state: [''],
      registeredState: ['', Validators.required],
      radius: ['50', Validators.required],
      radiusTerritory: [null],
      autoLiabilityPremium: [null],
      cargoPremium: [null],
      physicalDamagePremium: [null],
      totalPremium: [null],
      createdDate: [''],
      createdBy: [null],
      savedVins: [['']],
      isValidated: [null],
      effectiveDate: [null],
      expirationDate: [null],
      isManualCoverageFtLimit: [null],
      isManualCollisionLimit: [null],
    }, { validators: this.validateDuplicateVin } );
    return this.form;
  }

  populateVehicleHeaderDetails() {
    const defaultValueMainGVW = this.submissionData.raterVersionSupport() ? 5 : 1;

    this.vehicleHeaderForm.controls['trailerTypeLimitId'].patchValue(this.submissionData.riskDetail.riskCoverage?.trailerTypeLimitId ?? 859);
    this.vehicleHeaderForm.controls['mainUseDropdown'].patchValue(this.submissionData.riskDetail.businessDetails?.mainUseId ?? 1);
    this.vehicleHeaderForm.controls['mainGVW'].patchValue(this.submissionData.riskDetail.businessDetails?.mainGVWId ?? defaultValueMainGVW);
    this.vehicleHeaderForm.controls['secondaryBusinessGroupDropdown'].patchValue(this.submissionData.riskDetail.businessDetails?.secondaryBusinessGroupId ?? 1);
    this.vehicleHeaderForm.controls['accountCategoryDropdown'].patchValue(this.submissionData.riskDetail.businessDetails?.accountCategoryId ?? 1);
    this.vehicleHeaderForm.controls['accountCategoryUWDropdown'].patchValue(this.submissionData.riskDetail.businessDetails?.accountCategoryUWId ?? 1);

  }

  private validateVinChars(control: AbstractControl): { [key: string]: boolean } | null {
    const pattern = new RegExp(/^[A-HJ-NPR-Za-hj-npr-z0-9]*$/);
    return !pattern.test(String(control.value)) ? { 'vinInvalidChar': true } : null;
  }

  private validateVinLength(control: AbstractControl) {
    let value = control.value;
    if (value && isNaN(+value) && value.length < 17) {
      return { 'vinInvalidLength': true };
    }
    return null;
  }
  
  
  private validateDuplicateVin(form: FormGroup) {
    const savedVins = form?.controls['savedVins'].value as Array<string>;
    const vin: string = form.controls['vin'].value ?? '' 
    const state : string = form.controls['registeredState'].value ?? ''
    let vinstate = vin + state;
      //const filteredVehicle = savedDriver.filter(s => s?.toLowerCase() !=  vinstate?.toLowerCase()?.trim());
      if (savedVins) {
        const vins = savedVins?.find(s => s?.toLowerCase() ===  vinstate?.toLowerCase()?.trim());
    return vins ? { 'vinDuplicate': true } : null;
    }
  
  } 


  public validateVinState() {
    const savedVehicles = this.submissionData?.riskDetail?.vehicles;
    const vin: string = this.form.value.vin;
    const state: string = this.form.controls['registeredState'].value ?? '';

    const filteredVehicle = savedVehicles.filter(i => i.id != this.form.value.id);
    if (filteredVehicle) {
      const vehicle = filteredVehicle?.find(s => s.vin?.toLowerCase()?.trim() === vin?.toLowerCase()?.trim() && s.registeredState === state);
      if(vehicle)
      {
        this.isValidVinState = true;
      }
      else
      {
        this.isValidVinState = false;
      }
    }
  }

  public validateAllVin( vehicleData: any) {
    const savedVehicles = this.submissionData.riskDetail.vehicles;
    const countvin = savedVehicles.filter(s => s.vin?.toLowerCase()?.trim() === vehicleData.vin?.toLowerCase()?.trim() && s.registeredState === vehicleData.state).length;
     
    var isValid = true;
    const pattern: RegExp = /[IOQ]/  
    const numPattern : RegExp = /^[0-9]/

    if(this.isEmpty(vehicleData.vin) )
    {
      isValid = false;
    }
  
    if(!this.isEmpty(vehicleData.vin))
    {
      if(!numPattern.test(vehicleData.vin) && vehicleData.vin.length < 17)
      {
        isValid = false;
      }
     
      if(pattern.test(vehicleData.vin))
      {
        isValid =false;
      }
     
      if (countvin >1)
      {
        isValid = false;
      } 
    }
  
    if (this.isEmpty(vehicleData.year))
    {
      isValid = false;
    }
    else if(this.isEmpty(vehicleData.make))
    {
      isValid = false;
    }
    else if(this.isEmpty(vehicleData.model))
    {
      isValid = false;
    }
    else if(this.isEmpty(vehicleData.vehicleDescriptionId))
    {
      isValid = false;
    }
    else if(this.isEmpty(vehicleData.businessClass) && this.isEmpty(vehicleData.vehicleBusinessClassId))
    {
      isValid = false;
    }
    else if(((vehicleData.coverageFireTheftDeductibleId != null && vehicleData.coverageFireTheftDeductibleId != 76) ||
      (vehicleData.coverageCollisionDeductibleId != null && vehicleData.coverageCollisionDeductibleId != 65)) &&
      vehicleData.statedAmount == 0)
    {
      isValid = false;
    }
  
    return isValid;
  }
  
   isEmpty(str) {
    return (!str || str.length === 0 );
  }
  
  initiateFormValues(vehicle: VehicleDto, savedVins?: String[], isViewOnly: boolean = false) {
    const model = new VehicleModel(vehicle);
    this.currentSelectedVehicle = model;
    this.riskDetailId = this.submissionData?.riskDetail?.id;
    this.form.patchValue(model);
    this.form.patchValue({ savedVins: savedVins });
   
    if (isViewOnly) {
      this.form.get('hasCoverageCargo').disable();
      this.form.get('hasCoverageRefrigeration').disable();
      this.form.get('isCoverageFireTheft').disable();
      this.form.get('isCoverageCollision').disable();
      this.form.get('year').disable();
      this.form.get('make').disable();
      this.form.get('model').disable();
      // this.form.get('vin').disable();
      this.form.get('registeredState').disable();
      this.form.get('style').disable();
      this.form.get('vehicleType').disable();
      this.form.get('grossVehicleWeightId').disable();
      this.form.get('vehicleDescriptionId').disable();
      this.form.get('vehicleBusinessClassId').disable();
      this.form.get('coverageFireTheftDeductibleId').disable();
      this.form.get('coverageCollisionDeductibleId').disable();
      this.form.get('statedAmount').disable();
      this.form.get('useClassId').disable();
      this.form.get('garagingAddressId').disable();
      this.form.get('primaryOperatingZipCode').disable();
      this.form.get('radius').disable();
      this.form.markAsUntouched();
      this.form.markAsPristine();
      return;
    } else {
      this.form.get('hasCoverageCargo').enable();
      this.form.get('hasCoverageRefrigeration').enable();
      this.form.get('isCoverageFireTheft').enable();
      this.form.get('isCoverageCollision').enable();
      this.form.get('year').enable();
      this.form.get('make').enable();
      this.form.get('model').enable();
      // this.form.get('vin').enable();
      this.form.get('registeredState').enable();
      this.form.get('style').enable();
      this.form.get('vehicleType').enable();
      this.form.get('grossVehicleWeightId').enable();
      this.form.get('vehicleDescriptionId').enable();
      this.form.get('vehicleBusinessClassId').enable();
      this.form.get('coverageFireTheftDeductibleId').enable();
      this.form.get('coverageCollisionDeductibleId').enable();
      this.form.get('statedAmount').enable();
      this.form.get('useClassId').enable();
      this.form.get('garagingAddressId').enable();
      this.form.get('primaryOperatingZipCode').enable();
      this.form.get('radius').enable();
    }

    // enable disable coverages per vehicle based on limits
    if (!this.limitsData.hasCargo) {
      this.form.get('hasCoverageCargo').setValue(false);
      this.form.get('hasCoverageCargo').disable();
    }
    if (!this.form.get('hasCoverageCargo').value || !this.limitsData.hasRefrigeration) {
      this.form.get('hasCoverageRefrigeration').setValue(false);
      this.form.get('hasCoverageRefrigeration').disable();
    } else {
      this.form.get('hasCoverageRefrigeration').enable();
      this.form.get('hasCoverageCargo').enable();
    }

    if (!this.limitsData.hasComprehensive) {
      this.form.get('isCoverageFireTheft').setValue(false);
      this.form.get('isCoverageFireTheft').disable();
    } else {
      this.form.get('isCoverageFireTheft').enable();
    }

    if (!this.limitsData.hasCollision) {
      this.form.get('isCoverageCollision').setValue(false);
      this.form.get('isCoverageCollision').disable();
    } else {
      this.form.get('isCoverageCollision').enable();
    }

    if (this.form.get('radius').value == null) {
      this.form.get('radius').setValue('50');
    }

    this.checkStatedAmountLimits();

    if (model.id) {
      this.form.markAllAsTouched();
    } else {
      this.form.markAsUntouched();
      this.form.markAsPristine();
    }
  }

  checkStatedAmountLimits() {
    console.log("checkStatedAmountLimits", this.form.get('coverageCollisionDeductibleId').value, this.form.get('coverageFireTheftDeductibleId').value);
    if (!this.form.get('isCoverageCollision').value && !this.form.get('isCoverageFireTheft').value) {
      this.isReqStatedAmount = false;
      this.form.get('statedAmount').clearValidators();
      this.form.get('statedAmount').updateValueAndValidity();
      return;
    }
  
    const hasCovCollision = this.form.get('isCoverageCollision').value == true && this.form.get('coverageCollisionDeductibleId').value != 65;
    const hasCovFireTheft = this.form.get('isCoverageFireTheft').value == true && this.form.get('coverageFireTheftDeductibleId').value != 76;
    if (hasCovCollision || hasCovFireTheft) {
      this.isReqStatedAmount = true;
      this.form.get('statedAmount').setValidators([Validators.required, Validators.min(1)]);
      this.form.get('statedAmount').updateValueAndValidity();
      return;
    }
  
    this.isReqStatedAmount = false;
    this.form.get('statedAmount').clearValidators();
    this.form.get('statedAmount').updateValueAndValidity();
  }

  populateVehicleField() {
    this.populateVehicleHeaderDetails();
    this.fetchVehicleDatasets();
    this.retrieveDropDownValues();
    this.renderVehicleList();
  }

  setPage(page: number, hasListUpdated: boolean = false) {
    let totalPages: number = 0;

    if (this.vehicleInfoList.length > 0 && page === 0) {
      page = 1;
    }

    if (page < 1) {
      return;
    }

    let vehicles = Object.assign([], this.vehicleInfoList);
    if (this.searchFilter) {
      vehicles = this.submissionData.riskDetail.vehicles.filter(v => v.vin?.trim().toLocaleLowerCase().includes(this.searchFilter)) as any[];
    }

    totalPages = Math.ceil(vehicles.length / this.showRowsNumber);

    this.currentPage = (page > totalPages) ? totalPages : 
                       (hasListUpdated) ? totalPages : page;
    this.pager = this.pagerService.getPagerMod(vehicles.length, this.currentPage, this.showRowsNumber);

    if (hasListUpdated) {
      this.pager.currentPage = this.pager.totalPages;
      this.pager.endIndex = this.pager.totalItems;
      this.pager.startIndex = ((this.pager.currentPage * this.pager.pageSize) - this.pager.pageSize);

      this.pagedItems = vehicles.slice(this.pager.startIndex, this.pager.endIndex + 1);
    } else {
      while (this.pager.totalItems <= ((this.pager.currentPage * this.pager.pageSize) - this.pager.pageSize)) {
        this.pager.currentPage--;
        this.pager.startIndex = ((this.pager.currentPage * this.pager.pageSize) - this.pager.pageSize);
      }

      this.pagedItems = vehicles.slice(this.pager.startIndex, this.pager.endIndex + 1);
    }

    this.pagedItems.forEach(vehicle => {
      if (vehicle.hasPreviousVersions != null)
        return;

      let isPolicy = this.submissionData?.riskDetail?.isPolicy
      if(isPolicy)
      {
        this.vehicleService.checkIfHasPreviousVersion$(vehicle.id).pipe(
          tap(hasPreviousVersions => {
            vehicle.hasPreviousVersions = hasPreviousVersions;
          })
        ).subscribe();
      }
      this.currentTotalItems = this.pager.totalItems;
    });
  }

  changeShowRowsNumber(page: number, value: number) {
    if (this.showRowsNumber === value) { return; }

    this.showRowsNumber = value;
    let vehicles = Object.assign([], this.vehicleInfoList);

    this.pager = this.pagerService.getPagerMod(vehicles.length, page, value);
    this.pagedItems = vehicles.slice(this.pager.startIndex, this.pager.endIndex + 1);
    this.currentPage = page;
    this.currentTotalItems = this.pager.totalItems;
  }

  uploadExcelFile(inputData?: any): Observable<any> {
    return this.vehicleService.uploadExcelFile(inputData);
  }

  downloadTemplate() {
    return this.submissionData.raterVersionSupport() ? this.vehicleService.downloadTemplateV7() : this.vehicleService.downloadTemplate();
    // return this.vehicleService.downloadTemplate();
  }

  retrieveDropDownValues() {


    return this.vehicleDropdownsList = {
      grossVehicleWeightCategoryList: [],
      vehicleDescriptionList: [],
      yearList: this.generateYearList(1960),

      vehicleTypeList: [
        { value: 'Power Unit', label: 'Power Unit' },
        { value: 'Trailer', label: 'Trailer' },
        { value: 'Other', label: 'Other' },
      ],

      grossVehicleWeightList: [
        { value: 1, label: '0 - 10,000' },
        { value: 2, label: '10,001 - 20,000' },
        { value: 3, label: '20,001 - 45,000' },
        { value: 4, label: '45,001 +' },
      ],

      grossVehicleWeightCategoryListBeforeV7: [
        { value: 1, label: 'Light' },
        { value: 2, label: 'Medium' },
        { value: 3, label: 'Heavy' },
        { value: 4, label: 'Extra Heavy (including tractors)' },
      ],

      grossVehicleWeightCategoryListV7: [
        { value: 1, label: 'Light Trucks' },
        { value: 2, label: 'Medium Trucks' },
        { value: 3, label: 'Heavy Trucks' },
        { value: 4, label: 'Extra Heavy Truck Tractors' },
      ],

      vehicleDescriptionListV7: [
        // manually sorted
        { value: 141, label: 'Agricultural Hopper Truck' },
        { value: 102, label: 'Auto Hauling Trailer' },
        { value: 142, label: 'Boom Truck/Crane Truck >26k GVW' },
        { value: 143, label: 'Boom Truck/Crane Truck 0-16k GVW' },
        { value: 144, label: 'Boom Truck/Crane Truck 16-26k GVW' },
        { value: 129, label: 'Bucket Truck/Cherry Picker 0-16k GVW' },
        { value: 130, label: 'Bucket Truck/Cherry Picker 16-26k GVW' },
        { value: 131, label: 'Bucket Truck/Cherry Picker >26K GVW' },
        { value: 98, label: 'Bulk Commodity Trailer' },
        { value: 137, label: 'Bus 9-15 passengers' },
        { value: 138, label: 'Bus 16-60 passengers' },
        { value: 139, label: 'Bus 61+ passengers' },
        { value: 133, label: 'Car Carrier 0-16 GVW' },
        { value: 140, label: 'Car Carrier >16k GVW' },
        { value: 117, label: 'Catering Truck' },
        { value: 113, label: 'Cement Mixer' },
        { value: 111, label: 'Concession Trailer' },
        { value: 38, label: 'Delivery Van>10k GVW' },
        { value: 16, label: 'Dry Freight Trailer' },
        { value: 21, label: 'Dump Body Trailer' },
        { value: 2, label: 'Dump Truck 0-16k GVW' },
        { value: 120, label: 'Dump Truck 16-33k GVW' },
        { value: 66, label: 'Dump Truck 33-45k GVW' },
        { value: 65, label: 'Dump Truck >45k GVW' },
        { value: 92, label: 'Flatbed Trailer' },
        { value: 83, label: 'Flatbed Truck 0-16 GVW' },
        { value: 121, label: 'Flatbed Truck 16-26k GVW' },
        { value: 114, label: 'Flatbed Truck >26k GVW' },
        { value: 122, label: 'Front Loader 0-45k' },
        { value: 127, label: 'Front Loader >45k GVW' },
        // { value: 8, label: 'Full size Van' }, removed
        { value: 145, label: 'Full size Van (0-8 passengers)' },
        { value: 60, label: 'Garbage Truck 0-45 GVW' },
        { value: 68, label: 'Garbage Truck >45 GVW' },
        { value: 132, label: 'Gooseneck Trailer' },
        { value: 109, label: 'Hearse' },
        { value: 95, label: 'Horse Trailer(1-2 stalls)' },
        { value: 110, label: 'Ice Cream Truck' },
        //{ value: 108, label: 'Limousine' }, removed
        { value: 146, label: 'Limousine <120 inches in Length' },
        { value: 147, label: 'Limousine 121-180 inches in Length' },
        { value: 148, label: 'Limousine >180 inches in length' },
        { value: 94, label: 'Livestock Trailer' },
        { value: 126, label: 'Logging Trailer' },
        { value: 96, label: 'Lowboy Trailer' },
        { value: 105, label: 'Luxury Auto' },
        { value: 149, label: 'Luxury Utility Vehicle' },
        { value: 91, label: 'Mini Van' },
        { value: 106, label: 'Motor Home' },
        { value: 5, label: 'Pickup<= 1/2 Ton 4x2' },
        { value: 6, label: 'Pickup<= 1/2 Ton 4x4' },
        { value: 116, label: 'Pickup >  1/2 Ton 4x2' },
        { value: 7, label: 'Pickup >  1/2 Ton 4x4' },
        { value: 119, label: 'Pickup w/ 5th Wheel 0-16k GVW' },
        { value: 93, label: 'Pole Trailer' },
        { value: 34, label: 'Private Passenger Auto' },
        { value: 134, label: 'Pump Truck(Cement) 0-16k GVW' },
        { value: 135, label: 'Pump Truck(Cement) 16-26k GVW' },
        { value: 136, label: 'Pump Truck(Cement) >26k GVW' },
        { value: 100, label: 'Rag Top Trailer' },
        { value: 22, label: 'Refrigerated Dry Freight Trailer' },
        { value: 44, label: 'Refrigerated Truck 0-16k GVW' },
        { value: 45, label: 'Refrigerated Truck 16-26k GVW' },
        { value: 46, label: 'Refrigerated Truck >26k GVW' },
        { value: 123, label: 'Roll On Vehicle 0-45 GVW' },
        { value: 128, label: 'Roll On Vehicle >45k GVW' },
        { value: 103, label: 'Sport Auto' },
        { value: 89, label: 'Stake Truck 0-16k GVW' },
        { value: 124, label: 'Stake Truck 16-26k GVW' },
        { value: 118, label: 'Stake Truck >26k GVW' },
        { value: 37, label: 'Step Van 0-10k GVW' },
        { value: 41, label: 'Straight Truck 0-16k GVW' },
        { value: 42, label: 'Straight Truck 16-26k GVW' },
        { value: 43, label: 'Straight Truck >26K GVW' },
        { value: 17, label: 'Tank Trailer' },
        { value: 90, label: 'Tank Truck <= 1400 gals' },
        { value: 125, label: 'Tank Truck > 1400 gals' },
        { value: 99, label: 'Tilt Trailer' },
        { value: 84, label: 'Tow Truck - one axle' },
        { value: 150, label: 'Tow Truck - one axle 0-26k GVW' },
        { value: 151, label: 'Tow Truck - one axle >26k GVW' },
        { value: 1, label: 'Tractor' },
        { value: 112, label: 'Travel Trailer' },
        { value: 115, label: 'TW Truck-two axle' },
        { value: 97, label: 'Unidentified Trailer' },
        { value: 101, label: 'Utility Trailer <= 1 ft.' },
        { value: 104, label: 'Utility Trailer > 12 ft' },
        // { value: 107, label: 'Utility Vehicle' }, removed
        // { value: 86, label: 'Wheelchair Bus' }, removed
        // { value: 85, label: 'Wheelchair Van' }, removed
        { value: 152, label: 'Utility Vehicle (except Luxury)' },
        { value: 153, label: 'Wedge Trailer' },
        { value: 154, label: 'Wheelchair Bus (16-60 passengers)' },
        { value: 155, label: 'Wheelchair Bus (61+ passengers)' },
        { value: 156, label: 'Wheelchair Van (0-8 passengers)' },
        { value: 157, label: 'Wheelchair Van (9-15 passengers)' },

      ],

      vehicleDescriptionListBeforeV7: [
        // manually sorted
        { value: 141, label: 'Agricultural Hopper Truck' },
        { value: 102, label: 'Auto Hauling Trailer' },
        { value: 129, label: 'Bucket Truck/Cherry Picker 0-16k GVW' },
        { value: 130, label: 'Bucket Truck/Cherry Picker 16-26k GVW' },
        { value: 131, label: 'Bucket Truck/Cherry Picker >26K GVW' },
        { value: 98, label: 'Bulk Commodity Trailer' },
        { value: 137, label: 'Bus 9-15 passengers' },
        { value: 138, label: 'Bus 16-60 passengers' },
        { value: 139, label: 'Bus 61+ passengers' },
        { value: 133, label: 'Car Carrier 0-16 GVW' },
        { value: 140, label: 'Car Carrier >16k GVW' },
        { value: 117, label: 'Catering Truck' },
        { value: 113, label: 'Cement Mixer' },
        { value: 111, label: 'Concession Trailer' },
        { value: 38, label: 'Delivery Van>10k GVW' },
        { value: 16, label: 'Dry Freight Trailer' },
        { value: 21, label: 'Dump Body Trailer' },
        { value: 2, label: 'Dump Truck 0-16k GVW' },
        { value: 120, label: 'Dump Truck 16-33k GVW' },
        { value: 66, label: 'Dump Truck 33-45k GVW' },
        { value: 65, label: 'Dump Truck >45k GVW' },
        { value: 92, label: 'Flatbed Trailer' },
        { value: 83, label: 'Flatbed Truck 0-16 GVW' },
        { value: 121, label: 'Flatbed Truck 16-26k GVW' },
        { value: 114, label: 'Flatbed Truck >26k GVW' },
        { value: 122, label: 'Front Loader 0-45k' },
        { value: 127, label: 'Front Loader >45k GVW' },
        { value: 8, label: 'Full size Van' },
        { value: 60, label: 'Garbage Truck 0-45 GVW' },
        { value: 68, label: 'Garbage Truck >45 GVW' },
        { value: 132, label: 'Gooseneck Trailer' },
        { value: 109, label: 'Hearse' },
        { value: 95, label: 'Horse Trailer(1-2 stalls)' },
        { value: 110, label: 'Ice Cream Truck' },
        { value: 108, label: 'Limousine' },
        { value: 94, label: 'Livestock Trailer' },
        { value: 126, label: 'Logging Trailer' },
        { value: 96, label: 'Lowboy Trailer' },
        { value: 105, label: 'Luxury Auto' },
        { value: 91, label: 'Mini Van' },
        { value: 106, label: 'Motor Home' },
        { value: 5, label: 'Pickup<= 1/2 Ton 4x2' },
        { value: 6, label: 'Pickup<= 1/2 Ton 4x4' },
        { value: 116, label: 'Pickup >  1/2 Ton 4x2' },
        { value: 7, label: 'Pickup >  1/2 Ton 4x4' },
        { value: 119, label: 'Pickup w/ 5th Wheel 0-16k GVW' },
        { value: 93, label: 'Pole Trailer' },
        { value: 34, label: 'Private Passenger Auto' },
        { value: 134, label: 'Pump Truck(Cement) 0-16k GVW' },
        { value: 135, label: 'Pump Truck(Cement) 16-26k GVW' },
        { value: 136, label: 'Pump Truck(Cement) >26k GVW' },
        { value: 100, label: 'Rag Top Trailer' },
        { value: 22, label: 'Refrigerated Dry Freight Trailer' },
        { value: 44, label: 'Refrigerated Truck 0-16k GVW' },
        { value: 45, label: 'Refrigerated Truck 16-26k GVW' },
        { value: 46, label: 'Refrigerated Truck >26k GVW' },
        { value: 123, label: 'Roll On Vehicle 0-45 GVW' },
        { value: 128, label: 'Roll On Vehicle >45k GVW' },
        { value: 103, label: 'Sport Auto' },
        { value: 89, label: 'Stake Truck 0-16k GVW' },
        { value: 124, label: 'Stake Truck 16-26k GVW' },
        { value: 118, label: 'Stake Truck >26k GVW' },
        { value: 37, label: 'Step Van 0-10k GVW' },
        { value: 41, label: ' Straight Truck 0-16k GVW' },
        { value: 42, label: ' Straight Truck 16-26k GVW' },
        { value: 43, label: ' Straight Truck >26K GVW' },
        { value: 17, label: 'Tank Trailer' },
        { value: 90, label: 'Tank Truck <= 1400 gals' },
        { value: 125, label: 'Tank Truck > 1400 gals' },
        { value: 99, label: 'Tilt Trailer' },
        { value: 84, label: 'Tow Truck - one axle' },
        { value: 1, label: 'Tractor' },
        { value: 112, label: 'Travel Trailer' },
        { value: 115, label: 'TW Truck-two axle' },
        { value: 97, label: 'Unidentified Trailer' },
        { value: 101, label: 'Utility Trailer <= 1 ft.' },
        { value: 104, label: 'Utility Trailer > 12 ft' },
        { value: 107, label: 'Utility Vehicle' },
        { value: 86, label: 'Wheelchair Bus' },
        { value: 85, label: 'Wheelchair Van' },
      ],

      vehicleBusinessClassList: LvAccountCategory,

      comprehensiveDeductibleList: [],

      collisionDeductibleList: [],

      useClassList: LvMainUse,

      garagingAddressList: [],
      oldGaragingAddressList: [],

      zipCodeList: [],

      optionList: Options,

      radiusList: LvRadius,

    };

  }

  
  renderVehicleList(hasVehicleListUpdated?: boolean) {
    if ((this.submissionData.riskDetail.vehicles?.length ?? 0) === 0) { this.pagedItems = []; } // Temp fix for recurring vehicle cache
    if (this.submissionData.isNew) { return; }
    const riskDetailId = this.submissionData.riskDetail?.id;
    let vehicles: any[] = [];

    if (!this.vehicleDropdownsList?.vehicleDescriptionList || this.vehicleDropdownsList.vehicleDescriptionList.length === 0) {
      this.SetVehicleDescriptionList();
    }

    Utils.blockUI();
    this.vehicleService.getByRiskDetailId(riskDetailId, false).pipe(
      take(1),
      map(data => {
        vehicles = data.filter(v => v.deletedDate == null);
      }),
      mergeMap(() => {
        const garagingAddresses = this.getGaragingAddressList();
        const deductibles = this.getDeductibles();
        return forkJoin([garagingAddresses, deductibles]);
      }),
    )
      .subscribe(data => {
        //Utils.unblockUI();
        this.applicant.zipCode = String(data[0].applicantZipCode);
        this.applicant.mainGarageAddressId = data[0].mainGarageAddressId;
        this.vehicleDropdownsList.garagingAddressList = data[0].garagingAddress;
        this.vehicleDropdownsList.zipCodeList = data[0].zipCodes.filter(this.uniqueOnly);
        this.vehicleDropdownsList.comprehensiveDeductibleList = data[1].comprehensive;
        this.vehicleDropdownsList.collisionDeductibleList = data[1].collision;
        this.vehicleInfoList = vehicles; // vehicles.sort(this.sortByVIN);
        this.submissionData.riskDetail.vehicles = this.vehicleInfoList;
        vehicles.forEach(v => {
          v = this.formatIdsToText(v);
        });
        this.vehicleInfoList = vehicles; // to update list after update in isValidated
        this.submissionData.riskDetail.vehicles = this.vehicleInfoList;
        this.setPage(this.currentPage, hasVehicleListUpdated);
        this.sortWithProperty();

        Utils.unblockUI();
      });

    this.getStateList();
  }

  renderVehicleList$(hasAdded?: boolean): Observable<any> {
    if ((this.submissionData.riskDetail.vehicles?.length ?? 0) === 0) { this.pagedItems = []; } // Temp fix for recurring vehicle cache
    if (this.submissionData.isNew) { return; }
    const riskDetailId = this.submissionData.riskDetail?.id;
    let vehicles: any[] = [];

    if (!this.vehicleDropdownsList?.vehicleDescriptionList || this.vehicleDropdownsList.vehicleDescriptionList.length === 0) {
      this.SetVehicleDescriptionList();
    }

    return this.vehicleService.getByRiskDetailId(riskDetailId, false).pipe(
      take(1),
      map(data => {
        vehicles = data.filter(v => v.deletedDate == null);
      }),
      mergeMap(() => {
        const garagingAddresses = this.getGaragingAddressList();
        const deductibles = this.getDeductibles();
        return forkJoin([garagingAddresses, deductibles]);
      }),
      tap((data) => {
        this.applicant.zipCode = String(data[0].applicantZipCode);
        this.applicant.mainGarageAddressId = data[0].mainGarageAddressId;
        this.vehicleDropdownsList.garagingAddressList = data[0].garagingAddress;
        this.vehicleDropdownsList.zipCodeList = data[0].zipCodes.filter(this.uniqueOnly);
        this.vehicleDropdownsList.comprehensiveDeductibleList = data[1].comprehensive;
        this.vehicleDropdownsList.collisionDeductibleList = data[1].collision;
        this.vehicleInfoList = vehicles; // vehicles.sort(this.sortByVIN);
        this.submissionData.riskDetail.vehicles = this.vehicleInfoList;
        vehicles.forEach(v => {
          v = this.formatIdsToText(v);
        });
        this.vehicleInfoList = vehicles; // vehicles.sort(this.sortByVIN);
        this.submissionData.riskDetail.vehicles = this.vehicleInfoList;
        this.setPage(1, hasAdded);
        this.changeShowRowsNumber(1, 10);
      }),
      switchMap(() => this.getStateList$())
    );
  }
  

  public loadVehicleFormDialogDropdowns$(): Observable<any> {
    const garagingAddresses = this.getGaragingAddressList();
    const deductibles = this.getDeductibles();
    const stateList = this.getStateList$();

    return of({}).pipe(
      mergeMap(() => forkJoin([garagingAddresses, deductibles, stateList])),
      tap((data) => {
        this.applicant.zipCode = String(data[0].applicantZipCode);
        this.applicant.mainGarageAddressId = data[0].mainGarageAddressId;
        this.vehicleDropdownsList.garagingAddressList = data[0].garagingAddress;
        this.vehicleDropdownsList.zipCodeList = data[0].zipCodes.filter(this.uniqueOnly);
        this.vehicleDropdownsList.comprehensiveDeductibleList = data[1].comprehensive;
        this.vehicleDropdownsList.collisionDeductibleList = data[1].collision;
      })
    );
  }


  renderPolicyVehicleListObservable(hasAdded?: boolean): Observable<any> {
    if ((this.submissionData.riskDetail.vehicles?.length ?? 0) === 0) { this.pagedItems = []; } // Temp fix for recurring vehicle cache
    if (this.submissionData.isNew) { return; }
    const riskDetail = this.submissionData.riskDetail;
    const riskDetailId = riskDetail?.id;
    let vehicles: any[] = [];

    if (!this.vehicleDropdownsList?.vehicleDescriptionList || this.vehicleDropdownsList.vehicleDescriptionList.length === 0) {
      this.SetVehicleDescriptionList();
    }

    return this.vehicleService.getByRiskDetailId(riskDetailId, true).pipe(
      take(1),
      map(data => {
        vehicles = data as VehicleDto[];
        vehicles.forEach((v: VehicleDto) => v.isMainRow = true);
      }),
      mergeMap(() => {
        return this.loadVehicleFormDialogDropdowns$();
      }),
      tap((data) => {

        this.vehicleInfoList = vehicles; // vehicles.sort(this.sortByVIN);
        this.submissionData.riskDetail.vehicles = this.vehicleInfoList;
        vehicles.forEach(v => {
          v = this.formatIdsToText(v);
        });

        this.vehicleInfoList = vehicles; // vehicles.sort(this.sortByVIN);
        this.submissionData.riskDetail.vehicles = this.vehicleInfoList;
        this.setPage(this.currentPage, hasAdded);
        console.log(this.vehicleDropdownsList);
      }),
    )
  }

  renderPolicyVehicleList(hasListUpdated?: boolean, isSortDisabled?: boolean) {
    if ((this.submissionData.riskDetail.vehicles?.length ?? 0) === 0) { this.pagedItems = []; } // Temp fix for recurring vehicle cache
    if (this.submissionData.isNew) { return; }
    const riskDetail = this.submissionData.riskDetail;
    const riskDetailId = riskDetail?.id;
    let vehicles: any[] = [];

    if (!this.vehicleDropdownsList?.vehicleDescriptionList || this.vehicleDropdownsList.vehicleDescriptionList.length === 0) {
      this.SetVehicleDescriptionList();
    }

    Utils.blockUI();
    this.vehicleService.getByRiskDetailId(riskDetailId, true).pipe(
      take(1),
      map(data => {
        vehicles = data as VehicleDto[];
        vehicles.forEach((v: VehicleDto) => v.isMainRow = true);
      }),
      mergeMap(() => {
        const garagingAddresses = this.getGaragingAddressList();
        const deductibles = this.getDeductibles();
        return forkJoin([garagingAddresses, deductibles]);
      })
    )
      .subscribe(data => {
        this.applicant.zipCode = String(data[0].applicantZipCode);
        this.applicant.mainGarageAddressId = data[0].mainGarageAddressId;
        this.vehicleDropdownsList.garagingAddressList = data[0].garagingAddress;
        this.vehicleDropdownsList.zipCodeList = data[0].zipCodes.filter(this.uniqueOnly);
        this.vehicleDropdownsList.comprehensiveDeductibleList = data[1].comprehensive;
        this.vehicleDropdownsList.collisionDeductibleList = data[1].collision;

        // if(!isSortDisabled)
        // {
        //   this.vehicleInfoList = vehicles.sort(this.sortByVIN);
        // }
        this.vehicleInfoList = vehicles;
        this.submissionData.riskDetail.vehicles = this.vehicleInfoList;
        this.vehicleInfoList.forEach(v => {
          v = this.formatIdsToText(v);
        });
        this.vehicleInfoList = vehicles;
        this.submissionData.riskDetail.vehicles = this.vehicleInfoList;
        this.setPage(this.currentPage, hasListUpdated);
        this.sortWithProperty();
        Utils.unblockUI();
      });

    this.getStateList();
  }

  update(vehicle: VehicleDto) {
    return this.vehicleService.put(vehicle);
  }

  insert(newVehicle: VehicleDto): Observable<VehicleDto> {
    return this.vehicleService.post(newVehicle);
  }

  insertVehicles(vehicles: VehicleDto[]): Observable<VehicleDto[]> {
    return this.vehicleService.postVehicles(vehicles);
  }

  delete(id: string, fromEndorsement?: boolean): Observable<any> {
    const observable = this.vehicleService.delete(id, fromEndorsement);
    return observable.pipe(
      tap(() => {
        this.submissionData.riskDetail.vehicles = this.submissionData.riskDetail?.vehicles.filter(t => t.id !== id && t.deletedDate == null);
      }));
  }

  deleteall(): Observable<any> {
    const riskDetailId = this.submissionData.riskDetail?.id;
    const observable = this.vehicleService.deleteall(riskDetailId);
    return observable.pipe(
      tap(() => {
        this.submissionData.riskDetail.vehicles = this.submissionData.riskDetail?.vehicles.filter(t =>  t.deletedDate == null);
      }));
  }
  
  public deleteUnboundVehicles() {
    const risk = this.submissionData?.riskDetail;
    const bindOption = Number(risk.binding?.bindOptionId) ?? 0;
    const vehicles = risk.vehicles.filter(vehicle => !vehicle.options?.split(',').includes(`${bindOption}`));
    if (vehicles?.length < 1) { return; }
    vehicles.forEach((vehicle) => {
      this.delete(vehicle.id).pipe(take(1)).subscribe(data => {
        this.vehicleInfoList = this.vehicleInfoList.filter(t => t.id !== vehicle.id);
        this.submissionData.riskDetail.vehicles = this.vehicleInfoList;
      }, (error) => {
        console.error('There was an error trying to remove a record. Please try again.');
      });
    });
  }

  reinstate(id: string, arg1: boolean): Observable<any> {
    return this.vehicleService.reinstate(id).pipe(
      tap(() => {
        this.submissionData.riskDetail.vehicles = this.submissionData.riskDetail?.vehicles.filter(t => t.id !== id && t.deletedDate == null);
      })
    );
  }

  public formatIdsToText(vehicle: any) {
    const lists = this.vehicleDropdownsList;

    const vehDescLength = this.vehicleDropdownsList?.vehicleDescriptionList?.length;

    if (vehDescLength > 0) {
      vehicle.vehicleDescriptionId = lists.vehicleDescriptionList?.find(d => d.value === vehicle.vehicleDescriptionId)?.value;
      vehicle.vehicleDescription = lists.vehicleDescriptionList?.find(vd => vd.value === vehicle.vehicleDescriptionId)?.label.trim();

    } else {
      vehicle.vehicleDescriptionId = this.submissionData.raterVersionSupport() ?
        lists.vehicleDescriptionListV7?.find(d => d.value === vehicle.vehicleDescriptionId)?.value :
        lists.vehicleDescriptionListBeforeV7?.find(d => d.value === vehicle.vehicleDescriptionId)?.value;

      vehicle.vehicleDescription = this.submissionData.raterVersionSupport() ?
        lists.vehicleDescriptionListV7?.find(vd => vd.value === vehicle.vehicleDescriptionId)?.label.trim() :
        lists.vehicleDescriptionListBeforeV7?.find(vd => vd.value === vehicle.vehicleDescriptionId)?.label.trim();

    }

    vehicle.coverageFireTheftDeductible = lists.comprehensiveDeductibleList?.find(g => g.value == vehicle.coverageFireTheftDeductibleId)?.label;
    vehicle.coverageCollisionDeductible = lists.collisionDeductibleList?.find(g => g.value == vehicle.coverageCollisionDeductibleId)?.label;
    vehicle.stateCode = lists.garagingAddressList?.find(g => g.value === vehicle.garagingAddressId)?.state;
    vehicle.isValidated = this.validateAllVin(vehicle);
    if (this.isVinUpdated || typeof vehicle.vehicleDescriptionId === 'undefined') {
      if (typeof vehicle.vehicleDescriptionId === 'undefined') vehicle.vehicleDescriptionId = null;

      // vehicle.isValidated = this.validateAllVin(vehicle.vin, vehicle.registeredState, vehicle.year, vehicle.make, vehicle.model, vehicle.vehicleDescriptionId, vehicle.businessClass, vehicle.vehicleBusinessClassId);
      this.update(vehicle)
        .pipe(finalize(() => Utils.unblockUI()))
        .subscribe(() => {
        });
    }
    
    return vehicle;
  }

  public formatVehicleIdsToText() {
    this.vehicleInfoList.forEach(v => this.formatIdsToText(v));
  }

  updateAddresses(applicant: NameAndAddressDTO) {
    const garagingAddressList = applicant.addresses?.filter(a => a.addressTypeId === 3);
    const vehiclesAddressInvalid: VehicleDto[] = [];
    this.submissionData?.riskDetail?.vehicles?.forEach(v => {
      const garagingAddressId = garagingAddressList.find(g => g.addressId === v.garagingAddressId)?.addressId;
      if (!garagingAddressId) {
        v.garagingAddressId = this.applicant.mainGarageAddressId;
      }
      // const zipCode = applicant.addresses?.find(z => String(z.address.zipCode) === v.primaryOperatingZipCode);
      // if (!zipCode) {
      //   v.primaryOperatingZipCode = String(applicant.zipCode);
      //   v.state = applicant.state;
      // }
      v.primaryOperatingZipCode = String(applicant.zipCodeOperations);
      v.state = applicant.stateOperations

      if (!garagingAddressId) {
        vehiclesAddressInvalid.push(v);
      }
      else {
        this.vehicleService.put(v).pipe(take(1)).subscribe(() => {
        }, (error) => {
          console.error('Error on saving', error);
        });
      }
    });

    if (vehiclesAddressInvalid.length) {
      this.vehicleService.putAddresses(vehiclesAddressInvalid).pipe(take(1)).subscribe(() => {
      }, (error) => {
        console.error('Error on saving', error);
      });
    }
  }

  updateOtherCoverages(oldRiskCoverage1: RiskCoverageDTO, fromEndorsement: boolean = false) {
    // if (this.limitsData.hasCargo && this.limitsData.hasRefrigeration) { return; }
    if (this.submissionData?.riskDetail?.vehicles === null || this.submissionData?.riskDetail?.vehicles?.length < 1) { return; }
    const vehicles: VehicleDto[] = [];
    const riskCoverage = this.submissionData?.riskDetail?.riskCoverage;
    console.log("updateOtherCoverages vehicles",this.submissionData?.riskDetail?.vehicles);
    this.submissionData?.riskDetail?.vehicles.forEach(v => {
      v.isEndorsement = fromEndorsement;

      /*
      const compAndFire = riskCoverage?.compDeductibleId ?? riskCoverage?.fireDeductibleId;

      if (this.limitsData.bComp !== compAndFire) {
        v.isCoverageFireTheft = this.limitsData.hasComprehensive;
      }

      if (this.limitsData.bColl !== riskCoverage?.collDeductibleId) {
        v.isCoverageCollision = this.limitsData.hasCollision;
      }
      */

      if (this.limitsData.bCargo !== riskCoverage?.cargoLimitId) {
        v.hasCoverageCargo = this.limitsData.hasCargo;
        if (!v.hasCoverageCargo) { v.hasCoverageRefrigeration = v.hasCoverageCargo; }
      }

      if (this.limitsData.bRef !== riskCoverage?.refCargoLimitId) {
        v.hasCoverageRefrigeration = this.limitsData.hasRefrigeration;
      }

      vehicles.push(v);
    });

    /*
    Utils.blockUI();
    this.vehicleService.putOtherCoverages({
      vehicles: vehicles,
      oldLimitsData: oldRiskCoverage1
    }).pipe(take(1)).subscribe(() => {
      console.log('Limits vehicle loaded');
      if (fromEndorsement) {
        this.renderPolicyVehicleList();
      } else {
        this.renderVehicleList();
      }
    }, (error) => {
      Utils.unblockUI();
      console.error('Error on saving', error);
    });
    */

    return this.vehicleService.putOtherCoverages({
      vehicles: vehicles,
      oldLimitsData: oldRiskCoverage1
    }).pipe(
      take(1),
      tap(() => {
        console.log('Limits vehicle loaded');
        if (fromEndorsement) {
          this.renderPolicyVehicleList();
        } else {
          this.renderVehicleList();
        }
      }),
      catchError((err) => throwError("Error on saving"))
    );
  }

  private get coverage(): RiskCoverageDTO {
    let coverage = this.submissionData.riskDetail?.riskCoverage;
    if (!coverage) {
      const riskCoverages = this.submissionData.riskDetail?.riskCoverages;
      if (riskCoverages) {
        coverage = riskCoverages[0];
      }
    }
    return coverage;
  }

  private generateYearList(startYear: number) {
    const currentYear = new Date().getFullYear()
    let years: SelectItem[] = [];
    startYear = startYear || 1960;
    let maxYear = currentYear + 1;
    for (var y = startYear; y <= maxYear; y++) {
      years.push({ value: y, label: String(y) });
    }
    return years.reverse();
  }

  public getStateList() {
    this.vehicleService.getStateList().subscribe(data => {
      this.vehicleDropdownsList.stateList = (data as any[]).map(d => ({ label: d.fullStateName?.replace('[E]', ''), value: d.stateCode }));
    });
  }

  public getStateList$() {
    return this.vehicleService.getStateList().pipe(tap(data => {
      this.vehicleDropdownsList.stateList = (data as any[]).map(d => ({ label: d.fullStateName?.replace('[E]', ''), value: d.stateCode }));
    }));
  }

  private getGaragingAddressList() {
    return this.applicantNameAddressService.getNameAndAddress(this.submissionData.riskDetail.id).pipe(
      take(1),
      map(
        data => {
          return {
            applicantZipCode: data.zipCode,
            mainGarageAddressId: data.addresses.find(a => a.address.isMainGarage)?.addressId,
            garagingAddress: data.addresses
              .filter(a => a.addressTypeId === 3)
              .map(a => {
                return {
                  label: `${a.address.streetAddress1}, ${a.address.city}, ${a.address.state}, ${a.address.zipCode}`,
                  value: a.addressId,
                  state: a.address.state
                }
              }),
            zipCodes: data.addresses
              .map(a => { return { label: a.address.zipCode, value: a.address.zipCode, state: a.address.state } })
          }
        }
      )
    );
  }

  getGaragingAddress(id, callbackFn?: Function) {
    this.applicantNameAddressService.getAddress(id).pipe(take(1)).subscribe((a) => {
      this.vehicleDropdownsList.oldGaragingAddressList = [{
        label: `${a.streetAddress1}, ${a.city}, ${a.state}, ${a.zipCode}`,
        value: a.id,
        state: a.state
      }];
      if (callbackFn) { callbackFn(); }
    }, (error) => {
      Utils.unblockUI();
      console.error('Error on saving', error);
    });
  }

  getPrimaryAddress() {
    return this.applicantNameAddressService.getNameAndAddress(this.submissionData.riskDetail.id).pipe(
      take(1)).subscribe((a) =>{
        this.primaryAddress.city = a.cityOperations,
        this.primaryAddress.state = a.stateOperations,
        this.primaryAddress.zipCode = a.zipCodeOperations
      }, (error) => {
        Utils.unblockUI();
        console.error('Error on getting primary address', error);
      });
  }

  private getDeductibles() {
    const state = this.submissionData.riskDetail.nameAndAddress.state;
    return this.limitService.getLimits(state, this.submissionData.riskDetail.raterVersionId).pipe(
      take(1),
      map(
        data => {
          return {
            comprehensive: data.comprehensiveLimits.map(c => { return { label: c.limitDisplay, value: c.id } }),
            collision: data.collisionLimits.map(c => { return { label: c.limitDisplay, value: c.id } })
          }
        }
      )
    );
  }

  private uniqueOnly(item: any, i: number, array: any[]) {
    return array.findIndex(t => t.value === item.value) === i;
  }

  private withCurreny(value: string) {
    return value === 'No Coverage' ? value : value;
  }

  private sortByVIN(cv1, cv2) {
    if (cv1.vin > cv2.vin) return 1;
    if (cv1.vin < cv2.vin) return -1;
    return 0;
  }

  public setDefaults(vehicle?: any) {
    const coverage = this.submissionData.getCoverage(this.limitsData.isVehicle);
    vehicle = new VehicleDto(vehicle);
    
    const compFireDeductibleId = coverage?.compDeductibleId ?? coverage?.fireDeductibleId;
    if (compFireDeductibleId && compFireDeductibleId > LimitsDefaultValues.compId) {
      vehicle.coverageFireTheftDeductibleId = compFireDeductibleId;
      vehicle.isCoverageFireTheft = true;
      vehicle.coverageFireTheft = "Yes";
    }
    else
    {
      vehicle.isCoverageFireTheft = false;
      vehicle.coverageFireTheft = "No";
    }

    if (coverage?.collDeductibleId && coverage?.collDeductibleId > LimitsDefaultValues.colId) {
      vehicle.coverageCollisionDeductibleId = coverage.collDeductibleId;
      vehicle.isCoverageCollision = true;
      vehicle.coverageCollision = "Yes";
    }
    else
    {
      vehicle.isCoverageCollision = false;
      vehicle.coverageCollision = "No";
    }
    vehicle.hasCoverageCargo = coverage?.cargoLimitId > LimitsDefaultValues.cargoId;
    vehicle.hasCoverageRefrigeration = coverage?.refCargoLimitId > LimitsDefaultValues.refId;
    vehicle.vehicleBusinessClassId = this.vehicleHeaderForm.controls['accountCategoryUWDropdown']?.value != 0 ? this.vehicleHeaderForm.controls['accountCategoryUWDropdown']?.value : null;
    vehicle.useClassId = this.vehicleHeaderForm.controls['mainUseDropdown']?.value != 0 ? this.vehicleHeaderForm.controls['mainUseDropdown']?.value : null;
    vehicle.garagingAddressId = this.applicant?.mainGarageAddressId;
    vehicle.primaryOperatingZipCode = this.primaryAddress?.zipCode;
    //vehicle.state = this.vehicleDropdownsList.zipCodeList.find(z => z.value == vehicle.primaryOperatingZipCode)?.state;
    vehicle.state = this.primaryAddress?.state;
    vehicle.registeredState = this.submissionData.riskDetail?.nameAndAddress?.state;
    //commented out to unlink applicant Main GVW to Vehicle
    // vehicle.grossVehicleWeightId = this.submissionData.riskDetail?.businessDetails?.mainGVWId;
    vehicle.radius = 50;

    return vehicle;
  }

  get getOptionQuote(): string {
    const opt = this.submissionData?.riskDetail?.riskCoverages?.map(x => x.optionNumber)?.toString();
    return !((opt ?? '').length) ? '1' : opt;
  }

  public onExitVehicleScreen(callbackFn: Function) {
    const option = (this.submissionData.isIssued) ? this.submissionData.riskDetail?.binding?.bindOptionId ?? 1 : 1;
    const data = new VehicleHeaderDto({
      riskDetailId: this.submissionData.riskDetail.id,
      optionNumber: Number(option),
      trailerTypeLimitId: (this.vehicleHeaderForm.value.trailerTypeLimitId && this.limitsData.isTrailerType) || this.vehicleHeaderForm.value.trailerTypeLimitId == 0 ? parseInt(this.vehicleHeaderForm.value.trailerTypeLimitId) : null,
      mainUseId: this.vehicleHeaderForm.value.mainUseDropdown || this.vehicleHeaderForm.value.mainUseDropdown == 0 ? parseInt(this.vehicleHeaderForm.value.mainUseDropdown) : null,
      mainGVWId: this.vehicleHeaderForm.value.mainGVW || this.vehicleHeaderForm.value.mainGVW == 0 ? parseInt(this.vehicleHeaderForm.value.mainGVW) : null,
      secondaryBusinessGroupId: ((this.vehicleHeaderForm.value.secondaryBusinessGroupDropdown || this.vehicleHeaderForm.value.secondaryBusinessGroupDropdown == 0) && (this.submissionData.riskDetail.raterVersionId == null || this.submissionData.riskDetail.raterVersionId > 6)) ? parseInt(this.vehicleHeaderForm.value.secondaryBusinessGroupDropdown) : null,
      accountCategoryId: this.vehicleHeaderForm.value.accountCategoryDropdown || this.vehicleHeaderForm.value.accountCategoryDropdown == 0 ? parseInt(this.vehicleHeaderForm.value.accountCategoryDropdown) : null,
      accountCategoryUWId: this.vehicleHeaderForm.value.accountCategoryUWDropdown || this.vehicleHeaderForm.value.accountCategoryUWDropdown == 0 ? parseInt(this.vehicleHeaderForm.value.accountCategoryUWDropdown) : null,
    });

    if (this.vehicleHeaderForm.dirty) {
      this.triggerHeaderUpdate(data).then((vehicleHeader) => {
        this.submissionData.riskDetail.riskCoverage.trailerTypeLimitId = data.trailerTypeLimitId;
        this.submissionData.riskDetail.businessDetails.mainUseId = data.mainUseId;
        this.submissionData.riskDetail.businessDetails.mainGVWId = data.mainGVWId;
        this.submissionData.riskDetail.businessDetails.secondaryBusinessGroupId = data.secondaryBusinessGroupId;
        this.submissionData.riskDetail.businessDetails.accountCategoryId = data.accountCategoryId;
        this.submissionData.riskDetail.businessDetails.accountCategoryUWId = data.accountCategoryUWId;
      }).catch((error) => {
          this.vehicleHeaderForm.markAsPristine();
          this.toasterService.error(error.message);
      }).finally(() => {
        if (callbackFn) { callbackFn(); }
      });
    }
  }

  public addUpdateHeader(obj: VehicleHeaderDto): Observable<VehicleHeaderDto | any> {
    return this.vehicleService.saveVehicleHeader(obj);
  }

  public triggerHeaderUpdate(vHeaderUpdate: VehicleHeaderDto): Promise<VehicleHeaderDto | any> {
    // tslint:disable-next-line: no-shadowed-variable
    const promise = new Promise((resolve, reject) => {
      try {
        this.addUpdateHeader(vHeaderUpdate).pipe(take(1)).subscribe(data => {
          if (data.message) {
            reject(data);
          }

          resolve(data);
        }, (error) => {
        });
      } catch {
      }
    });
    return promise;
  }

  public saveVehicleHeader(callBackFn?: Function) {
    const option = (this.submissionData.isIssued) ? this.submissionData.riskDetail?.binding?.bindOptionId ?? 1 : 1;
    const data = new VehicleHeaderDto({
      riskDetailId: this.submissionData.riskDetail.id,
      optionNumber: Number(option),
      trailerTypeLimitId: (this.vehicleHeaderForm.value.trailerTypeLimitId && this.limitsData.isTrailerType)? parseInt(this.vehicleHeaderForm.value.trailerTypeLimitId) : null,
      mainUseId: this.vehicleHeaderForm.value.mainUseDropdown || this.vehicleHeaderForm.value.mainUseDropdown == 0 ? parseInt(this.vehicleHeaderForm.value.mainUseDropdown) : null,
      mainGVWId: this.vehicleHeaderForm.value.mainGVW || this.vehicleHeaderForm.value.mainGVW == 0 ? parseInt(this.vehicleHeaderForm.value.mainGVW) : null,
      secondaryBusinessGroupId: ((this.vehicleHeaderForm.value.secondaryBusinessGroupDropdown || this.vehicleHeaderForm.value.secondaryBusinessGroupDropdown == 0) && (this.submissionData.riskDetail.raterVersionId == null || this.submissionData.riskDetail.raterVersionId > 6)) ? parseInt(this.vehicleHeaderForm.value.secondaryBusinessGroupDropdown) : null,
      accountCategoryId: this.vehicleHeaderForm.value.accountCategoryDropdown || this.vehicleHeaderForm.value.accountCategoryDropdown == 0 ? parseInt(this.vehicleHeaderForm.value.accountCategoryDropdown) : null,
      accountCategoryUWId: this.vehicleHeaderForm.value.accountCategoryUWDropdown || this.vehicleHeaderForm.value.accountCategoryUWDropdown == 0 ? parseInt(this.vehicleHeaderForm.value.accountCategoryUWDropdown) : null,
    });

    this.triggerHeaderUpdate(data).then((vehicleHeader) => {
      this.submissionData.riskDetail.riskCoverage.trailerTypeLimitId = data.trailerTypeLimitId;
      this.submissionData.riskDetail.businessDetails.mainUseId = data.mainUseId;
      this.submissionData.riskDetail.businessDetails.mainGVWId = data.mainGVWId;
      this.submissionData.riskDetail.businessDetails.secondaryBusinessGroupId = data.secondaryBusinessGroupId;
      this.submissionData.riskDetail.businessDetails.accountCategoryId = data.accountCategoryId;
      this.submissionData.riskDetail.businessDetails.accountCategoryUWId = data.accountCategoryUWId;
    }).catch((obj) => {
    }).finally(() => {
      if (callBackFn) { callBackFn(); }
    });
  }

  sort(targetElement) {
    if (targetElement instanceof HTMLElement) {
      const property = targetElement.getAttribute('data-name');
      if (property === 'coverageFireTheftDeductible' || property === 'coverageCollisionDeductible') {
        targetElement.setAttribute('data-type', 'text');
      }
    }
    let vehicles = Object.assign([], this.vehicleInfoList);
    let sortData = this.tableData.sortPagedData(targetElement, vehicles, this.currentPage, this.showRowsNumber);
    this.sortOptions = sortData.sortOptions;
    this.sortField = sortData.sortOptions.property;
    this.order = sortData.sortOptions.order;
    this.pagedItems = sortData.pagedItems;
    this.vehicleInfoList = sortData.sortedData;
  }

  sortWithProperty() {
    let vehicles = Object.assign([], this.vehicleInfoList);
    let sortData = this.tableData.sortPagedDataWithProperty(this.sortField, this.order, vehicles, this.currentPage, this.showRowsNumber);
    this.sortOptions = sortData.sortOptions;
    this.sortField = sortData.sortOptions.property;
    this.order = sortData.sortOptions.order;
    this.pagedItems = sortData.pagedItems;
    
    Utils.blockUI();
    this.pagedItems.forEach(vehicle => {
      if (vehicle.hasPreviousVersions != null) {
        Utils.unblockUI();
        return;
      }

      let isPolicy = this.submissionData?.riskDetail?.isPolicy
      if(isPolicy)
      {
        this.vehicleService.checkIfHasPreviousVersion$(vehicle.id).pipe(
          tap(hasPreviousVersions => {
            vehicle.hasPreviousVersions = hasPreviousVersions;
          })
        ).subscribe(() => Utils.unblockUI());
      }
    });
    this.vehicleInfoList = sortData.sortedData;
  }

  setDefaultSort(){
    this.sortField = 'createdDate';
    this.order = 'asc';

    this.sortWithProperty();
  }

  loadPreviousVehicle(vehicle: VehicleDto): Observable<any> {
    
    if (!this.vehicleDropdownsList?.vehicleDescriptionList || this.vehicleDropdownsList.vehicleDescriptionList.length === 0) {
      this.SetVehicleDescriptionList();
    }
    
    return this.vehicleService.getPreviousVehicles(vehicle).pipe(
      tap((data: VehicleDto[]) => {
        data.forEach(v => {
          this.formatIdsToText(v);
          // remove amounts
          v.autoLiabilityPremium = null;
          v.physicalDamagePremium = null;
          v.cargoPremium = null;
          v.totalPremium = null;
          v.proratedPremium = null;
        });
        vehicle.previousVehicles = data;
      })
    );
  }

  GetPolicyHistories() {
    this.policyHistories = [];
    this.policyHistoryData.getEndorsementHistory().subscribe((data: PolicyHistoryDTO[]) => {
      this.policyHistories = data;
    });
  }


  public cleanDeductibleValues(vehicle: VehicleModel): VehicleModel {
    if (vehicle.isCoverageFireTheft !== true) {
      vehicle.coverageFireTheftDeductibleId = null;
    }

    if (vehicle.isCoverageCollision !== true) {
      vehicle.coverageCollisionDeductibleId = null;
    }

    return vehicle;
  }


  recalculateAge(selectedYear: any): string | number {
    const effectiveDate = this.submissionData?.riskDetail?.brokerInfo?.effectiveDate ?? new Date();
    let age = selectedYear && selectedYear > 0 ? (new Date(effectiveDate)).getFullYear() - selectedYear : '';
    age = age < 0 ? 0 : age;
    return age;
  }


  get reportHeaders() {
    return {
      vin: 'VIN',
      year: 'Year',
      make: 'Make',
      model: 'Model',
      registeredState: 'State',
      statedAmount: 'TIV (stated Amount)',
      vehicleType: 'Vehicle Type',
      vehicleDescriptionId: 'Vehicle Description',
      vehicleBusinessClassId: 'Vehicle Business Class',
      grossVehicleWeightId: 'Gross Vehicle Weight (lbs)',
      radius: 'Vehicle Radius',
      isCoverageFireTheft: 'Comprehensive / Fire & Theft (Yes or No)',
      isCoverageCollision: 'Collision (Yes or No)',
      coverageFireTheftDeductibleId: 'Comprehensive / Fire & Theft Deductible',
      coverageCollisionDeductibleId: 'Collision Deductible',
      hasCoverageCargo: 'Cargo ( Yes or No)',
      hasCoverageRefrigeration: 'Refrigeration (Yes or No)'
    };
  }

  get policyReportHeaders() {
    return {
      vin: 'VIN',
      year: 'Year',
      make: 'Make',
      model: 'Model',
      registeredState: 'State',
      statedAmount: 'TIV (stated Amount)',
      vehicleType: 'Vehicle Type',
      vehicleDescriptionId: 'Vehicle Description',
      vehicleBusinessClassId: 'Vehicle Business Class',
      grossVehicleWeightId: 'Gross Vehicle Weight (lbs)',
      radius: 'Vehicle Radius',
      isCoverageFireTheft: 'Comprehensive / Fire & Theft (Yes or No)',
      isCoverageCollision: 'Collision (Yes or No)',
      coverageFireTheftDeductibleId: 'Comprehensive / Fire & Theft Deductible',
      coverageCollisionDeductibleId: 'Collision Deductible',
      hasCoverageCargo: 'Cargo ( Yes or No)',
      hasCoverageRefrigeration: 'Refrigeration (Yes or No)',
      effectiveDate: 'Effective Date',
      expirationDate: 'Expiration Date'

    };
  }

  raterVersionSupport(id: number = this.RATER_VERSION_SUPPORT): boolean {
    const raterVersionId = this.submissionData.riskDetail?.raterVersionId ?? this.RATER_VERSION_SUPPORT;
    // console.log(raterVersionId, "submission.data.ts");
    return raterVersionId == null || raterVersionId >= id || raterVersionId == 0;
  }
  checkVehicleDescriptionIfSupported(desc: string) {

    let vehicleDescriptionId;
    const isVehicleDescriptionSupported = this.vehicleDropdownsList?.vehicleDescriptionList?.find(d => d.label.toLowerCase() === desc?.trim()?.toLowerCase())?.value;

    if (isVehicleDescriptionSupported) {
      vehicleDescriptionId = isVehicleDescriptionSupported;
    }
    else {
      vehicleDescriptionId = null;
    }
    return vehicleDescriptionId;
  }

  checkVehicleDescriptionIdIfSupported(descId: number) {

    let vehicleDescriptionId;
    const isVehicleDescriptionSupported = this.vehicleDropdownsList?.vehicleDescriptionList?.find(d => d.value === descId)?.value;

    if (isVehicleDescriptionSupported) {
      vehicleDescriptionId = isVehicleDescriptionSupported;
    }
    else {
      vehicleDescriptionId = null;
    }
    return vehicleDescriptionId;
  }

  public fetchVehicleDatasets() {
    this.SetVehicleGVWList();
    this.SetVehicleDescriptionList();
  }

  public fetchVehicleGVW() {
    const isSupported = this.raterVersionSupport();
    return this.vehicleService.getVehicleGVWList(isSupported);
  }

  public fetchVehicleDescription() {
    const isSupported = this.raterVersionSupport();
    return this.vehicleService.getVehicleDescriptionList(isSupported);
  }

  private SetVehicleGVWList() {
    this.fetchVehicleGVW().subscribe(data => {
      this.vehicleDropdownsList.grossVehicleWeightCategoryList = (data as DatasetDTO[]).map(d => ({ label: d.descriptionInUI, value: d.value }));
    });
  }

  private SetVehicleDescriptionList() {
    this.fetchVehicleDescription().subscribe(data => {
      this.vehicleDropdownsList.vehicleDescriptionList = (data as DatasetDTO[]).map(d => ({ label: d.descriptionInUI, value: d.value }));
    });
  }

  // saveVehicleHeader(): Observable<any> {
  //   const option = (this.submissionData.isIssued) ? this.submissionData.riskDetail?.binding?.bindOptionId ?? 1 : 1;
  //   const data = new VehicleHeaderDto({
  //     riskDetailId: this.submissionData.riskDetail.id,
  //     optionNumber: Number(option),
  //     trailerTypeLimitId: (this.vehicleHeaderForm.value.trailerTypeLimitId && this.limitsData.isTrailerType)? parseInt(this.vehicleHeaderForm.value.trailerTypeLimitId) : null,
  //     mainUseId: this.vehicleHeaderForm.value.mainUseDropdown || this.vehicleHeaderForm.value.mainUseDropdown == 0 ? parseInt(this.vehicleHeaderForm.value.mainUseDropdown) : null,
  //     mainGVWId: this.vehicleHeaderForm.value.mainGVW || this.vehicleHeaderForm.value.mainGVW == 0 ? parseInt(this.vehicleHeaderForm.value.mainGVW) : null,
  //     secondaryBusinessGroupId: ((this.vehicleHeaderForm.value.secondaryBusinessGroupDropdown || this.vehicleHeaderForm.value.secondaryBusinessGroupDropdown == 0) && (this.submissionData.riskDetail.raterVersionId == null || this.submissionData.riskDetail.raterVersionId > 6)) ? parseInt(this.vehicleHeaderForm.value.secondaryBusinessGroupDropdown) : null,
  //     accountCategoryId: this.vehicleHeaderForm.value.accountCategoryDropdown || this.vehicleHeaderForm.value.accountCategoryDropdown == 0 ? parseInt(this.vehicleHeaderForm.value.accountCategoryDropdown) : null,
  //     accountCategoryUWId: this.vehicleHeaderForm.value.accountCategoryUWDropdown || this.vehicleHeaderForm.value.accountCategoryUWDropdown == 0 ? parseInt(this.vehicleHeaderForm.value.accountCategoryUWDropdown) : null,
  //   });

  //   const observable = this.vehicleService.saveVehicleHeader(data);
  //   return observable.pipe(
  //     tap(() => {
  //       this.submissionData.riskDetail.riskCoverage.trailerTypeLimitId = data.trailerTypeLimitId;
  //       this.submissionData.riskDetail.businessDetails.mainUseId = data.mainUseId;
  //       this.submissionData.riskDetail.businessDetails.mainGVWId = data.mainGVWId;
  //       this.submissionData.riskDetail.businessDetails.secondaryBusinessGroupId = data.secondaryBusinessGroupId;
  //       this.submissionData.riskDetail.businessDetails.accountCategoryId = data.accountCategoryId;
  //       this.submissionData.riskDetail.businessDetails.accountCategoryUWId = data.accountCategoryUWId;
  //     }));
  // }
}

export class VehicleInformationDropDowns {
  yearList: SelectItem[];
  vehicleTypeList: SelectItem[];
  grossVehicleWeightList: SelectItem[];
  grossVehicleWeightCategoryListBeforeV7: SelectItem[];
  grossVehicleWeightCategoryListV7: SelectItem[];
  vehicleDescriptionListV7: SelectItem[];
  vehicleDescriptionListBeforeV7: SelectItem[];
  vehicleBusinessClassList: SelectItem[];
  comprehensiveDeductibleList: SelectItem[];
  collisionDeductibleList: SelectItem[];
  useClassList: SelectItem[];
  zipCodeList: any[];
  garagingAddressList: any[];
  oldGaragingAddressList: any[];
  optionList: SelectItem[];
  stateList?: SelectItem[];
  radiusList: SelectItem[];
  grossVehicleWeightCategoryList: SelectItem[];
  vehicleDescriptionList: SelectItem[];
}
function withDeleted(riskDetailId: string, withDeleted: any, arg2: boolean) {
  throw new Error('Function not implemented.');
}



