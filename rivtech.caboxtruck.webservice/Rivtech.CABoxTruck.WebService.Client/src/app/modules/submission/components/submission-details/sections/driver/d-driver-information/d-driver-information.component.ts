import { AfterViewInit, Component, ElementRef, HostListener, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { DataTableDirective } from 'angular-datatables';
import { DataTable } from 'angular2-datatable';
import { Observable, Subject, Subscription } from 'rxjs';
import { DriverDto, DriverHeaderDto } from '../../../../../../../shared/models/submission/Driver/DriverDto';
import { DDriverInformationSectionComponent } from './d-driver-information-section/d-driver-information-section.component';
import { DriverData } from '../../../../../data/driver/driver.data';

import { BsModalRef, BsModalService } from 'ngx-bootstrap';
import { ToggleCollapseComponent } from '../../../../../../../shared/components/toggle-collapse/toggle-collapse.component';
import NotifUtils from '../../../../../../../shared/utilities/notif-utils';
import { switchMap, take, tap } from 'rxjs/operators';
import { DriverIncidentDto } from '../../../../../../../shared/models/submission/Driver/DriverIncidentDto';
import { SubmissionData } from '../../../../../../../modules/submission/data/submission.data';
import { ToastrService } from 'ngx-toastr';
import { DatePickerComponent } from '../../../../../../../shared/components/dynamic/date-picker/date-picker.component';
import { SelectItem } from '../../../../../../../shared/models/dynamic/select-item';
import { BaseComponent } from '../../../../../../../shared/base-component';
import { QuoteLimitsData } from '../../../../../../../modules/submission/data/quote/quote-limits.data';
import { HttpEventType } from '@angular/common/http';
import { TableConstants } from '../../../../../../../shared/constants/table.constants';
import Utils from '@app/shared/utilities/utils';
import { BlacklistedDriverService } from '@app/modules/dashboard/services/blacklisted-driver.service';
import { RaterApiData } from '@app/modules/submission/data/rater-api.data';
import { takeUntil } from 'rxjs/operators';
import { DriverService } from '@app/modules/submission/services/driver.service';
import { formatDate } from '@angular/common';
import { ReportUtils } from '@app/shared/utilities/report.utils';
import { ReportManagementFilterDTO } from 'app/shared/models/reports/report-management-filter.dto';
import { ReportsService } from '@app/modules/dashboard/services/reports.service';
import * as moment from 'moment';
@Component({
  selector: 'app-d-driver-information',
  templateUrl: './d-driver-information.component.html',
  styleUrls: ['./d-driver-information.component.scss'],
  // encapsulation: ViewEncapsulation.None // Add this line
})
export class DDriverInformationComponent extends BaseComponent implements OnInit, AfterViewInit, OnDestroy {

  subscriptions: Subscription[] = [];

  modalRef: BsModalRef | null;
  modalHeader: string = 'Add Driver Details';
  modalButton: string = 'Add Driver';

  dtElement: DataTableDirective;
  dtOptions: DataTables.Settings = {};
  dtTrigger: Subject<any> = new Subject();
  isDtInitialized: boolean = false;

  excelDrivers: DriverDto[];
  TableConstants = TableConstants;
  
  private savedDrivers: Array<DriverDto> = new Array<DriverDto>();
  private currDrivers : Array<DriverDto> = new Array<DriverDto>();

  @ViewChild('btnAddDriver') btnAddDriver;
  @ViewChild(DDriverInformationSectionComponent) driverInfoSec: DDriverInformationSectionComponent;
  @ViewChild('tblDriver') driverTable: DataTable;
  @ViewChild(ToggleCollapseComponent) toggleCollapse: ToggleCollapseComponent;

  @ViewChild('dtpmvrHeaderDate') dtpmvrHeaderDate: DatePickerComponent;

  @ViewChild('fileSelection') fileSelection: ElementRef;
  @ViewChild('btnAddDriversFromExcel') btnAddDriversFromExcel: ElementRef;

  constructor(public Data: DriverData,
    public submissionData: SubmissionData,
    private toastr: ToastrService,
    public quoteLimitsData: QuoteLimitsData,
    public blacklistedDriverService: BlacklistedDriverService,
    public raterApiData: RaterApiData,
    private driverService: DriverService,
    private reportService: ReportsService
  ) {
    super();
  }

  get fc() { return this.fg.controls; }
  get fg() { return this.Data.formDriverInfo; }
  get fh() { return this.Data.formDriverHeader.controls; }
  get applyFilterList() { return this.Data.driverDropdownsList.applyFilter.sort(this.sortByLabel); }
  get driverCount() { return this.Data.driverInfoList?.length ?? 0; }

  private sortByLabel(a: SelectItem, b: SelectItem) {
    return a.label < b.label ? -1 : a.label > b.label ? 1 : 0;
  }

  ngAfterViewInit() {
    setTimeout(() => {
      const elementReference = document.querySelector('[class="switch-slider"][id="driverRatingTab"]');
      if (elementReference instanceof HTMLElement) {
        elementReference.focus();
      }
    }, 200);
  }

  searchFilter(obj) {
    this.Data.setPage(this.Data.currentPage, true);
    obj?.preventDefault();
    if (obj) this.Data.setPage(1, false);
    return false;
  }

  ngOnInit() {
    // this.Data.formDriverHeader.controls.filter.valueChanges.subscribe(value => {
    //   this.Data.filterValue = value;
    // });

    this.dtOptions = {
      pagingType: 'full_numbers',
      responsive: true,
      processing: true,
      destroy: true
    };

    this.savedDrivers = [];
    this.currDrivers=[];

    this.renderTable();
    this.Data.retrieveDropDownValues();

    const risk = this.submissionData?.riskDetail;
    const policyEffectiveDate = this.submissionData.riskDetail?.brokerInfo?.effectiveDate ?? new Date(this.submissionData.currentServerDateTime);
    
    if (risk?.drivers) {
      this.Data.driverInfoList = risk?.drivers;
   
      this.Data.driverInfoList.forEach(d => {
        d.age= this.Data.calculateAge(d.birthDate, policyEffectiveDate)?.toString();
        if (this.isMissingRequiredFields(d) || this.validateDuplicates(d)) {
          d.isValidated = false;
        } else {
          d.isValidated = true;
        }
        });
   
    } else {
      this.Data.driverInfoList = Array<DriverDto>();
    }

    this.Data.changeShowRowsNumber(1, 10);
    this.Data.setPage(1);

    this.Data.dateFormatOptions();


    this.raterApiData.ratingData.pipe(
      switchMap(() => this.loadDriverHeader$())
    ).subscribe();

    this.subscriptions.push(this.loadDriverHeader$().subscribe());

    setTimeout(() => {
      this.Data.getDefaultPolicyState(risk?.id).then((state) => {
      });
    }, 1500);

    this.Data.sortByCreatedDate;

    this.getBlacklistedDrivers();
  }

  ngOnDestroy() {
    this.subscriptions.forEach(subs => subs.unsubscribe());
  }

  loadDriverHeader$() : Observable<any> {
    let risk = this.submissionData.riskDetail;
    this.Data.sortWithProperty("loadDriverHeader",);
    return this.Data.loadDriverHeader$().pipe(
      tap(() => {
        if (risk?.driverHeader) {
          this.Data.initiateHeaderValues(risk.driverHeader);
          this.Data.currMVRHeaderDate = this.Data.formDriverHeader.controls['mvrHeaderDate'].value;
        } else {
          this.Data.formDriverHeader.controls['mvrHeaderDate'].patchValue(this.Data.formatDateForPicker(risk?.brokerInfo?.effectiveDate));

          /*
          this.Data.triggerHeaderUpdate(new DriverHeaderDto(this.Data.formDriverHeader.value)).then((driverHeader) => {
            this.submissionData.riskDetail.driverHeader = driverHeader;
            console.log(driverHeader);
            this.Data.initiateHeaderValues(driverHeader);
            this.Data.currMVRHeaderDate = this.Data.formDriverHeader.controls['mvrHeaderDate'].value;
          }).catch((obj) => { }).finally(() => { });
          */
        }
      })
    );
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

  getBlacklistedDrivers() {
    this.Data.blacklistedDrivers = [];
    this.blacklistedDriverService.searchDriver('', '').pipe(take(1)).subscribe((result) => {
      this.Data.blacklistedDrivers = result;
    });
  }

  deleteDriver(driver: DriverDto) {
    NotifUtils.showConfirmMessage(`Are you sure you want to delete '${driver?.firstName} ${driver?.lastName}'?`, () => {
      this.Data.delete(driver.id).pipe(take(1)).subscribe(data => {
        this.Data.driverInfoList = this.Data.driverInfoList.filter(t => t.id !== driver.id);
        this.submissionData.riskDetail.drivers = this.Data.driverInfoList;
        //this.searchFilter(null);
        this.Data.setPage(this.Data.currentPage);
        this.Data.sortWithProperty();
      }, (error) => {
        //this.searchFilter(null);
        this.Data.setPage(this.Data.currentPage);
        this.Data.sortWithProperty();
        NotifUtils.showError('There was an error trying to remove a record. Please try again.');
      });
    });
  }

  deleteAllDriver() {
    NotifUtils.showConfirmMessage(`Are you sure you want to delete all drivers?`, () => {
      this.Data.deleteAll().pipe(take(1)).subscribe(data => {
        this.reloadtable();
        this.searchFilter(null);
      }, (error) => {
        //this.searchFilter(null);
        this.Data.setPage(this.Data.currentPage);
        this.Data.sortWithProperty();
        NotifUtils.showError('There was an error trying to remove a record. Please try again.');
      });
    });
  }

  editDriver(driver: DriverDto) {
    this.modalHeader = 'Edit Driver Details';
    this.modalButton = 'Save';
    driver = driver ?? new DriverDto();

    if (driver.isValidated === false) {
      driver.hireDate = driver.mvrDate = this.Data.formDriverHeader.get('mvrHeaderDate').value?.singleDate?.jsDate?.toLocaleDateString();
    }

    this.Data.getAllBlacklisterDriver(() => {
      this.Data.initiateFormValues(driver);

      this.Data.driverIncidentList = driver.driverIncidents ?? Array<DriverIncidentDto>();

      Object.assign(this.Data.driverIncidentList_old, this.Data.driverIncidentList);

      this.fn_openDriverInfo();

      //this.driverInfoSec.showExcludeKO =

      this.Data.enableKORule();
      this.driverInfoSec.isExcludeKOSwitch(driver.isExcludeKO);
      this.driverInfoSec.currMVRDate = this.Data.formDriverInfo.get('mvrDate').value;

      if (driver.isValidated === false) {
        this.driverInfoSec.ngOnChangesDOB(driver.birthDate);

        // this.fg.get('isOutOfState').setValue(this.Data.defaultPolicyState !== this.fc.state.value);

      }
      // this.driverInfoSec.outStateDriverSwitch(this.fg.get('isOutOfState').value);

      if (this.fg.get('isOutOfState').value == false) {
        this.fc['state'].disable();
      } else {
        this.fc['state'].enable();
      }

      if (this.fg.get('state').value == null && this.fg.get('isOutOfState').value == false) {
        this.fg.get('state').setValue(this.Data.getDriverState(driver));
      }

      this.fg.markAllAsTouched();
      this.fg.updateValueAndValidity();

      setTimeout(() => {
        (<HTMLElement>document.querySelector("#birthDateId")).focus();


        const elementReference = document.querySelector('[id=firstName]');
        // elementReference = document.querySelector('[id=firstName]');
        if (elementReference instanceof HTMLElement) {
          elementReference.focus();
        }
      }, 500);
    });
  }

  addNewDriver() {
    this.modalHeader = 'Add Driver Details';
    this.modalButton = 'Add Driver';
    // get state from Applicant name address
    // if (!this.Data.driverInfoList?.length) {
    this.Data.getAllBlacklisterDriver();
    if (this.Data.defaultPolicyState == null || this.Data.defaultPolicyState == "") {
      this.Data.getDefaultPolicyState(this.submissionData.riskDetail.id).then((state) => {
        this.addNewDriverDefault(state);
      }).catch((ret) => {
        this.addNewDriverDefault(ret);
        NotifUtils.showError('There was an error trying to get the default State. Please try again.');
      });
    } else {
      this.addNewDriverDefault(this.Data.defaultPolicyState);
    }

    // } else {
    //   this.addNewDriverDefault(this.Data.driverInfoList.slice(-1)[0].state);
    // }
  }

  addNewDriverDefault(state) {
    this.driverInfoSec.resetDatePicker();

    const drvr = new DriverDto();
    drvr.state = state;

    drvr.hireDate = drvr.mvrDate = this.Data.formDriverHeader.get('mvrHeaderDate').value?.singleDate?.jsDate?.toLocaleDateString();

    this.Data.formDriverInfo.reset();
    // this.Data.initiateFormGroup(drvr);
    drvr.isOutOfState = drvr.isOutOfState ? true : false;


    this.Data.initiateFormValues(drvr);



    this.Data.formDriverIncident.patchValue(new DriverIncidentDto());
    this.Data.formDriverIncident.reset();

    this.Data.driverIncidentList = Array<DriverIncidentDto>();
    Object.assign(this.Data.driverIncidentList_old, this.Data.driverIncidentList);

    //this.driverInfoSec.showExcludeKO =

    this.Data.enableKORule();
    this.driverInfoSec.isExcludeKOSwitch(drvr.isExcludeKO);
    this.driverInfoSec.currMVRDate = null;

    this.fn_openDriverInfo();

    this.Data.getStatesFiltered(true, drvr.isOutOfState);

    setTimeout(() => {
      const elementReference = document.querySelector('[id=firstName]');
      if (elementReference instanceof HTMLElement) {
        elementReference.focus();
      }
      this.driverInfoSec.outStateDriverSwitch(this.fc.isOutOfState.value, true);


    }, 500);
  }

  optionChanged(drvr: DriverDto, optNum: number) {
    if (this.quoteLimitsData.optionIsDisabled(optNum) && optNum !== 1) {
      const menuDropdownOpen = document.querySelectorAll(`.optInlcude[id*='${drvr.id}']`) as any;
      menuDropdownOpen[optNum - 1].checked = false;
      menuDropdownOpen[optNum - 1].disabled = true;

      return false;
    }

    let optValue = "";
    const oldOption = drvr.options;
    this.Data.driverInfoList = this.Data.driverInfoList.filter((value, key) => {
      if (value.id === drvr.id) {

        const menuDropdownOpen = document.querySelectorAll(`.optInlcude[id*='${drvr.id}']`) as any;
        const validateAgeOptions = this.Data.optAgeValidation(drvr);
        if (validateAgeOptions != null && !drvr.isExcludeKO) {
          NotifUtils.showWarning(validateAgeOptions.message);
          menuDropdownOpen.forEach(opt => { opt.checked = false; });
          return true;
        }

        menuDropdownOpen.forEach(opt => {
          if (opt.checked) {
            optValue = optValue + opt.value + ',';
          }
        });

        if (optNum === 1 && !menuDropdownOpen[optNum - 1].checked) {
          optValue = '';
        }
        optValue = optValue.replace(/,(?=\s*$)/, '');
        drvr.options = optValue?.length === 0 ? null : optValue;
        drvr.riskDetailId = this.submissionData.riskDetail.id;
        this.Data.updateOption(drvr).pipe(take(1)).subscribe(data => {
          Object.assign(value, drvr);
          this.updateDriverExclusionForms();
        }, (error) => {
          drvr.options = oldOption;
          Object.assign(value, drvr);
          this.toastr.error('There was an error trying to Update a record. Please try again.', 'Success!');
        });
      }
      return true;
    });
  }

  optionsDisabled(drvr: DriverDto) {
    const optionsArray = (drvr.options ?? '').split(',').map(o => +o);
    return optionsArray.indexOf(1) === -1 ? true : false;
  }

  updateDriverExclusionForms() {
    if (this.submissionData.riskDetail.riskForms?.length > 0) {
      const optionId = this.submissionData.riskDetail?.binding?.bindOptionId ?? '1';
      const drivers = this.submissionData.riskDetail?.drivers;
      let form = this.submissionData.riskDetail.riskForms.find(x => x.form?.id == 'DriverExclusion');
      if (form != null) {
        let index = this.submissionData.riskDetail.riskForms.indexOf(form);
        this.submissionData.riskDetail.riskForms[index].isSelected = false;
        if (drivers?.length > 0 && drivers.some(d => d.options == null || !d.options?.split(',')?.includes(optionId.toString()))) {
          this.submissionData.riskDetail.riskForms[index].isSelected = true;
        }
      }
    }
  }

  mvrChanged(drvr: DriverDto, isChecked) {
    const oldOption = drvr.options;
    this.Data.driverInfoList = this.Data.driverInfoList.filter((value, key) => {
      if (value.id === drvr.id) {
        drvr.isMvr = isChecked;
        const menuDropdownOpen = document.querySelectorAll(`.optInlcude[id*='${drvr.id}']`) as any;
        const validateAgeOptions = this.Data.optAgeValidation(drvr);
        if (validateAgeOptions != null && !drvr.isExcludeKO) { // && drvr.options != null
          if ((Number(drvr.age) == 23 || Number(drvr.age) == 24)) { NotifUtils.showWarning(validateAgeOptions.message); }
          menuDropdownOpen.forEach(opt => { opt.checked = false; });
          drvr.options = null;
        } else {

          drvr.options = this.Data.getOptionQuote;
        }
        drvr.isValidatedKO = validateAgeOptions == null ? true : (drvr.isExcludeKO && drvr.koReason?.length > 0);
        const parseOBJ = new DriverDto(drvr);
        Utils.blockUI();
        this.Data.update(parseOBJ).pipe(take(1)).subscribe(data => {
          Utils.unblockUI();
          Object.assign(value, drvr);
        }, (error) => {
          Utils.unblockUI();
          drvr.isMvr = !isChecked;
          drvr.options = oldOption;
          Object.assign(value, drvr);
          this.toastr.error('There was an error trying to Update MVR of record. Please try again.', 'Success!');
        });
      }
      return true;
    });
  }

  outStateChanged(drvr: DriverDto, isChecked) {
    this.Data.driverInfoList = this.Data.driverInfoList.filter((value, key) => {
      if (value.id === drvr.id) {
        drvr.isOutOfState = isChecked;
        const oldState = drvr.state;

        if (drvr.isOutOfState == false) {
          drvr.state = this.Data.defaultPolicyState;
        } else {
          drvr.state = null;
        }

        this.Data.initiateFormValues(drvr);

        if (this.Data.formDriverInfo.invalid ||this.validateDuplicates(drvr)) {
          drvr.isValidated = false;
        } else {
          drvr.isValidated = true;
        }
        Utils.blockUI();
        this.Data.update(drvr).pipe(take(1)).subscribe(data => {
          Utils.unblockUI();
          Object.assign(value, drvr);
        }, (error) => {
          Utils.unblockUI();
          drvr.isOutOfState = !isChecked;
          drvr.state = oldState;
          Object.assign(value, drvr);
          this.toastr.error('There was an error trying to Update Out State of record. Please try again.', 'Success!');
        });
      }
      return true;
    });
  }

  driverRatingTabSwitch(isChecked) {
    // this.Data.triggerHeaderUpdate(new DriverHeaderDto(this.Data.formDriverHeader.value)).then((driverHeader) => {
    // }).catch((obj) => { }).finally(() => { });
  }

  useAccidentInfoSwitch(isChecked) {
    // this.Data.triggerHeaderUpdate(new DriverHeaderDto(this.Data.formDriverHeader.value)).then((driverHeader) => {
    // }).catch((obj) => { }).finally(() => { });
  }

  ngOnChangesHeaderMVRDate(obj) {
    if (this.submissionData?.isIssued) { return; }
    const brokerEffectiveDate = this.submissionData?.riskDetail?.brokerInfo?.effectiveDate;
    let dateReverse = this.Data.formatDateForPicker(null); // this will return current date.

    if (this.Data.currMVRHeaderDate) {
      dateReverse = this.Data.currMVRHeaderDate;
    } else if (brokerEffectiveDate) {
      dateReverse = this.Data.formatDateForPicker(brokerEffectiveDate);
    }

    if (!obj.jsDate) {
      this.Data.formDriverHeader.get('mvrHeaderDate').setValue(dateReverse);
    } else {
      const validateMVR = this.Data.validateMVRdateEffective(obj.jsDate, brokerEffectiveDate);
      if (validateMVR?.length > 0) {
        NotifUtils.showError(validateMVR);
        this.Data.formDriverHeader.get('mvrHeaderDate').setValue(dateReverse);
      } else {
        this.Data.formDriverHeader.get('mvrHeaderDate').setValue(this.Data.formatDateForPicker(obj.jsDate));
      }
    }

    this.Data.currMVRHeaderDate = this.Data.formDriverHeader.get('mvrHeaderDate').value;
    const driverheader = Object.assign({}, this.submissionData?.riskDetail?.driverHeader);

    if (!driverheader.riskDetailId) { driverheader.riskDetailId = this.submissionData.riskDetail?.id; }
    driverheader.mvrHeaderDate = this.Data.formDriverHeader.get('mvrHeaderDate').value?.singleDate?.jsDate?.toLocaleDateString();

    this.Data.triggerHeaderUpdate(driverheader).then((drvr) => {
      this.submissionData.riskDetail.driverHeader.mvrHeaderDate = drvr.mvrHeaderDate;
      this.Data.initiateHeaderValues({ mvrHeaderDate: drvr?.mvrHeaderDate });
    }).catch(() => {
      this.Data.initiateHeaderValues({ mvrHeaderDate: this.submissionData.riskDetail?.driverHeader?.mvrHeaderDate });
    }).finally(() => { });
  }

  onItemSelectDeSelect(item: SelectItem, selected: boolean) {
    // this.Data.triggerHeaderUpdate().then((driverHeader) => {
    // }).catch((obj) => { }).finally(() => { });
  }

  onDeSelectDeselectAll(items: SelectItem[]) {
    // this.Data.formDriverHeader.patchValue({ applyFilter: items });
    // this.Data.triggerHeaderUpdate().then((driverHeader) => {
    // }).catch((obj) => { }).finally(() => { });
  }

  onFilterChange(value) {
    this.selectAllTag.input.parentElement.hidden = (value != '');
  }

  onDropDownClose() {
    this.selectAllTag.input.parentElement.hidden = false;
  }

  public fn_openDriverInfo() {
    this.btnAddDriver.nativeElement.click();
  }

  setPage(page: number) {
    this.Data.setPage(page);
  }

  changeShowRowsNumber(showRowsNumber: number) {
    this.Data.changeShowRowsNumber(1, showRowsNumber);
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
    // if (file.name.indexOf(submissionNo) === -1 || file.name.indexOf('BT Driver') === -1) {
    //   //this.toastr.error('Error encounter while uploading the file!', 'Failed!');
    //   NotifUtils.showError(`Incorrect driver template uploaded for <br>${submissionNo}`, () => { });
    //   return;
    // }
    //endregion

    const formData = new FormData();
    formData.append('file', file, file.name);

    this.Data.uploadExcelFile(formData).subscribe(data => {
      if (data.type === HttpEventType.Response) {
        this.excelDrivers = this.mapTextToId(data.body);
        this.btnAddDriversFromExcel.nativeElement.click();
      }
    }, error => {
      NotifUtils.showError('There was an error uploading the file. Please try again.', () => {
      });
    });
  }

  downloadTemplate() {
    const submissionNo = this.submissionData?.riskDetail?.submissionNumber;

    this.Data.downloadTemplate().subscribe(data => {
      if (data.type === HttpEventType.Response) {
        const blob = new Blob([data.body], { type: 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet' });
        const url = window.URL.createObjectURL(blob);
        const anchor = document.createElement('a');
        // anchor.download = 'Driver-Upload-Template.xlsx';
        anchor.download = `BT Driver Upload Template_${submissionNo}.xlsx`;
        anchor.href = url;
        anchor.click();
      }
    });
  }

  validateDuplicates(_drvr: DriverDto): boolean {
    //const currDrivers = Object.assign([], this.submissionData?.riskDetail?.drivers);
    //const currDrivers = Object.assign([], new Array<DriverDto>());
      const firstName: string = _drvr['firstName'] ?? '';
      const lastName: string = _drvr['lastName'] ?? '';
      const licenseNumber: string = _drvr['licenseNumber'] ?? '';
      const state: string = _drvr['state'] ?? '';
      const policyEffectiveDate = this.submissionData.riskDetail?.brokerInfo?.effectiveDate ?? new Date(this.submissionData.currentServerDateTime);
      
      let duplicateName: boolean = false;
      let duplicateDriverId: boolean = false;
      let duplicateBlacklistedDriverId: boolean = false;


      let validateEmpty : boolean = false;
      let dvrAge : any;

      if(!this.isEmpty(_drvr.birthDate))
      {
        dvrAge= this.calculateAge(_drvr.birthDate, policyEffectiveDate) ;
      }

      if (this.isEmpty(_drvr.firstName) ||this.isEmpty(_drvr.lastName) ||this.isEmpty(_drvr.birthDate) 
          ||this.isEmpty(_drvr.licenseNumber)||this.isEmpty(_drvr.state)
          ||this.isEmpty(_drvr.yrsCommDrivingExp)||dvrAge === 0|| (dvrAge <= 16)||(_drvr.yrsCommDrivingExp >= 65) )
      {
        validateEmpty= true;
      }

      if (this.currDrivers.length > 0){
        const drvr2 = Object.assign([], this.currDrivers, this.savedDrivers)?.find(s => s.licenseNumber?.toLowerCase()?.trim() === licenseNumber?.toLowerCase()?.trim() &&
          s.state?.toLowerCase()?.trim() === state?.toLowerCase()?.trim());
  
        if (drvr2 && drvr2?.id != _drvr?.id && (licenseNumber.length > 0)) {
          duplicateDriverId = true;
        } else {
          duplicateDriverId = false;
        }
        //#endregion Validate Driver Id
  
        const drvr3 = this.Data.blacklistedDrivers?.find(s => s.lastName.toLowerCase() === lastName.toLowerCase() && s.driverLicenseNumber?.toLowerCase()?.trim() === licenseNumber?.toLowerCase()?.trim()  && s.isActive === true);
  
        if (drvr3 && drvr3?.id != _drvr?.id && (licenseNumber.length > 0)) {
          duplicateBlacklistedDriverId = true;
        } else {
          duplicateBlacklistedDriverId = false;
        }
        //#endregion Validate Blacklisted Driver
        this.currDrivers.push(_drvr);
        return (duplicateDriverId === true || duplicateBlacklistedDriverId === true || validateEmpty);
        return (duplicateDriverId === true || duplicateBlacklistedDriverId === true || validateEmpty);
      }
      else {
      
        const drvr3 = this.Data.blacklistedDrivers?.find(s => s.driverLicenseNumber?.toLowerCase()?.trim() === licenseNumber?.toLowerCase()?.trim() &&
        s.lastName?.toLowerCase()?.trim() === lastName?.toLowerCase()?.trim() && s.isActive === true);
  
        if (drvr3 && drvr3?.id != _drvr?.id && (licenseNumber.length > 0)) {
          duplicateBlacklistedDriverId = true;
        } else {
          duplicateBlacklistedDriverId = false;
        }
          this.currDrivers.push(_drvr);
          return (false || duplicateBlacklistedDriverId === true || validateEmpty);
          return (false || duplicateBlacklistedDriverId === true || validateEmpty);
      }
     
    }

  calculateAge(dateOfBirth: any, policyEffectiveDate: any) {
    const dateEntry = new Date(dateOfBirth);
    const policyDate = policyEffectiveDate!='Invalid Date' ? new Date(policyEffectiveDate) : new Date();
    const yearDiff = moment(policyDate.toDateString()).diff(moment(dateEntry.toDateString()), 'years');
    
    return dateOfBirth ? Math.abs(yearDiff) : 0;
  }

  isMissingRequiredFields(obj: DriverDto): Boolean {
      if (this.isEmpty(obj.firstName) || this.isEmpty(obj.lastName) || this.isEmpty(obj.licenseNumber) ||
          this.isEmpty(obj.state) || this.isEmpty(obj.birthDate) || this.isEmpty(obj.yrsCommDrivingExp)) return true;

      return false;
   }

   private isEmpty(str) {
    return (!str || str.length === 0 );
  }
  
  generateExcelReport() {
    Utils.blockUI();
    const submissionNo = this.submissionData?.riskDetail?.submissionNumber; 
    
    this.generateDriverExport((data) => {
      if (data.length >= 0) {
        const reportfileName = `BT Driver Submission List_${submissionNo}`;
        const rows: any[] = [];
        rows.push(this.Data.reportHeaders); // add headers
        data.forEach(row => {
          const record = {
            firstName: row.firstName,
            middleName: row.middleName,
            lastName: row.lastName,
            licenseNumber: row.licenseNumber,
            state: row.state,
            birthDate: row.birthDate == null?'' : formatDate(row.birthDate, 'MM/dd/yyyy', 'en'),
            yrsCommDrivingExp: row.yrsCommDrivingExp
          };
          rows.push(record);
        });
        ReportUtils.exportToCsv(reportfileName, rows);
      } 
    });
  }

  generateExcelReportV1(){
    Utils.blockUI();
    const riskDetailId = this.submissionData.riskDetail?.id;
    const submissionNo = this.submissionData?.riskDetail?.submissionNumber; 
    const reportfileName = `BT Driver Submission List_${submissionNo}`;

    const reportManagementFilterDTO: ReportManagementFilterDTO = {
      reportType: "DriverReport",
      reportFileName: reportfileName,
      riskDetailId: riskDetailId,
      sortField: this.Data.sortOptions?.property,
      ascending: this.Data.sortOptions?.order == 'asc' ? true : false
    };

    this.reportService.getReport(reportManagementFilterDTO).pipe((takeUntil(this.stop$))).subscribe((resp) => {
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

  public generateDriverExport(callbackFn?: Function) {
    Utils.blockUI();
    const riskDetailId = this.submissionData.riskDetail?.id;
    this.driverService.getRiskExportDrivers(riskDetailId, false).pipe(takeUntil(this.stop$)).subscribe((data) => {
        Utils.unblockUI();
        if (callbackFn) { callbackFn(data); }
    }, err => {
        Utils.unblockUI();
        NotifUtils.showError(JSON.stringify(err));
    });
  }

  private mapTextToId(drivers: DriverDto[]): DriverDto[] {
    const lists = this.Data.driverDropdownsList;
    const policyEffectiveDate = this.submissionData.riskDetail?.brokerInfo?.effectiveDate ?? new Date(this.submissionData.currentServerDateTime);
    console.log('CheckServertime:'+this.submissionData.currentServerDateTime);
    drivers.forEach(d => {
      d.hireDate = d.mvrDate = this.Data.formDriverHeader.get('mvrHeaderDate').value?.singleDate?.jsDate?.toLocaleDateString();

      this.Data.initiateFormValues(d);

      this.Data.driverIncidentList = d.driverIncidents ?? Array<DriverIncidentDto>();
      d.riskDetailId = this.submissionData.riskDetail?.id;

      d.age = this.Data.calculateAge(d.birthDate, policyEffectiveDate)?.toString();
      d.age = d.age?.length === 0 ? null : d.age;
      d.isMvr = true;
      
      // let state = '';
      // if (d.stateFull?.length > 0) {
      //   state = this.Data.driverDropdownsList.stateList.find(f => f.value.toLowerCase() === d.stateFull?.toLowerCase() || f.label.toLowerCase() === d.stateFull?.toLowerCase())?.value ?? this.Data.defaultPolicyState;
      //   //d.state = state;
      // }

      // d.isOutOfState = (state === this.Data.defaultPolicyState || state?.length == 0) ? false : true;

      const showExcludeKO = this.Data.enableKORule(); // determine if the record is eligible or failed KO rule

      if (d.isExcludeKO && !showExcludeKO) { d.isExcludeKO = false; }
      d.isExcludeKO = d.isExcludeKO ? true : false;

      const validateAgeOptions = this.Data.optAgeValidation(d);
      d.isValidatedKO = validateAgeOptions == null ? true : (d.isExcludeKO && d.koReason?.length > 0);

      if (d.isValidatedKO === true) {
        d.options = this.Data.getOptionQuote;
      }

      //populate options selected;

      // d.state = ((d.state?.length === 0) ? this.Data.defaultPolicyState : d.state)?.substring(0, 3);
      //d.state = d.state?.substring(0, 3);
    });

    return drivers;
  }

  get selectAllTag() {
    const id = 'applyFilter';
    const selectAllCheckbox = <HTMLInputElement>document.querySelector(`ng-multiselect-dropdown#${id} li.multiselect-item-checkbox input`);
    const selectAllCheckboxLabel = <HTMLInputElement>document.querySelector(`ng-multiselect-dropdown#${id} li.multiselect-item-checkbox div`);
    return {
      input: selectAllCheckbox,
      label: selectAllCheckboxLabel
    };
  }

  public DisabledViaFilter(obj: DriverDto): boolean {
    // {value: 1, label: "KO Under 25"}
    // {value: 2, label: "KO AF/Moving Violation
    const applyFilter = this.Data.formDriverHeader.get('applyFilter').value;

    let items = applyFilter?.map(item => item.value);
    var validation = this.Data.optAgeValidation(obj);

    if (validation?.id == "KOUnder25" && items.find(item => item == 1 && obj.isExcludeKO !== true) && Number(obj.age) < 25) {
      return true;
    }

    if ((validation?.id == "KOMovingViolations" || validation?.hasMv == true) && items.find(item => item == 2 && obj.isExcludeKO !== true)) {
      return true;
    }

    return false;
  }

  public hiddenViaFilter(obj: DriverDto): boolean {
    // {value: 1, label: "KO Under 25"}
    // {value: 2, label: "KO AF/Moving Violation
    const applyFilter = this.Data.formDriverHeader.get('applyFilter').value;

    let items = applyFilter?.map(item => item.value);
    var validation = this.Data.optAgeValidation(obj);

    if(items.length === 0)
      return false;

    if (validation?.id == "KOUnder25" && items.find(item => item == 1 && obj.isExcludeKO !== true) && Number(obj.age) < 25) {
      return false;
    }

    if ((validation?.id == "KOMovingViolations" || validation?.hasMv == true) && items.find(item => item == 2 && obj.isExcludeKO !== true)) {
      return false;
    }

    return true;
  }

  //#region functions
  isOptionChecked(optList) {
    return optList = optList ?? '';
  }

  get acceptedFileTypes(): string {
    let acceptedFileTypes: string = 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet';

    return acceptedFileTypes;
  }

  @HostListener('keydown', ['$event'])
  onKeydownHandler(event: any, obj: any) {
    if (event.keyCode == 13 && event?.srcElement?.ariaLabel === "multiselect-search") {
      event.preventDefault();
      event.stopPropagation();
    }
  }

  //#endregion
  reloadtable() {
    // this.renderTable();
    const policyEffectiveDate = this.submissionData.riskDetail?.brokerInfo?.effectiveDate ?? new Date(this.submissionData.currentServerDateTime);
    this.generateDriverExport((data) => {
      if (data.length > 0) {
        this.Data.driverInfoList = data;
        this.Data.driverInfoList.forEach(d => {
          d.age= this.Data.calculateAge(d.birthDate, policyEffectiveDate)?.toString(); });
      } 
      else {
        this.Data.driverInfoList = Array<DriverDto>();
      }
      this.Data.setPage(1);
      this.submissionData.riskDetail.drivers = this.Data.driverInfoList;
    });

  }
  
}
