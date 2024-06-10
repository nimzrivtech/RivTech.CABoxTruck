import { Component, OnInit, ViewChild } from '@angular/core';
import { Observable, Subject } from 'rxjs';
import { finalize, take, switchMap } from 'rxjs/operators';
import { BsModalRef } from 'ngx-bootstrap';
import { DataTableDirective } from 'angular-datatables';

import Utils from '../../../../../../../shared/utilities/utils';
import NotifUtils from '../../../../../../../shared/utilities/notif-utils';
import { VehicleDto } from '../../../../../../../shared/models/submission/Vehicle/VehicleDto';
import { VehicleTextConstants } from '../../../../../../../shared/constants/vehicle.text.constants';
import { VehicleModel } from '../../../../../../../shared/models/submission/Vehicle/vehicle.model';
import { RiskCoverageDTO } from '../../../../../../../shared/models/submission/limits/riskCoverageDto';
import { ElementRef } from '@angular/core';
import { HttpEventType } from '@angular/common/http';
import { TableConstants } from '../../../../../../../shared/constants/table.constants';
import { VehicleData } from '../../../../../../../modules/submission/data/vehicle/vehicle.data';
import { SubmissionData } from '../../../../../../../modules/submission/data/submission.data';
import { LimitsData } from '../../../../../../../modules/submission/data/limits/limits.data';
import { PolicySummaryData } from '@app/modules/policy/data/policy-summary.data';
import { RaterApiData } from '@app/modules/submission/data/rater-api.data';
import { ToastrService } from 'ngx-toastr';
import { takeUntil } from 'rxjs/operators';
import { VehicleService } from '@app/modules/submission/services/vehicle.service';
import { CurrencyPipe,formatDate } from '@angular/common';
import { BaseClass } from 'app/shared/base-class';
import { ReportUtils } from '@app/shared/utilities/report.utils';
import { LvAccountCategory, LvGVWCategoryBeforeV7, LvGVWCategoryV7, LvMainUse, LvSecondaryBusinessGroup } from '@app/shared/constants/business-details.options.constants';
import { ApplicantData, ApplicantInformationDropDowns } from '@app/modules/submission/data/applicant/applicant.data';

@Component({
  selector: 'app-vehicle-information',
  templateUrl: './vehicle-information.component.html',
  styleUrls: ['./vehicle-information.component.scss']
})
export class VehicleInformationComponent extends BaseClass implements OnInit {
  lists: ApplicantInformationDropDowns;
  mainUseList = LvMainUse;
  disableTrailerType: boolean = true;

  accountCategoryList = LvAccountCategory;

  modalButton: string = 'Add Vehicle';
  modalRef: BsModalRef | null;
  VehicleTexts = VehicleTextConstants;
  tableHeaders: any[] = [];
  excelVehicles: VehicleDto[];
  hideMe: boolean = false;
  dtElement: DataTableDirective;
  dtOptions: DataTables.Settings = {};
  dtTrigger: Subject<any> = new Subject();
  isDtInitialized: boolean = false;
  @ViewChild('btnAddVehicle') btnAddVehicle: ElementRef;
  @ViewChild('btnAddVehiclesFromExcel') btnAddVehiclesFromExcel: ElementRef;
  @ViewChild('fileSelection') fileSelection: ElementRef;
  TableConstants = TableConstants;

  constructor(public vehicleData: VehicleData,
    public submissionData: SubmissionData,
    public limitsData: LimitsData,
    public policySummaryData: PolicySummaryData,
    public raterApiData: RaterApiData,
    private vehicleService: VehicleService,
    private currencyPipe: CurrencyPipe,
    public applicantData: ApplicantData,
  ) 
  { 
    super(); 
  }

  ngOnInit() {
    Utils.blockUI();
    let initialObservable = this.vehicleData.loadVehicleFormDialogDropdowns$();
    this.vehicleData.GetPolicyHistories();
    this.vehicleData.initiateFormGroup(new VehicleDto());
    this.vehicleData.setPage(1);
    this.dtOptions = {
      pagingType: 'full_numbers',
      responsive: true,
      processing: true,
      destroy: true
    };
    console.log('Initial Vehicle Load : ', this.vehicleData.isVehicleUpdated);
    this.tableHeaders = Object.keys(this.VehicleTexts.tableHeadersPolicy).map(key => ({ value: key, text: this.VehicleTexts.tableHeadersPolicy[key] }));

    this.vehicleData.searchFilter = null;
    this.renderTable();
    this.vehicleData.retrieveDropDownValues();
    this.vehicleData.getPrimaryAddress();
    this.changeShowRowsNumber(10);
    let isVehicleLoaded = false;
    this.raterApiData.ratingData.subscribe(() => {
      isVehicleLoaded = true;
      if (!this.vehicleData.isVehicleUpdated) {
        initialObservable.pipe(
          switchMap(() => this.renderVehicleTable$())
        ).subscribe(() => Utils.unblockUI())
      }
    });
    if (!isVehicleLoaded && !this.vehicleData.isVehicleUpdated) {
        initialObservable.pipe(
          switchMap(() => this.renderVehicleTable$())
        ).subscribe(() => Utils.unblockUI())
    }
    this.vehicleData.isVehicleUpdated = false;
    initialObservable.subscribe(() => Utils.unblockUI());

    this.vehicleData.setDefaultSort();

    this.checkTrailerInterchangeLimits();
    this.applicantData.setApplicantDataset();
    this.lists = this.applicantData.applicantDropdownList;
    this.getBusinessDetails();
  }
  
    // convenience getter for easy access to form fields
    get fgBD() { return this.vehicleData.vehicleHeaderForm; }
    get f() { return this.vehicleData.vehicleHeaderForm.controls; }

  changeAccountCategory() {
    this.vehicleData.vehicleHeaderForm.controls['accountCategoryUWDropdown'].patchValue(this.vehicleData.vehicleHeaderForm.controls['accountCategoryDropdown'].value);
  }

  getBusinessDetails() {
    if (this.submissionData.riskDetail.businessDetails == null) {
      return;
    }
    const defaultValueMainGVW = this.submissionData.raterVersionSupport() ? 5 : 1;

    this.f['trailerTypeLimitId'].patchValue(this.submissionData.riskDetail.riskCoverage?.trailerTypeLimitId ?? 859);
    this.f['mainUseDropdown'].patchValue(this.submissionData.riskDetail.businessDetails?.mainUseId ?? 1);
    this.f['mainGVW'].patchValue(this.submissionData.riskDetail.businessDetails?.mainGVWId ?? defaultValueMainGVW);
    this.f['secondaryBusinessGroupDropdown'].patchValue(this.submissionData.riskDetail.businessDetails?.secondaryBusinessGroupId ?? 1);
    this.f['accountCategoryDropdown'].patchValue(this.submissionData.riskDetail.businessDetails?.accountCategoryId ?? 1);
    this.f['accountCategoryUWDropdown'].patchValue(this.submissionData.riskDetail.businessDetails?.accountCategoryUWId ?? 1);

  }

  checkTrailerInterchangeLimits() {
    this.disableTrailerType = (
      (this.submissionData.riskDetail.riskCoverage.trailerInterchangeLimitId == null || this.submissionData.riskDetail.riskCoverage.trailerInterchangeLimitId == 822) && 
      (this.submissionData.riskDetail.riskCoverage.nonOwnedTrailerLimitId == null || this.submissionData.riskDetail.riskCoverage.nonOwnedTrailerLimitId == 839)
    );
    if (this.disableTrailerType) {
      this.limitsData.isTrailerType = false;
    } else {
      this.limitsData.isTrailerType = true;
    }
  }

  setTooltip(field, dataList) {
    if (field && dataList?.length > 0) {
      const data = dataList.find(x => x.value == field);
      if (data) {
        return data.label;
      }
    }
  }

  renderVehicleTable() {
    this.vehicleData.vehicleInfoList = [];
    this.vehicleData.pagedItems = [];
    this.vehicleData.renderPolicyVehicleList();
  }

  renderVehicleTable$() : Observable<any> {
    this.vehicleData.vehicleInfoList = [];
    this.vehicleData.pagedItems = [];
    return this.vehicleData.renderPolicyVehicleListObservable();
  }

  get vehicleCount() { return this.submissionData.riskDetail.vehicles?.filter(x => x.deletedDate == null && x.isValidated).length ?? 0; }

  public isLimitCheckboxDisabled(vehicle) : boolean {
    return !vehicle.isMainRow || vehicle.deletedDate !== null || !this.policySummaryData.canEditPolicy;
  }

  renderTable() {
    if (this.isDtInitialized) {
      this.dtElement.dtInstance.then((dtInstance: DataTables.Api) => {
        // Destroy the table first
        dtInstance.destroy();
        // Call the dtTrigger to rerender again
        this.dtTrigger.next();
      });
    } else {
      this.dtTrigger.next();
      this.isDtInitialized = true;
    }
  }

  deleteVehicle(vehicle: VehicleDto) {
    if (!this.policySummaryData.canEditPolicy) { return; }
    NotifUtils.showConfirmMessage(`Are you sure you want to delete '${vehicle?.vin}'?`, () => {
      Utils.blockUI();
      this.vehicleData.delete(vehicle.id, true)
        .pipe(finalize(() => Utils.unblockUI()))
        .subscribe(() => {
          this.vehicleData.renderPolicyVehicleList();
          this.policySummaryData.updateSubtatusForPendingEndorsement();
          this.policySummaryData.setVehicleTypeHeader();
        }, (error) => {
          console.error(this.VehicleTexts.messages.errorRemove, error);
        });
    });
  }

  editVehicle(vehicle: VehicleDto) {
    if (!this.policySummaryData.canEditPolicy) { return; }
    if (!vehicle.isValidated && !vehicle.registeredState) {
      vehicle = new VehicleDto(vehicle);
      vehicle.registeredState = this.submissionData.riskDetail?.nameAndAddress?.state;
    }
    
    const vins = this.vehicleData.vehicleInfoList.filter(v => v.id !== vehicle.id).map(v => v.vin + v.registeredState);
    this.vehicleData.initiateFormValues(vehicle,vins);

    this.modalButton = 'Save';
    this.btnAddVehicle.nativeElement.click();

    setTimeout(() => {
      const elementReference = document.querySelector('[id=year]');
      if (elementReference instanceof HTMLElement) {
        elementReference.focus();
      }
    }, 500);
  }

  viewVehicle(vehicle: VehicleDto) {
    if (!vehicle.isValidated && !vehicle.registeredState) {
      vehicle = new VehicleDto(vehicle);
      vehicle.registeredState = this.submissionData.riskDetail?.nameAndAddress?.state;
    }
    
    const vins = this.vehicleData.vehicleInfoList.filter(v => v.id !== vehicle.id).map(v => v.vin + v.registeredState);
    this.vehicleData.initiateFormValues(vehicle,vins,  true);

    this.vehicleData.getGaragingAddress(vehicle.garagingAddressId, () => {
      this.modalButton = 'View';
      this.btnAddVehicle.nativeElement.click();
    });
  }

  addNewVehicle() {
    const vehicle = this.vehicleData.setDefaults();
  
    const vins = this.vehicleData.vehicleInfoList.filter(v => v.id !== vehicle.id).map(v => v.vin + v.registeredState);
    this.vehicleData.initiateFormValues(vehicle,vins);
    this.modalButton = 'Add Vehicle';
    this.btnAddVehicle.nativeElement.click();

    setTimeout(() => {
      const elementReference = document.querySelector('[id=year]');
      if (elementReference instanceof HTMLElement) {
        elementReference.focus();
      }
    }, 500);
  }

  reinstateVehicle(vehicle: VehicleDto) {
    if (!this.policySummaryData.canEditPolicy) { return; }
    NotifUtils.showConfirmMessage(`Are you sure you want to reinstate '${vehicle?.vin}'?`, () => {
      Utils.blockUI();
      this.vehicleData.reinstate(vehicle.id, true)
        .pipe(finalize(() => Utils.unblockUI()))
        .subscribe(() => {
          this.vehicleData.renderPolicyVehicleList();
          this.policySummaryData.updateSubtatusForPendingEndorsement();
        }, (error) => {
          console.error(this.VehicleTexts.messages.errorRemove, error);
        });
    });
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

  coverageChanged(vehicle: VehicleModel, value: boolean, field: string) {
    Utils.blockUI();
    vehicle[field] = value;
    vehicle = this.setCoverageFromLimits(vehicle, field);
    vehicle = this.vehicleData.cleanDeductibleValues(vehicle);
    vehicle.isEndorsement = true;
    if (field === 'hasCoverageCargo' && !value) {
      vehicle.hasCoverageRefrigeration = false;
    }

    this.vehicleData.formatVehicleIdsToText();
    this.vehicleData.update(vehicle).pipe(
      take(1),
      switchMap((data) => {
        // this.vehicleData.renderVehicleList(); //Don't use this as this will redraw table
        let editingVehicle = this.vehicleData.vehicleInfoList.find(v => v.id === data.id);
        if(editingVehicle === undefined) {
          this.vehicleData.vehicleInfoList = this.vehicleData.vehicleInfoList.filter(v => v.id != vehicle.id);
          this.vehicleData.vehicleInfoList.push(data);
          editingVehicle = data;
        }

        if (field === 'isCoverageCollision') {
          editingVehicle['coverageCollisionDeductible'] = this.vehicleData.vehicleDropdownsList.collisionDeductibleList.find(g => g.value === data.coverageCollisionDeductibleId)?.label;
        } else if (field === 'isCoverageFireTheft')  {
          editingVehicle['coverageFireTheftDeductible'] = this.vehicleData.vehicleDropdownsList.comprehensiveDeductibleList.find(g => g.value === data.coverageFireTheftDeductibleId)?.label;
        }

        // Update RiskDetail Vehicles
        this.submissionData.riskDetail.vehicles = this.vehicleData.vehicleInfoList;
        this.policySummaryData.updateSubtatusForPendingEndorsement();
        return this.vehicleData.renderPolicyVehicleListObservable();
      })
    ).subscribe((data) => {
      Utils.unblockUI();
    }, (error) => {
      Utils.unblockUI();
      console.error('Error on saving', error);
    });
  }

  public ToggleHiding(): void {
    this.hideMe = !this.hideMe;
  }

  searchByVin(vin: string) {
    vin = vin?.trim().toLowerCase();
    this.vehicleData.searchFilter = vin === '' ? null : vin;
    this.vehicleData.setPage(1, false);
  }

  setPage(page: number) {
    this.vehicleData.setPage(page);
  }

  selectFile() {
    this.fileSelection.nativeElement.value = null;
    this.fileSelection.nativeElement.click();
  }

  uploadFile(fileInfo: any) {
    if (fileInfo.length === 0) {
      return;
    }

    const submissionNo = this.submissionData?.riskDetail?.submissionNumber;

    const file = <File>fileInfo[0];

    //region validate filename
    // if (file.name.indexOf(submissionNo) === -1 || file.name.indexOf('BT Vehicle') === -1) {
    //   // this.toastr.error('Error encounter while uploading the file!', 'Failed!');
    //   NotifUtils.showError(`Incorrect vehicle template uploaded for <br>${submissionNo}`, () => { });
    //   return;
    // }
    //endregion

    const formData = new FormData();
    formData.append('file', file, file.name);
    this.vehicleData.uploadExcelFile(formData).subscribe(data => {
      if (data.type === HttpEventType.Response) {
        this.excelVehicles = this.mapTextToId(data.body);
        this.btnAddVehiclesFromExcel.nativeElement.click();
        // this.toastr.success('Upload Successful!', 'Success!');
      }
    }, error => {
      NotifUtils.showError('There was an error uploading the file. Please try again.', () => {
      });
    });
  }

  downloadTemplate() {
    const submissionNo = this.submissionData?.riskDetail?.submissionNumber;

    this.vehicleData.downloadTemplate().subscribe(data => {
      if (data.type === HttpEventType.Response) {
        const blob = new Blob([data.body], { type: 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet' });
        const url = window.URL.createObjectURL(blob);
        const anchor = document.createElement('a');
        // anchor.download = 'Vehicles-Upload-Template.xlsx';
        anchor.download = `BT Vehicle Upload Template_${submissionNo}.xlsx`;

        anchor.href = url;
        anchor.click();
      }
    });
  }

  private mapTextToId(vehicles: VehicleDto[]): VehicleDto[] {
    const lists = this.vehicleData.vehicleDropdownsList;
    const cols = lists.collisionDeductibleList;

    const vehiclesDefaults: Array<VehicleDto> = new Array<VehicleDto>();

    vehicles.forEach(v => {
      const vehDefault = this.vehicleData.setDefaults();

      vehDefault.riskDetailId = this.submissionData.riskDetail?.id;
      vehDefault.isValidated = false;

      vehDefault.vin = v.vin;
      vehDefault.year = v.year;
      vehDefault.make = v.make;
      vehDefault.model = v.model;
      vehDefault.registeredState = v.registeredState;
      vehDefault.stateFull = v.stateFull;
      vehDefault.statedAmount = v.statedAmount;
      vehDefault.vehicleType = v.vehicleType;
      vehDefault.description = v.description;
      vehDefault.businessClass = v.businessClass;
      vehDefault.grossVehicleWeight = v.grossVehicleWeight;

      vehDefault.vin = vehDefault.vin?.substring(0, 17);
      vehDefault.isAutoLiability = true;

      if (vehDefault.statedAmount) {
        vehDefault.statedAmount = Number(vehDefault.statedAmount?.toString()?.substring(0, 10));
      }

      if (vehDefault.grossVehicleWeight) {
        vehDefault.grossVehicleWeightId = lists.grossVehicleWeightList.find(w => w.label === vehDefault.grossVehicleWeight)?.value;
      }
      if (vehDefault.coverageCollisionDeductible) {
        vehDefault.coverageCollisionDeductibleId = cols.find(x => x.label.trim() === vehDefault.coverageCollisionDeductible)?.value;
      }

      if (!this.isEmpty(vehDefault.year) && !this.isEmpty(vehDefault.make) && !this.isEmpty(vehDefault.model) && !this.isEmpty(vehDefault.vin) && !this.isEmpty(vehDefault.grossVehicleWeight)
          && !this.isEmpty(vehDefault.stateFull) && !this.isEmpty(vehDefault.statedAmount) && !this.isEmpty(vehDefault.vehicleType) && !this.isEmpty(vehDefault.description) && !this.isEmpty(vehDefault.businessClass)) {
        vehDefault.isValidated = this.isDuplicateVin(vehDefault.vin);
      }

      vehDefault.options = this.vehicleData.getOptionQuote;

      vehiclesDefaults.push(vehDefault);
    });


    return vehiclesDefaults;
  }

  private isEmpty(str) {
      return (!str || str.length === 0 );
  }

  private isDuplicateVin(vinValue) {
    const savedVins = this.vehicleData.vehicleInfoList.map(v => v.vin);
    const vin = String(vinValue);

    const savedVin = savedVins.find(sv => sv?.toLowerCase() === vin?.toLowerCase());
    console.log('savedVins', savedVins, 'savedVin', savedVin);
    return savedVin ? false : true;
  }

  setCoverageFromLimits(vehicle: VehicleModel, field: string) {
    if (field === 'isCoverageFireTheft') {
      vehicle.coverageFireTheftDeductibleId = this.coverage?.compDeductibleId ?? this.coverage?.fireDeductibleId;
    } else if (field === 'isCoverageCollision') {
      vehicle.coverageCollisionDeductibleId = this.coverage && this.coverage.collDeductibleId;
    }
    return vehicle;
  }

  get acceptedFileTypes(): string {
    let acceptedFileTypes: string = 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet';

    return acceptedFileTypes;
  }

  isCheckboxColumns(column: string): boolean {
    return ['Liab', 'Comp / FT', 'Coll', 'Cargo', 'Reefer'].filter(x => x == column).length > 0;
  }

  vehicleExpandToggle(vehicle: VehicleDto) {
    vehicle.isExpanded = !vehicle.isExpanded;
    if(!!vehicle.isExpanded) {
      if(vehicle.previousVehicles == null){
        // Load previous vehicle if not yet loaded.
        this.vehicleData.loadPreviousVehicle(vehicle).subscribe();
      }
    }
  }

  GetVehicleEndorsementNumber(vehicle: VehicleDto) {
    let details = { label: '', description: ''};

    const histories = this.vehicleData.policyHistories;
    const history = histories?.find(x => x.endorsementNumber == vehicle.endorsementNumber.toString());

    if (history?.policyStatus === 'Canceled') {
      details.label = 'C';
      details.description = 'Cancellation';
    } else if (history?.policyStatus === 'Reinstated') {
      details.label = 'R';
      details.description = 'Reinstatement';
    } else if (history?.endorsementNumber === '0') {
      details.label = 'B';
      details.description = 'Bound';
    } else if (Number(history?.endorsementNumber) > 0) {
      details.label = `E${history?.endorsementNumber}`;
      details.description = `Endorsement ${history?.endorsementNumber}`;
    }

    return details;
  }

  generateExcelReport() {
    Utils.blockUI();
    const submissionNo = this.submissionData?.riskDetail?.submissionNumber; 
    const lists = this.vehicleData.vehicleDropdownsList;
   
    
    this.generateVehicleExport((data) => {
      if (data.length >= 0) {
        const reportfileName = `BT Vehicle Midterm List_${submissionNo}`;
        const rows: any[] = [];
        rows.push(this.vehicleData.policyReportHeaders); // add headers
        data.forEach(row => {
          const record = {
            vin: row.vin,
            year: row.year,
            make: row.make,
            model: row.model,
            registeredState: row.registeredState,
            statedAmount: this.currencyPipe.transform(row.statedAmount ?? 0),
            vehicleType: row.vehicleType,
            vehicleDescriptionId: lists.vehicleDescriptionList?.find(d => d.value === row.vehicleDescriptionId)?.label,
            vehicleBusinessClassId: lists.vehicleBusinessClassList.find(d => d.value === row.vehicleBusinessClassId.toString())?.label,
            grossVehicleWeightId: lists.grossVehicleWeightList?.find(d => d.value === row.grossVehicleWeightId)?.label,
            radius: row.radius,
            isCoverageFireTheft: row.isCoverageFireTheft ? 'Yes' : 'No',
            isCoverageCollision: row.isCoverageCollision ? 'Yes' : 'No',
            coverageFireTheftDeductibleId: lists.comprehensiveDeductibleList.find(d => d.value === row.coverageFireTheftDeductibleId)?.label,
            coverageCollisionDeductibleId: lists.collisionDeductibleList.find(d => d.value === row.coverageCollisionDeductibleId)?.label,
            hasCoverageCargo: row.hasCoverageCargo ? 'Yes' : 'No',
            hasCoverageRefrigeration: row.hasCoverageRefrigeration ? 'Yes' : 'No',
            effectiveDate: row.effectiveDate == null ? '' : formatDate(row.effectiveDate, 'MM/dd/yyyy', 'en'),
            expirationDate: row.expirationDate == null ? '' : formatDate(row.expirationDate, 'MM/dd/yyyy', 'en')
          };
          rows.push(record);
        });
        ReportUtils.exportToCsv(reportfileName, rows);
      } else {
        this.vehicleData.downloadTemplate().subscribe(data => {
          if (data.type === HttpEventType.Response) {
            const blob = new Blob([data.body], { type: 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet' });
            const url = window.URL.createObjectURL(blob);
            const anchor = document.createElement('a');
            // anchor.download = 'Vehicles-Upload-Template.xlsx';
            anchor.download = `BT Vehicle Export_${submissionNo}.xlsx`;
    
            anchor.href = url;
            anchor.click();
          }
        });
      }
    });
  }

  private generateVehicleExport(callbackFn?: Function) {
    Utils.blockUI();
    if ((this.submissionData.riskDetail.vehicles?.length ?? 0) === 0) { this.vehicleData.pagedItems = []; } // Temp fix for recurring vehicle cache
    if (this.submissionData.isNew) { return; }
    const riskDetailId = this.submissionData.riskDetail?.id;
    this.vehicleService.getByRiskDetailIdPolicy(riskDetailId, true).pipe(takeUntil(this.stop$)).subscribe((data) => {
        Utils.unblockUI();
        if (callbackFn) { callbackFn(data); }
    }, err => {
        Utils.unblockUI();
        NotifUtils.showError(JSON.stringify(err));
    });
  }
  
  changeShowRowsNumber(showRowsNumber: number) {
    this.vehicleData.changeShowRowsNumber(1, showRowsNumber);
  }
}
