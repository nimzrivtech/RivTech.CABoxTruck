import { AfterViewInit, Component, ElementRef, HostListener, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { DataTableDirective } from 'angular-datatables';
import { DataTable } from 'angular2-datatable';
import { Observable, of, Subject, Subscription } from 'rxjs';
import { DriverDto, DriverHeaderDto } from '../../../../../../../shared/models/submission/Driver/DriverDto';
import { DDriverInformationSectionComponent } from './d-driver-information-section/d-driver-information-section.component';
import { DriverData } from '../../../../../../../modules/submission/data/driver/driver.data';
import { BsModalRef, BsModalService } from 'ngx-bootstrap';
import { ToggleCollapseComponent } from '../../../../../../../shared/components/toggle-collapse/toggle-collapse.component';
import NotifUtils from '../../../../../../../shared/utilities/notif-utils';
import { finalize, switchMap, take } from 'rxjs/operators';
import { DriverIncidentDto } from '../../../../../../../shared/models/submission/Driver/DriverIncidentDto';
import { SubmissionData } from '../../../../../../../modules/submission/data/submission.data';
import { ToastrService } from 'ngx-toastr';
import { DatePickerComponent } from '../../../../../../../shared/components/dynamic/date-picker/date-picker.component';
import { SelectItem } from '../../../../../../../shared/models/dynamic/select-item';
import { BaseComponent } from '../../../../../../../shared/base-component';
import { QuoteLimitsData } from '../../../../../../../modules/submission/data/quote/quote-limits.data';
import { HttpEventType } from '@angular/common/http';
import { TableConstants } from '../../../../../../../shared/constants/table.constants';
import { PolicySummaryData } from '@app/modules/policy/data/policy-summary.data';
import Utils from '@app/shared/utilities/utils';
import { BlacklistedDriverService } from '@app/modules/dashboard/services/blacklisted-driver.service';
import { DriverService } from '@app/modules/submission/services/driver.service';
import { formatDate } from '@angular/common';
import { ReportUtils } from '@app/shared/utilities/report.utils';
import { takeUntil } from 'rxjs/operators';
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
  bindOption: Number;

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
    public policySummaryData: PolicySummaryData,
    public blacklistedDriverService: BlacklistedDriverService,
    private driverService: DriverService
  ) {
    super();
  }

  get fc() { return this.fg.controls; }
  get fg() { return this.Data.formDriverInfo; }
  get fh() { return this.Data.formDriverHeader.controls; }
  get applyFilterList() { return this.Data.driverDropdownsList.applyFilter.sort(this.sortByLabel); }
  get driverCount() { return this.Data.driverInfoList?.filter(x => x.deletedDate == null)?.length ?? 0; }

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
    this.Data.getPolicyHistories();
    this.dtOptions = {
      pagingType: 'full_numbers',
      responsive: true,
      processing: true,
      destroy: true
    };

   

    this.renderTable();

    const risk = this.submissionData?.riskDetail;
    this.bindOption = Number(risk.binding?.bindOptionId) ?? 0;
    this.Data.renderDriverList().subscribe(() => {
      this.Data.setPage(1);
      this.Data.changeShowRowsNumber(1, 10);
      this.Data.retrieveDropDownValues();
    });

    this.Data.dateFormatOptions();

    this.subscriptions.push(this.Data.loadDriverHeader$().subscribe(() => {
      if (risk?.driverHeader) {
        this.Data.initiateHeaderValues(risk.driverHeader);
        this.Data.currMVRHeaderDate = this.Data.formDriverHeader.controls['mvrHeaderDate'].value;
      } else {
        this.Data.formDriverHeader.controls['mvrHeaderDate'].patchValue(this.Data.formatDateForPicker(risk?.brokerInfo?.effectiveDate));

        /*
        this.Data.triggerHeaderUpdate(new DriverHeaderDto(this.Data.formDriverHeader.value)).then((driverHeader) => {
          this.submissionData.riskDetail.driverHeader = driverHeader;
          this.Data.initiateHeaderValues(driverHeader);
          this.Data.currMVRHeaderDate = this.Data.formDriverHeader.controls['mvrHeaderDate'].value;
        }).catch((obj) => { }).finally(() => { });
        */
      }
    }));

    setTimeout(() => {
      this.Data.getDefaultPolicyState(risk?.id).then((state) => {
      });
    }, 1500);

    this.Data.sortByCreatedDate;
    this.getBlacklistedDrivers();
    this.ReloadDriver();
  }

  ngOnDestroy() {
    this.subscriptions.forEach(subs => subs.unsubscribe());
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
    if (!this.policySummaryData.canEditPolicy) { return; }
    NotifUtils.showConfirmMessage(`Are you sure you want to delete '${driver?.firstName} ${driver?.lastName}'?`, () => {
      Utils.blockUI();
      this.Data.delete(driver.id).pipe(take(1)).subscribe(data => {
        this.Data.renderDriverList().subscribe(() => {
          Utils.unblockUI();
          this.Data.setPage(this.Data.currentPage);
          this.Data.sortWithProperty();
        });
        this.policySummaryData.updateSubtatusForPendingEndorsement();
        //this.searchFilter(null);
      }, (error) => {
        //this.searchFilter(null);
        this.Data.setPage(this.Data.currentPage);
        this.Data.sortWithProperty();
        NotifUtils.showError('There was an error trying to remove a record. Please try again.');
      });
    });
  }

  editDriver(driver: DriverDto) {
    if (!this.policySummaryData.canEditPolicy) { return; }
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

  ReloadDriver()
  {
    if (!this.Data.driverInfoList?.length) {
      const policyEffectiveDate = this.submissionData.riskDetail?.brokerInfo?.effectiveDate ?? new Date(this.submissionData.currentServerDateTime);
      this.Data.driverInfoList.forEach(d => {
        d.age= this.Data.calculateAge(d.birthDate, policyEffectiveDate)?.toString(); });
       
    }
  }
  addNewDriver() {
    this.modalHeader = 'Add Driver Details';
    this.modalButton = 'Add Driver';
    // get state from Applicant name address
    // if (!this.Data.driverInfoList?.length) {
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
        optValue = optValue.replace(/,(?=\s*$)/, '');
        drvr.options = optValue?.length === 0 ? null : optValue;
        drvr.riskDetailId = this.submissionData.riskDetail.id;
        this.Data.updateOption(drvr).pipe(take(1)).subscribe(data => {
          Object.assign(value, drvr);
          this.policySummaryData.updateSubtatusForPendingEndorsement();
        }, (error) => {
          drvr.options = oldOption;
          Object.assign(value, drvr);
          this.toastr.error('There was an error trying to Update a record. Please try again.', 'Success!');
        });
      }
      return true;
    });
  }

  passRatingChanged(drvr: DriverDto) {
    const menuDropdownOpen = document.querySelectorAll(`.optInlcude[id*='${drvr.id}']`) as any;
    const isPassRating = menuDropdownOpen[0].checked;
    const oldOption = drvr.options ?? "";
    this.Data.driverInfoList = this.Data.driverInfoList.filter((value, key) => {
      if (value.id === drvr.id) {
        const validateAgeOptions = this.Data.optAgeValidation(drvr);
        if (validateAgeOptions != null && !drvr.isExcludeKO) {
          NotifUtils.showWarning(validateAgeOptions.message);
          menuDropdownOpen[0].checked = false;
          return true;
        }

        let drvrOptions = oldOption?.split(',') ?? [];
        if (isPassRating) {
          drvrOptions.push(`${this.bindOption}`);
          drvrOptions.sort((a, b) => a < b ? -1 : 1);
        } else {
          drvrOptions = oldOption?.split(',').filter(a => a != `${this.bindOption}`);
        }

        drvr.options = (drvrOptions?.length === 0) ? null : drvrOptions?.join(',');
        /*
        if(drvr.options?.length === 0) {
          drvr.ageFactor = null;
          drvr.yearPoint = null;
          drvr.totalFactor = null;
        }
        */

        drvr.riskDetailId = this.submissionData.riskDetail.id;
        Utils.blockUI();
        this.Data.updateEndorsement(drvr)
        .pipe(
          take(1),
          switchMap(() => this.Data.renderDriverList()),
          finalize(() => Utils.unblockUI())
        ).subscribe(data => {
          this.Data.setPage(this.Data.currentPage);
          this.policySummaryData.updateSubtatusForPendingEndorsement();
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

  updateDriverExclusionForms() {
    if (this.submissionData.riskDetail.riskForms?.length > 0) {
      const optionId = this.submissionData.riskDetail?.binding?.bindOptionId ?? '1';
      const drivers = this.submissionData.riskDetail?.drivers;
      const form = this.submissionData.riskDetail.riskForms.find(x => x.form?.id === 'DriverExclusion');
      if (form != null) {
        const index = this.submissionData.riskDetail.riskForms.indexOf(form);
        this.submissionData.riskDetail.riskForms[index].isSelected = false;
        if (this.submissionData.riskDetail.hasInitialDriverExclusion || (drivers?.length > 0 && drivers.some(d => d.options == null || !d.options?.split(',')?.includes(optionId.toString())))) {
          this.submissionData.riskDetail.riskForms[index].isSelected = true;
        }
      }
    }
  }

  mvrChanged(drvr: DriverDto, isChecked) {
    const oldOption = drvr.options;
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
    this.Data.updateEndorsement(parseOBJ)
    .pipe(
      take(1),
      switchMap(() => this.Data.renderDriverList()),
      finalize(() => Utils.unblockUI())
    ).subscribe(data => {
      this.policySummaryData.updateSubtatusForPendingEndorsement();
      this.Data.setPage(this.Data.currentPage);
    }, (error) => {
      drvr.isMvr = !isChecked;
      drvr.options = oldOption;
      this.toastr.error('There was an error trying to Update MVR of record. Please try again.', 'Success!');
    });
  }

  outStateChanged(drvr: DriverDto, isChecked) {
    drvr.isOutOfState = isChecked;
    const oldState = drvr.state;

    if (drvr.isOutOfState == false) {
      drvr.state = this.Data.defaultPolicyState;
    } else {
      drvr.state = null;
    }

    this.Data.initiateFormValues(drvr);

    if (this.Data.formDriverInfo.invalid ) {
      drvr.isValidated = false;
    } else {
      drvr.isValidated = true;
    }
    console.log('update driver');
    Utils.blockUI();
    this.Data.updateEndorsement(drvr)
    .pipe(
      take(1),
      switchMap(() => this.Data.renderDriverList()),
      finalize(() => Utils.unblockUI())
    ).subscribe(data => {
      this.policySummaryData.updateSubtatusForPendingEndorsement();
      this.Data.setPage(this.Data.currentPage);
    }, (error) => {
      drvr.isOutOfState = !isChecked;
      drvr.state = oldState;
      this.toastr.error('There was an error trying to Update Out State of record. Please try again.', 'Success!');
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
        NotifUtils.showWarning(validateMVR);
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

  generateExcelReport() {
    Utils.blockUI();
    const submissionNo = this.submissionData?.riskDetail?.submissionNumber; 
    
    this.generateDriverExport((data) => {
      if (data.length >= 0) {
        const reportfileName = `BT Driver Midterm List_${submissionNo}`;
        const rows: any[] = [];
        rows.push(this.Data.reportPolicyHeaders); // add headers
        data.forEach(row => {
          const record = {
            firstName: row.firstName,
            middleName: row.middleName,
            lastName: row.lastName,
            licenseNumber: row.licenseNumber,
            state: row.state,
            birthDate: row.birthDate == null?'' : formatDate(row.birthDate, 'MM/dd/yyyy', 'en'),
            yrsCommDrivingExp: row.yrsCommDrivingExp,
            totalFactor: row.totalFactor,
            effectiveDate: row.effectiveDate == null?'' : formatDate(row.effectiveDate, 'MM/dd/yyyy', 'en'), 
            expirationDate: row.expirationDate == null?'' : formatDate(row.expirationDate, 'MM/dd/yyyy', 'en') 
          };
          rows.push(record);
        });
        ReportUtils.exportToCsv(reportfileName, rows);
      } 
    });
  }

 
  private generateDriverExport(callbackFn?: Function) {
    Utils.blockUI();
    const riskDetailId = this.submissionData.riskDetail?.id;
    this.driverService.getRiskExportDrivers(riskDetailId, true).pipe(takeUntil(this.stop$)).subscribe((data) => {
        Utils.unblockUI();
        if (callbackFn) { callbackFn(data); }
    }, err => {
        Utils.unblockUI();
        NotifUtils.showError(JSON.stringify(err));
    });
  }
  private mapTextToId(drivers: DriverDto[]): DriverDto[] {
    const lists = this.Data.driverDropdownsList;
    const policyEffectiveDate  = this.submissionData.riskDetail?.brokerInfo?.effectiveDate ?? new Date(this.submissionData.currentServerDateTime);
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

  driverExpandToggle(driver: DriverDto) {
    driver.isExpanded = !driver.isExpanded;
    if(!!driver.isExpanded) {
      if(driver.previousDrivers == null){
        // Load previous vehicle if not yet loaded.
        this.Data.loadPreviousDriver(driver).subscribe();
      }
    }
  }

  GetDriverEndorsementNumber(driver: DriverDto) {
    const details = { label: '', description: ''};
    const histories = this.Data.policyHistories;
    const history = histories?.find(x => x.previousRiskDetailId === driver.riskDetailId);

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

  reinstateDriver(obj: DriverDto) {
    if (!this.policySummaryData.canEditPolicy) { return; }
    NotifUtils.showConfirmMessage(`Are you sure you want to reinstate '${obj?.firstName} ${obj?.lastName}'?`, () => {
      Utils.blockUI();
      this.Data.reinstate(obj.id, true)
        .pipe(finalize(() => Utils.unblockUI()))
        .subscribe(() => {
          this.Data.renderDriverList().subscribe(() => {
            this.Data.setPage(this.Data.currentPage);
            this.Data.sortWithProperty();
          });
          this.policySummaryData.updateSubtatusForPendingEndorsement();
        }, (error) => {
        });
    });
  }

  viewDriver(driver: DriverDto) {
    this.modalHeader = 'View Driver Details';
    this.modalButton = 'View';
    driver = driver ?? new DriverDto();

    if (driver.isValidated === false) {
      driver.hireDate = driver.mvrDate = this.Data.formDriverHeader.get('mvrHeaderDate').value?.singleDate?.jsDate?.toLocaleDateString();
    }

    this.Data.initiateFormValues(driver, true);
    this.Data.driverIncidentList = driver.driverIncidents ?? Array<DriverIncidentDto>();

    Object.assign(this.Data.driverIncidentList_old, this.Data.driverIncidentList);

    this.fn_openDriverInfo();

    this.Data.enableKORule();
    this.driverInfoSec.isExcludeKOSwitch(driver.isExcludeKO);
    this.driverInfoSec.currMVRDate = this.Data.formDriverInfo.get('mvrDate').value;

    if (driver.isValidated === false) {
      this.driverInfoSec.ngOnChangesDOB(driver.birthDate);
    }

    if (this.fg.get('state').value == null && this.fg.get('isOutOfState').value == false) {
      this.fg.get('state').setValue(this.Data.getDriverState(driver));
    }
  }
  
  //#endregion
}
