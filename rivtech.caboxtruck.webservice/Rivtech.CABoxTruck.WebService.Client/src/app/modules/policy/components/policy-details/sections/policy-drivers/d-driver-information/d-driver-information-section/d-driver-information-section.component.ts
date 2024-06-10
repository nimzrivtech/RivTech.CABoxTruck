import { Component, EventEmitter, OnInit, Output, ViewChild } from '@angular/core';
import { DriverData } from '../../../../../../../../modules/submission/data/driver/driver.data';
import { DriverDto } from '../../../../../../../../shared/models/submission/Driver/DriverDto';
import Utils from '../../../../../../../../shared/utilities/utils';
import { Validators } from '@angular/forms';
import { finalize, map, take, tap } from 'rxjs/operators';
import NotifUtils from '../../../../../../../../shared/utilities/notif-utils';
// tslint:disable-next-line: import-blacklist
import { Subject } from 'rxjs/Rx';
import { DataTableDirective } from 'angular-datatables';
import { DriverIncidentDto } from '../../../../../../../../shared/models/submission/Driver/DriverIncidentDto';
import { Guid } from 'guid-typescript';
import { SubmissionData } from '../../../../../../../../modules/submission/data/submission.data';
import { DatePickerComponent } from '../../../../../../../../shared/components/dynamic/date-picker/date-picker.component';
import { Input } from '@angular/core';
import { TableConstants } from '../../../../../../../../shared/constants/table.constants';
import { PolicySummaryData } from '@app/modules/policy/data/policy-summary.data';


@Component({
  selector: 'app-d-driver-information-section',
  templateUrl: './d-driver-information-section.component.html',
  styleUrls: ['./d-driver-information-section.component.scss'],
})
export class DDriverInformationSectionComponent implements OnInit {
  @ViewChild('btnCloseModal') btnCloseModal;
  @ViewChild('btnAddIncident') btnAddIncident;

  @ViewChild('dtpIncidentDate') dtpIncidentDate: DatePickerComponent;
  @ViewChild('dtpconvictionDate') dtpconvictionDate: DatePickerComponent;

  @ViewChild('dtpbirthDate') dtpbirthDate: DatePickerComponent;
  @ViewChild('dtpmvrDate') dtpmvrDate: DatePickerComponent;
  @ViewChild('dtphireDate') dtphireDate: DatePickerComponent;

  @ViewChild('modalIncident') modalIncident;

  @ViewChild('btnSaveIncidentModal') btnSaveIncidentModal;

  // tslint:disable-next-line: no-output-rename
  @Output('btnModalDriverRef') btnModalDriverRef: EventEmitter<any> = new EventEmitter();

  @Input() modalButton: string;

  dtElement: DataTableDirective;
  dtOptions: DataTables.Settings = {};
  dtTrigger: Subject<any> = new Subject();
  isDtInitialized: boolean = false;

  showExcludeKO: boolean = false;
  currMVRDate: any;
  TableConstants = TableConstants;

  customId: string = 'birthDateId';
  submitted = false;
  constructor(
    public driverData: DriverData,
    private submissionData: SubmissionData,
    private policySummaryData: PolicySummaryData
  ) {
  }

  ngOnInit() {
    this.dtOptions = {
      pagingType: 'full_numbers',
      responsive: true,
      processing: true,
      destroy: true
    };

    this.renderTable();

    //
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

  get fc() { return this.fg.controls; }
  get fg() { return this.driverData.formDriverInfo; }

  onCancel() {
    if (this.modalIncident.nativeElement.classList.contains('show')) {
      this.btnAddIncident.nativeElement.click();
    }

    this.fg.markAsUntouched();
    // const driver: DriverDto = new DriverDto(this.driverData.formDriverInfo.value);
    // if (driver.id !== undefined && driver.id != null && driver.id !== '') {
    //   this.driverData.driverInfoList = this.driverData.driverInfoList.filter((value, key) => {
    //     if (value.id === driver.id) {
    //       value.driverIncidents = this.driverData.driverIncidentList_old;
    //     }
    //     return true;
    //   });

    // } else {
    //   this.driverData.driverIncidentList = this.driverData.driverIncidentList_old;
    // }
    this.driverData.driverIncidentList_old = Array<DriverIncidentDto>();
  }
  /////////
  ngOnChangesDOB(event) {
    const dateOfBirth = event;
    const policyEffectiveDate = this.submissionData.riskDetail?.brokerInfo?.effectiveDate ?? new Date(this.submissionData.currentServerDateTime);
    this.fg.get('age').setValue(this.driverData.calculateAge(dateOfBirth, policyEffectiveDate));
    this.showExcludeKO = this.driverData.enableKORule();
    this.isExcludeKOSwitch(this.fg.get('isExcludeKO').value);
  }

  ngOnChangesMVRDate(obj) {
    setTimeout(() => {
      const brokerEffectiveDate = this.submissionData?.riskDetail?.brokerInfo?.effectiveDate;
      let dateReverse = this.driverData.formatDateForPicker(null); // this will return current date.
      const mvrHeaderDate = this.driverData.formDriverHeader.get('mvrHeaderDate').value;

      if (this.currMVRDate) {
        dateReverse = this.currMVRDate;
      } else if (mvrHeaderDate) {
        dateReverse = mvrHeaderDate;
      }

      if (!obj.jsDate) {
        this.driverData.formDriverInfo.get('mvrDate').setValue(dateReverse);
      } else {
        const validateMVR = this.driverData.validateMVRdateEffective(obj.jsDate, brokerEffectiveDate);
        if (validateMVR?.length > 0) {
          NotifUtils.showWarning(validateMVR);
          this.driverData.formDriverInfo.get('mvrDate').setValue(dateReverse);
        } else {
          this.driverData.formDriverInfo.get('mvrDate').setValue(this.driverData.formatDateForPicker(obj.jsDate));
        }
      }

      const mvrDate = this.driverData.formDriverInfo.get('mvrDate').value;
      this.currMVRDate = mvrDate;
      this.driverData.formDriverInfo.get('hireDate').setValue(mvrDate);
    }, 50);
  }

  onCancelIncident() {
    this.driverData.formDriverIncident.markAsUntouched();
    this.btnAddIncident.nativeElement.click();
  }

  onSubmit(obj: MouseEvent) {
    obj.preventDefault();
    (obj.target as HTMLButtonElement).disabled = true;

    if (this.driverData.formDriverInfo.invalid) {
      this.driverData.formDriverInfo.markAllAsTouched();
      return;
    } else {
      Utils.blockUI();

      let validateAgeOptions: any = null;

      const driver: DriverDto = new DriverDto(this.driverData.formDriverInfo.value);
      driver.birthDate = this.driverData.formDriverInfo.get('birthDate').value?.singleDate?.jsDate?.toLocaleDateString();
      driver.mvrDate = this.driverData.formDriverInfo.get('mvrDate').value?.singleDate?.jsDate?.toLocaleDateString();
      driver.hireDate = this.driverData.formDriverInfo.get('hireDate').value?.singleDate?.jsDate?.toLocaleDateString();
      driver.riskDetailId = this.submissionData.riskDetail.id;
      
      if (this.driverData.formDriverInfo.controls.state.disable) {
        driver.state = this.driverData.formDriverInfo.controls.state.value;
      }

      // if (driver.isExcludeKO && !this.showExcludeKO) { driver.isExcludeKO = false; }

      if (driver.id !== undefined && driver.id != null && driver.id !== '') {
        this.driverData.driverInfoList = this.driverData.driverInfoList.filter((value, key) => {
          if (value.id === driver.id) {
            driver.stateId = driver.stateId?.toString();

            if (this.driverData.driverIncidentList?.length > 0) {
              driver.driverIncidents = this.driverData.driverIncidentList;
            }

            driver.isOutOfState = driver.isOutOfState ? true : false;
            driver.isExcludeKO = driver.isExcludeKO ? true : false;
            driver.options = value.options;

            validateAgeOptions = this.driverData.optAgeValidation(driver);

            //if (value.options) {
            if (validateAgeOptions != null && driver.isExcludeKO != true) {
              driver.options = null;
            } else {
              if (!value.options) driver.options = this.driverData.getOptionQuote;
            }
            // }

            driver.riskDetailId = this.submissionData.riskDetail?.id;
            driver.isValidated = true;
            driver.isValidatedKO = validateAgeOptions == null ? true : (driver.isExcludeKO && driver.koReason?.length > 0);
            driver['savedDrivers'] = [];
            Utils.blockUI();
            this.driverData.updateEndorsement(driver).pipe(take(1)).subscribe(data => {
              this.policySummaryData.updateSubtatusForPendingEndorsement();
              this.driverData.renderDriverList().subscribe(() => {
                Utils.unblockUI();
                this.driverData.setPage(this.driverData.currentPage);
                if (validateAgeOptions != null && driver.isExcludeKO != true) {
                  NotifUtils.showWarning(validateAgeOptions.message);
                }
              });
              this.btnModalDriverRef.emit();
              this.driverData.formDriverInfo.reset();
              (obj.target as HTMLButtonElement).disabled = false;

              // driver.id = data.id;
              // driver.driverIncidents = data.driverIncidents;
              // if (value.options) {
              //   if (validateAgeOptions != null && driver.isExcludeKO != true) {
              //     NotifUtils.showWarning(validateAgeOptions.message);
              //   }
              // }

              // Object.assign(value, driver);
              // this.submissionData.riskDetail.drivers = this.driverData.driverInfoList;
              // this.btnModalDriverRef.emit();
              // this.driverData.formDriverInfo.reset();
              // (obj.target as HTMLButtonElement).disabled = false;

              // this.driverData.setPage(this.driverData.currentPage);

            }, (error) => {
              this.driverData.setPage(this.driverData.currentPage);
              (obj.target as HTMLButtonElement).disabled = false;
              NotifUtils.showError('There was an error trying to Update a record. Please try again.');
            });
          }
          return true;
        });
      } else {
        if (driver.isExcludeKO && !this.showExcludeKO) { driver.isExcludeKO = false; }

        driver.riskDetailId = this.submissionData.riskDetail?.id;
        driver.createdBy = 12345;
        driver.createdDate = new Date();
        driver.stateId = driver.stateId?.toString();

        driver.include = true;
        driver.isMvr = true;

        if (this.driverData.driverIncidentList?.length > 0) {
          driver.driverIncidents = this.driverData.driverIncidentList;
        }

        driver.options = this.driverData.getOptionQuote;
        driver.isOutOfState = driver.isOutOfState ? true : false;
        driver.isExcludeKO = driver.isExcludeKO ? true : false;

        validateAgeOptions = this.driverData.optAgeValidation(driver);

        if (driver.options) {
          if (validateAgeOptions != null && driver.isExcludeKO != true) {
            driver.options = null;
          }
        }
        driver.isValidated = true;
        driver.isValidatedKO = validateAgeOptions == null ? true : (driver.isExcludeKO && driver.koReason?.length > 0);
        driver['savedDrivers'] = [];

        this.driverData.insert(driver).pipe(
          map(data => data as DriverDto),
          tap((data: any) => {
            (data as DriverDto).isMainRow = true;
            this.driverData.driverInfoList.push(data);
          })
        ).subscribe(data => {
          this.submissionData.riskDetail.drivers = this.driverData.driverInfoList;
          this.policySummaryData.updateSubtatusForPendingEndorsement();
          // show warning upon saving for driver ineligibiliy [NEWCABT-1268]
          if (validateAgeOptions != null && driver.isExcludeKO != true) {
            NotifUtils.showWarning(validateAgeOptions.message, () => {
              Utils.unblockUI();
              this.driverData.setPage(this.driverData.currentPage, true);
            });
          } else {
            Utils.unblockUI();
            this.driverData.setPage(this.driverData.currentPage, true);
          }

          this.btnModalDriverRef.emit();
          this.driverData.formDriverInfo.reset();
          (obj.target as HTMLButtonElement).disabled = false;
        }, (error) => {
          this.driverData.setPage(this.driverData.currentPage);
          (obj.target as HTMLButtonElement).disabled = false;
          NotifUtils.showError('There was an error trying to insert a new record. Please try again.');
        });
      }

      Utils.unblockUI();
      return;
    }
  }

  onSwitchClick(obj) {
  }

  outStateDriverSwitch(value: any, newInitLoad?: boolean) {

    if (value == false) {
      if (!newInitLoad) {
        this.fc['state'].patchValue(this.driverData.defaultPolicyState);
      }

      this.fc['state'].disable();
    } else {
      this.fc['state'].setValue(null);
      this.fc['state'].enable();
    }
    this.driverData.getStatesFiltered(true, this.fc.isOutOfState.value);
  }

  isExcludeKOSwitch(value: any) {
    if (value) {
      this.addValidators(['koReason'], this.fc);
    } else {
      this.clearValidators(['koReason'], this.fc);
      this.fc['koReason'].setValue(null);
    }
  }

  onChangeState(value: any) {
  }

  onTextChange(obj, value) {
    if (value) {
      //obj.clearValidators();
      // obj.updateValueAndValidity();
    } else {
      //obj.setValidators([Validators.required], this.driverData.driverMaxYrsExp(63));
      //obj.updateValueAndValidity();
    }
  }

  onYrsDriveChange(obj, value) {
    //obj.clearValidators();
    //obj.updateValueAndValidity();

    if (value != '') {
      this.fc['yrsCommDrivingExp'].patchValue(value);
    }
  }

  resetDatePicker() {
    this.dtpbirthDate.val.reset();
    this.dtpmvrDate.val.reset();
    this.dtphireDate.val.reset();

    this.dtpIncidentDate.val.reset();
    if (this.dtpconvictionDate) { this.dtpconvictionDate.val.reset(); }
  }

  //#region  INCIDENT CODES
  addNewIncident() {
    this.driverData.initiateFormIncident(new DriverIncidentDto());
    this.driverData.formDriverIncident.markAsUntouched();
    this.driverData.formDriverIncident.reset();

    setTimeout(() => {
      this.btnAddIncident.nativeElement.click();
    }, 200);

    if (this.driverData.formDriverIncident.valid) {
      this.btnSaveIncidentModal.nativeElement.disabled = false;
    }

    this.dtpIncidentDate.val.reset();
    if (this.dtpconvictionDate) { this.dtpconvictionDate.val.reset(); }


    setTimeout(() => {
      // const elementReference = document.querySelector('[id=incidentType]');
      // if (elementReference instanceof HTMLElement) {
      //   elementReference.focus();
      // }
    }, 500);
  }

  editIncident(driverIncident) {
    this.btnSaveIncidentModal.nativeElement.disabled = false;

    this.driverData.formDriverIncident.patchValue(driverIncident ?? new DriverIncidentDto());
    this.onChangeIncidentType(this.driverData.formDriverIncident.controls.incidentType.value);

    if (driverIncident.incidentDate) { this.driverData.formDriverIncident.get('incidentDate').setValue({ isRange: false, singleDate: { jsDate: new Date(driverIncident?.incidentDate) } }); }
    if (driverIncident.convictionDate) { this.driverData.formDriverIncident.get('convictionDate').setValue({ isRange: false, singleDate: { jsDate: new Date(driverIncident?.convictionDate) } }); }

    this.btnAddIncident.nativeElement.click();

    setTimeout(() => {
      // const elementReference = document.querySelector('[id=incidentType]');
      // if (elementReference instanceof HTMLElement) {
      //   elementReference.focus();
      // }
    }, 500);
  }

  deleteIncident(driverIncident: DriverIncidentDto) {
    let obj = this.driverData.driverDropdownsList.incidentTypeList.find(i => i.value == driverIncident.incidentType).label ?? "";

    NotifUtils.showConfirmMessage(`Are you sure you want to delete '${obj}'?`, () => {
      this.driverData.driverIncidentList = this.driverData.driverIncidentList.filter(t => t.id !== driverIncident.id);
      this.showExcludeKO = this.driverData.enableKORule();
      this.isExcludeKOSwitch(this.fg.get('isExcludeKO').value);
    });
  }

  onSubmitIncident(obj: MouseEvent) {
    obj.preventDefault();
    (obj.target as HTMLButtonElement).disabled = true;

    if (this.driverData.formDriverIncident.invalid) {
      this.driverData.formDriverIncident.markAllAsTouched();
      return;
    } else {

      Utils.blockUI();
      const driverIncident: DriverIncidentDto = new DriverIncidentDto(this.driverData.formDriverIncident.value);
      if (driverIncident.incidentType === '4' || driverIncident.incidentType === '6') {
        driverIncident.convictionDate = null;
      }
      const value = this.driverData.driverIncidentList.find(x => x.id == driverIncident.id);
      if (value != undefined) {
        this.driverData.driverIncidentList = this.driverData.driverIncidentList.filter((value, key) => {
          if (value.id === driverIncident.id) {
            if (driverIncident.incidentDate) { driverIncident.incidentDate = (driverIncident.incidentDate as any).singleDate?.jsDate?.toLocaleDateString(); }
            if (driverIncident.convictionDate) { driverIncident.convictionDate = (driverIncident.convictionDate as any).singleDate?.jsDate?.toLocaleDateString(); }
            Object.assign(value, driverIncident);
          }
          return true;
        });
      } else {
        driverIncident.id = Guid.create().toString();
        if (driverIncident.incidentDate) { driverIncident.incidentDate = (driverIncident.incidentDate as any).singleDate?.jsDate?.toLocaleDateString(); }
        if (driverIncident.convictionDate) { driverIncident.convictionDate = (driverIncident.convictionDate as any).singleDate?.jsDate?.toLocaleDateString(); }
        this.driverData.driverIncidentList.push(driverIncident);
      }

      this.showExcludeKO = this.driverData.enableKORule();
      this.isExcludeKOSwitch(this.fg.get('isExcludeKO').value);
      this.btnAddIncident.nativeElement.click();
      Utils.unblockUI();
      return;
    }
  }

  getIncidentTypeText(id: number) {
    const obj = this.driverData.driverDropdownsList.incidentTypeList.find(l => l.value === id)?.label ?? '';
    return obj;
  }

  onChangeIncidentType(value) {
    // '4' == 'At-Fault'
    // '6' == 'Not at Fault'
    if (!this.driverData.IncidentTypeConviction(value)) {
      this.driverData.initiateFormIncident(this.driverData.formDriverIncident.value);
    } else {
      this.clearValidators(['convictionDate'], this.driverData.formDriverIncident.controls);
    }
  }
  //#endregion  INCIDENT CODES

  clearValidators(controlName: string[], formcontrol: any) {
    controlName.forEach(eachControl => {
      formcontrol[eachControl].clearValidators();
      formcontrol[eachControl].updateValueAndValidity();
    });
  }

  addValidators(controlName: string[], formcontrol: any) {
    controlName.forEach(eachControl => {
      formcontrol[eachControl].setValidators([Validators.required]);
      formcontrol[eachControl].markAsUntouched();
      formcontrol[eachControl].markAsPristine();
      formcontrol[eachControl].updateValueAndValidity();
    });
  }

  //#region Function Helpers
  numbersOnly(event: any) {
    const pattern = /[0-9\+\-\ ]/;

    const inputChar = String.fromCharCode(event.charCode);
    if (event.keyCode != 8 && !pattern.test(inputChar)) {
      event.preventDefault();
    }
  }

  alphaNumeric(event: any) {
    //const pattern = /^$|^[A-Za-z0-9 .]+/; use this or below
    // const pattern = /^[\w\-\s.]+$/
    // const inputChar = String.fromCharCode(event.charCode);
    // if (event.keyCode != 8 && !pattern.test(inputChar)) {
    //   event.preventDefault();
    // }
    //from qa, they want free form field to enter any special characters
  }

  // @HostListener('paste', ['$event']) blockPaste(e: KeyboardEvent) {
  //   e.preventDefault(); // prevent paste on all controls to prevent invalid entry.
  // }
  //#endregion Function Helpers

  get isViewOnly(): boolean {
    return this.modalButton === 'View';
  }
}
