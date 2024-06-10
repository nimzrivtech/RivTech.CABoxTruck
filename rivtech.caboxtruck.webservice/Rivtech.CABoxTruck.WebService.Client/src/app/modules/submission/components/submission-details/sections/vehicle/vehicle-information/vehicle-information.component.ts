import { Component, OnInit, ViewChild } from '@angular/core';
import { Subject } from 'rxjs';
import { finalize, take } from 'rxjs/operators';
import { BsModalRef } from 'ngx-bootstrap';
import { DataTableDirective } from 'angular-datatables';

import Utils from '../../../../../../../shared/utilities/utils';
import NotifUtils from '../../../../../../../shared/utilities/notif-utils';
import { VehicleDto } from '../../../../../../../shared/models/submission/Vehicle/VehicleDto';
import { VehicleData } from '../../../../../data/vehicle/vehicle.data';
import { VehicleTextConstants } from '../../../../../../../shared/constants/vehicle.text.constants';
import { SubmissionData } from '../../../../../data/submission.data';
import { LimitsData } from '../../../../../data/limits/limits.data';
import { VehicleModel } from '../../../../../../../shared/models/submission/Vehicle/vehicle.model';
import { RiskCoverageDTO } from '../../../../../../../shared/models/submission/limits/riskCoverageDto';
import { ElementRef } from '@angular/core';
import { HttpEventType } from '@angular/common/http';
import { TableConstants } from '../../../../../../../shared/constants/table.constants';
import { ReportsService } from '@app/modules/dashboard/services/reports.service';
import { takeUntil } from 'rxjs/operators';
import { BaseClass } from 'app/shared/base-class';
import { ReportUtils } from '@app/shared/utilities/report.utils';
import { ToastrService } from 'ngx-toastr';
import { VehicleService } from '@app/modules/submission/services/vehicle.service';
import { CurrencyPipe } from '@angular/common';
import { ReportManagementFilterDTO } from 'app/shared/models/reports/report-management-filter.dto';
import { SummaryData } from '../../../../../../../modules/submission/data/summary.data';
import { LvAccountCategory, LvGVWCategoryBeforeV7, LvGVWCategoryV7, LvMainUse, LvSecondaryBusinessGroup } from '@app/shared/constants/business-details.options.constants';
import { ApplicantData, ApplicantInformationDropDowns } from '@app/modules/submission/data/applicant/applicant.data';

@Component({
  selector: 'app-vehicle-information',
  templateUrl: './vehicle-information.component.html',
  styleUrls: ['./vehicle-information.component.scss']
})
export class VehicleInformationComponent extends BaseClass implements OnInit {
  applicantList: ApplicantInformationDropDowns;
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
    private reportService: ReportsService,
    private toastr: ToastrService,
    private vehicleService: VehicleService,
    private currencyPipe: CurrencyPipe,
    private summaryData: SummaryData,
    public applicantData: ApplicantData,
  ) {
    super();
   }

  ngOnInit() {
    this.vehicleData.setPage(1);
    this.dtOptions = {
      pagingType: 'full_numbers',
      responsive: true,
      processing: true,
      destroy: true
    };

    this.tableHeaders = Object.keys(this.VehicleTexts.tableHeaders).map(key => ({ value: key, text: this.VehicleTexts.tableHeaders[key] }));

    this.vehicleData.searchFilter = null;
    this.renderTable();
    this.vehicleData.retrieveDropDownValues();
    this.vehicleData.fetchVehicleDatasets();
    this.vehicleData.getPrimaryAddress();

    this.vehicleData.vehicleInfoList = [];
    this.vehicleData.pagedItems = [];
    if (!this.vehicleData.isVehicleUpdated) {
      Utils.blockUI();
      this.vehicleData.renderVehicleList$().subscribe(() => Utils.unblockUI());
    }

    this.vehicleData.isVehicleUpdated = false;

    this.vehicleData.setDefaultSort();

    this.checkTrailerInterchangeLimits();
    this.applicantData.setApplicantDataset();
    this.applicantList = this.applicantData.applicantDropdownList;
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

    this.f['trailerTypeLimitId'].patchValue(this.submissionData.riskDetail.riskCoverage?.trailerTypeLimitId ?? this.assignDefaultValue(859));
    this.f['mainUseDropdown'].patchValue(this.submissionData.riskDetail.businessDetails?.mainUseId ?? this.assignDefaultValue(1));
    this.f['mainGVW'].patchValue(this.submissionData.riskDetail.businessDetails?.mainGVWId ?? this.assignDefaultValue(defaultValueMainGVW));
    this.f['secondaryBusinessGroupDropdown'].patchValue(this.submissionData.riskDetail.businessDetails?.secondaryBusinessGroupId ?? this.assignDefaultValue(1));
    this.f['accountCategoryDropdown'].patchValue(this.submissionData.riskDetail.businessDetails?.accountCategoryId ?? this.assignDefaultValue(1));
    this.f['accountCategoryUWDropdown'].patchValue(this.submissionData.riskDetail.businessDetails?.accountCategoryUWId ?? this.assignDefaultValue(1));
  }

  assignDefaultValue(value: any): any {
    this.vehicleData.vehicleHeaderForm.markAsDirty();
    return value;
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

  get vehicleCount() { return this.vehicleData.vehicleInfoList?.length ?? 0; }

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
    NotifUtils.showConfirmMessage(`Are you sure you want to delete '${vehicle?.vin}'?`, () => {
      Utils.blockUI();
      this.vehicleData.delete(vehicle.id)
        .pipe(finalize(() => Utils.unblockUI()))
        .subscribe(() => {
          this.vehicleData.renderVehicleList();
          this.summaryData.setVehicleTypeHeader();
        }, (error) => {
          console.error(this.VehicleTexts.messages.errorRemove, error);
        });
    });
  }


  editVehicle(vehicle: VehicleDto) {
    if (!vehicle.isValidated && !vehicle.registeredState) {
      vehicle = new VehicleDto(vehicle);
      vehicle.registeredState = this.submissionData.riskDetail?.nameAndAddress?.state;
    }
    
    const vins = this.vehicleData.vehicleInfoList.filter(v => v.id !== vehicle.id).map(v => v.vin??'' + v.registeredState??'');
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

  addNewVehicle() {
    const vehicle = this.vehicleData.setDefaults();
    //const vins = this.vehicleData.vehicleInfoList.map(v => v.vin + v.registeredState);
    const vins = this.vehicleData.vehicleInfoList.filter(v => v.id !== vehicle.id).map(v => v.vin??'' + v.registeredState??'');
    this.vehicleData.initiateFormValues(vehicle, vins );
    this.modalButton = 'Add Vehicle';
    this.btnAddVehicle.nativeElement.click();
   
    setTimeout(() => {
      const elementReference = document.querySelector('[id=year]');
      if (elementReference instanceof HTMLElement) {
        elementReference.focus();
      }
    }, 500);
  }
  deleteAllVins() {
    NotifUtils.showConfirmMessage(`Are you sure you want to delete all vehicles?`, () => {
      Utils.blockUI();
      this.vehicleData.deleteall()
        .pipe(finalize(() => Utils.unblockUI()))
        .subscribe(() => {
          this.vehicleData.renderVehicleList();
          this.summaryData.setVehicleTypeHeader();
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
    vehicle[field] = value;
    vehicle = this.setCoverageFromLimits(vehicle, field);
    vehicle = this.vehicleData.cleanDeductibleValues(vehicle);
    if (field === 'hasCoverageCargo' && !value) {
      vehicle.hasCoverageRefrigeration = false;
    }
    this.vehicleData.formatVehicleIdsToText();
    Utils.blockUI();
    this.vehicleData.update(vehicle).pipe(take(1), finalize(() => Utils.unblockUI()))
      .subscribe((data) => {
        // this.vehicleData.renderVehicleList(); //Don't use this as this will redraw table
        const editingVehicle = this.vehicleData.vehicleInfoList.find(v => v.id === data.id);
        console.log(editingVehicle);
        if (field === 'isCoverageCollision') {
          editingVehicle['coverageCollisionDeductible'] = this.vehicleData.vehicleDropdownsList.collisionDeductibleList.find(g => g.value === data.coverageCollisionDeductibleId)?.label;
        } else if (field === 'isCoverageFireTheft')  {
          editingVehicle['coverageFireTheftDeductible'] = this.vehicleData.vehicleDropdownsList.comprehensiveDeductibleList.find(g => g.value === data.coverageFireTheftDeductibleId)?.label;
        }
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
    this.vehicleData.searchFilter = vin === '' ? null : vin
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
        this.summaryData.setVehicleTypeHeader();
        //this.toastr.success('Upload Successful!', 'Success!');
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
  
  generateExcelReport() {
    Utils.blockUI();
    const submissionNo = this.submissionData?.riskDetail?.submissionNumber; 
    const lists = this.vehicleData.vehicleDropdownsList;
   
    
    this.generateVehicleExport((data) => {
      if (data.length >= 0) {
        const reportfileName = `BT Vehicle Submission List_${submissionNo}`;
        const rows: any[] = [];
        rows.push(this.vehicleData.reportHeaders); // add headers
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
            grossVehicleWeightId: lists.grossVehicleWeightList.find(d => d.value === row.grossVehicleWeightId)?.label,
            radius: row.radius,
            isCoverageFireTheft: row.isCoverageFireTheft ? 'Yes' : 'No',
            isCoverageCollision: row.isCoverageCollision ? 'Yes' : 'No',
            coverageFireTheftDeductibleId: lists.comprehensiveDeductibleList.find(d => d.value === row.coverageFireTheftDeductibleId)?.label,
            coverageCollisionDeductibleId: lists.collisionDeductibleList.find(d => d.value === row.coverageCollisionDeductibleId)?.label,
            hasCoverageCargo: row.hasCoverageCargo ? 'Yes' : 'No',
            hasCoverageRefrigeration: row.hasCoverageRefrigeration ? 'Yes' : 'No'
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
            anchor.download = `BT Vehicle Upload Template_${submissionNo}.xlsx`;
    
            anchor.href = url;
            anchor.click();
          }
        });
      }
    });
  }

  generateExcelReportV1() {
    Utils.blockUI();
    const riskDetailId = this.submissionData.riskDetail?.id;
    const submissionNo = this.submissionData?.riskDetail?.submissionNumber;
    const reportfileName = `BT Vehicle Submission List_${submissionNo}`;
    const isRaterV7 = this.submissionData.raterVersionSupport();
    const reportManagementFilterDTO: ReportManagementFilterDTO = {
      reportType: "VehicleReport",
      reportFileName: reportfileName,
      riskDetailId: riskDetailId,
      sortField: this.vehicleData.sortOptions?.property,
      ascending: this.vehicleData.sortOptions?.order == 'asc' ? true : false
    };

    this.reportService.getReport(reportManagementFilterDTO, isRaterV7).pipe((takeUntil(this.stop$))).subscribe((resp) => {
      let downloadLink = document.createElement('a');
      downloadLink.href = window.URL.createObjectURL(resp.body);
      downloadLink.setAttribute('download', reportManagementFilterDTO.reportFileName);
      document.body.appendChild(downloadLink);
      downloadLink.click();
      downloadLink.remove();
      Utils.unblockUI();
    }, err => {
      Utils.unblockUI();
      NotifUtils.showError(JSON.stringify(err));
    });

  }

 
  private generateVehicleExport(callbackFn?: Function) {
    Utils.blockUI();
    if ((this.submissionData.riskDetail.vehicles?.length ?? 0) === 0) { this.vehicleData.pagedItems = []; } // Temp fix for recurring vehicle cache
    if (this.submissionData.isNew) { return; }
    const riskDetailId = this.submissionData.riskDetail?.id;
    this.vehicleService.getByRiskDetailId(riskDetailId, false).pipe(takeUntil(this.stop$)).subscribe((data) => {
        Utils.unblockUI();
        if (callbackFn) { callbackFn(data); }
    }, err => {
        Utils.unblockUI();
        NotifUtils.showError(JSON.stringify(err));
    });
  }

  private mapTextToId(vehicles: VehicleDto[]): VehicleDto[] {
    const lists = this.vehicleData.vehicleDropdownsList;
    const cols = lists.collisionDeductibleList;
    const comps = lists.comprehensiveDeductibleList;
    let regex = /IOQ/i

    const vehiclesDefaults: Array<VehicleDto> = new Array<VehicleDto>();

    vehicles.forEach(v => {
      const vehDefault = this.vehicleData.setDefaults(v);

      vehDefault.riskDetailId = this.submissionData.riskDetail?.id;
      vehDefault.isValidated = true;

      vehDefault.vin = this.vehicleData.isEmpty(v.vin)? vehDefault.vin?.substring(0,17): v.vin;
      vehDefault.year = v.year==0 ? null: v.year ;
      vehDefault.make = this.vehicleData.isEmpty(v.make)? null: v.make;
      vehDefault.model =this.vehicleData.isEmpty(v.model)? null: v.model;
      vehDefault.registeredState = v.registeredState;
      vehDefault.stateFull = v.stateFull;
      vehDefault.statedAmount = v.statedAmount;
      vehDefault.vehicleType = v.vehicleType;
      vehDefault.description =  v.description;
      vehDefault.businessClass = v.businessClass;
      vehDefault.grossVehicleWeight = v.grossVehicleWeight;
    

      
      vehDefault.isAutoLiability = true;

      if (vehDefault.statedAmount) {
        vehDefault.statedAmount = Number(vehDefault.statedAmount?.toString()?.substring(0, 10));
      }

      if (vehDefault.grossVehicleWeight) {
        vehDefault.grossVehicleWeightId = lists.grossVehicleWeightList.find(w => w.label === vehDefault.grossVehicleWeight)?.value;
      }

      if (vehDefault.coverageCollisionDeductible) {
        var a = vehDefault.coverageCollisionDeductible.replace('DED', '').trim();
        vehDefault.coverageCollisionDeductibleId = cols.find(x => x.label.replace('DED', '').replace('No', '').trim() ===
          vehDefault.coverageCollisionDeductible.replace('DED', '').replace('No', '').trim())?.value;
      }

      vehDefault.options = this.vehicleData.getOptionQuote;
      vehDefault.radius = this.vehicleData.isEmpty(v.radius) ? vehDefault.radius : v.radius;

      if (vehDefault.coverageFireTheftDesc) {
        vehDefault.coverageFireTheftDeductibleId = comps.find(x => x.label.replace('DED', '').replace('No', '').trim() ===
          vehDefault.coverageFireTheftDesc.replace('DED', '').replace('No', '').trim())?.value;
      }

      if ((vehDefault.year >= 3000) || this.InvalidVin(vehDefault.vin) || this.vehicleData.isEmpty(vehDefault.vin) || this.vehicleData.isEmpty(vehDefault.year)
        || this.vehicleData.isEmpty(vehDefault.make) || this.vehicleData.isEmpty(vehDefault.model) || this.vehicleData.isEmpty(vehDefault.vin)
        || this.vehicleData.isEmpty(vehDefault.statedAmount) || this.vehicleData.isEmpty(vehDefault.description)
        || (this.isEmpty(vehDefault.businessClass) && this.vehicleData.isEmpty(vehDefault.vehicleBusinessClassId))
        || this.vehicleData.isEmpty(vehDefault.useClassId)) {
        vehDefault.isValidated = false;
      }
      vehDefault.isValidated = this.vehicleData.vehicleDropdownsList?.vehicleDescriptionList?.find(d => d.label.toLowerCase() === v.description?.trim()?.toLowerCase())?.value === undefined ? false : vehDefault.isValidated;
      vehDefault.coverageFireTheft = this.vehicleData.isEmpty(v.coverageFireTheft) == false ? v.coverageFireTheft.toLocaleUpperCase() === "YES" || v.coverageFireTheft === "NO" ?
        v.coverageFireTheft : v.coverageFireTheft : vehDefault.coverageFireTheft;
      vehDefault.coverageCollision = this.vehicleData.isEmpty(v.coverageCollision) == false ? v.coverageCollision.toLocaleUpperCase() === "YES" || v.coverageCollision === "NO" ?
        v.coverageCollision : v.coverageCollision : vehDefault.coverageCollision;
      vehDefault.coverageFireTheftDesc = v.coverageFireTheftDesc;
      vehDefault.coverageCollisionDeductible = v.coverageCollisionDeductible;
      vehDefault.coverageRefrigeration = this.vehicleData.isEmpty(v.coverageRefrigeration) == false ? v.coverageRefrigeration.toLocaleUpperCase() === "YES" || v.coverageRefrigeration === "NO" ?
        v.coverageRefrigeration : v.coverageRefrigeration : vehDefault.coverageRefrigeration;
      vehDefault.coverageCargo = this.vehicleData.isEmpty(v.coverageCargo) == false ? v.coverageCargo.toLocaleUpperCase() === "YES" || v.coverageCargo === "NO" ?
        v.coverageCargo : v.coverageCargo : vehDefault.coverageCargo;

      vehiclesDefaults.push(vehDefault);
    });


    return vehiclesDefaults;
  }

  
  private InvalidVin(str: any)
  {
    var isInvalid = false
    const pattern: RegExp = /[IOQ]/  
    const numPattern : RegExp = /^[0-9]/

    if(this.vehicleData.isEmpty(str) )
    {
      isInvalid = true;
    }
    if(!this.vehicleData.isEmpty(str))
    {
      if(!numPattern.test(str) && str.length < 17)
      {
        isInvalid = true;
      }
      else if(pattern.test(str))
      {
        isInvalid =true;
      }
    }
    
    return isInvalid;
  }
  private isEmpty(str) {
    return (!str || str.length === 0 );
  }
  private isUndefined(str) {
    let isInvalid = false;
    isInvalid= str === undefined? true : false;
    return (isInvalid);
  }
  // private isDuplicateVin(vinValue) {
  //   const savedVins = this.vehicleData.vehicleInfoList.map(v => v.vin);
  //   const vin = String(vinValue);

  //   const savedVin = savedVins.find(sv => sv?.toLowerCase() === vin?.toLowerCase());
  //   console.log('savedVins', savedVins, 'savedVin', savedVin);
  //   return savedVin ? false : true;
  // }

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

  changeShowRowsNumber(showRowsNumber: number) {
    this.vehicleData.changeShowRowsNumber(1, showRowsNumber);
  }

  test() {
    alert(this.submissionData.validationList.vehicle);
  }
}
