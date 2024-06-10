import { Component, Input, OnInit } from '@angular/core';
import { SubmissionData } from '../../../../../../../../modules/submission/data/submission.data';
import { DriverData } from '../../../../../../../../modules/submission/data/driver/driver.data';
import { DriverDto } from '../../../../../../../../shared/models/submission/Driver/DriverDto';
import { ToastrService } from 'ngx-toastr';
import { Guid } from 'guid-typescript';
import Utils from '@app/shared/utilities/utils';
import { DriverService } from '@app/modules/submission/services/driver.service';
import { takeUntil } from 'rxjs/operators';
import { BaseClass } from 'app/shared/base-class';
import NotifUtils from '../../../../../../../../shared/utilities/notif-utils';
import * as moment from 'moment';
@Component({
  selector: 'app-driver-excel-upload',
  templateUrl: './driver-excel-upload.component.html',
  styleUrls: ['./driver-excel-upload.component.scss']
})

export class DriverExcelUploadComponent extends BaseClass implements OnInit {
  private savedDrivers: Array<DriverDto> = new Array<DriverDto>();
  private currDrivers : Array<DriverDto> = new Array<DriverDto>();
  @Input() excelDrivers: DriverDto[] = [];

  constructor(private driverData: DriverData, private submissionData: SubmissionData, private toastr: ToastrService,
              private driverService: DriverService) { super(); }

  ngOnInit(): void {
    
  }

  onSubmit() {
    this.savedDrivers = [];
    this.currDrivers=[];

    if (!this.excelDrivers?.length) {
      return this.toastr.success('No Record found to be saved!', 'Success!');
    }

    this.driverData.getAllBlacklisterDriver(() => {
      //#region remap statefull to states
      this.excelDrivers.forEach(d => {
        const state = this.driverData.driverDropdownsList.stateList.find(f => f.value.toLowerCase() === d.stateFull?.toLowerCase() || f.label.toLowerCase() === d.stateFull?.toLowerCase())?.value;
        const validateAgeOptions = this.driverData.optAgeValidation(d);

        d.state = state== undefined? '' :state;
        d.isOutOfState = (state === this.driverData.defaultPolicyState || state?.length == 0) ? false : true;
        d.id = Guid.create().toString();
        d.riskDetailId = this.submissionData.riskDetail?.id;

        if (validateAgeOptions != null && d.isExcludeKO != true) {
          d.options = null;
        } else {
          d.options = this.driverData.getOptionQuote;
        }

        if (d.age != null && Number(d.age) <= 22) {
          d.options = '';
        }

        this.driverData.formDriverInfo.reset();
        this.driverData.formDriverInfo.controls['state'].enable();
        this.driverData.initiateFormValues(d);

        this.savedDrivers.push(d);
        if (this.validateDuplicates(d)) {
          d.isValidated = false;
        } else {
          d.isValidated = true;
        }
      });
      //#endregion  remap statefull to states

      this.driverData.insertDrivers(this.excelDrivers).subscribe(data => {
        this.savedDrivers  = new Array<DriverDto>();
        this.currDrivers = [];
        this.driverData.driverInfoList = new Array<DriverDto>();
        data.forEach(d => {
         
          if (this.validateDuplicates(d)) {
            d.isValidated = false;
          } else {
            d.isValidated = true;
          }

          this.driverData.driverInfoList.push(new DriverDto(d));
          this.submissionData.riskDetail.drivers = this.driverData.driverInfoList;

        });
        this.toastr.success('Upload Successful!', 'Success!');
        this.driverData.setPage(this.driverData.currentPage, true, data.length);
        this.driverData.changeShowRowsNumber(this.driverData.currentPage, 10, true);
        this.showValidatedDriverMessage(data);
      }, (error) => {
        this.toastr.error('Error encounter while saving the data. Please try again.', 'Failed!');
      });
    });
  }

  showValidatedDriverMessage(drivers: DriverDto[]) {
    const latestWithKO = drivers.reduce((latestObj: DriverDto | null, obj: DriverDto) => {
      const validateAgeOptions = this.driverData.optAgeValidation(obj);
      if (validateAgeOptions) {
          if (!latestObj || obj.createdDate > latestObj.createdDate) {
              NotifUtils.showWarning(validateAgeOptions.message);
              return obj;
          }
      }
      return latestObj;
    }, null);
  }
  
  reloadtable() {
    const policyEffectiveDate = this.submissionData.riskDetail?.brokerInfo?.effectiveDate ?? new Date(this.submissionData.currentServerDateTime);
    this.getDriver((data) => {
      if (data.length > 0) {
          this.driverData.driverInfoList = data;
          this.driverData.driverInfoList.forEach(d => {
            d.age= this.driverData.calculateAge(d.birthDate, policyEffectiveDate)?.toString(); });
        } 
        else {
          this.driverData.driverInfoList = Array<DriverDto>();
        }
        this.driverData.setPage(1);
    });
  }

   getDriver(callbackFn?: Function) {
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
  
  validateDuplicates(_drvr: DriverDto): boolean {
  //const currDrivers = Object.assign([], this.submissionData?.riskDetail?.drivers);
  //const currDrivers = Object.assign([], new Array<DriverDto>());
    const policyEffectiveDate = this.submissionData.riskDetail?.brokerInfo?.effectiveDate ?? new Date(this.submissionData.currentServerDateTime);
    const firstName: string = _drvr['firstName'] ?? '';
    const lastName: string = _drvr['lastName'] ?? '';
    const licenseNumber: string = _drvr['licenseNumber'] ?? '';
    const state: string = _drvr['state'] ?? '';

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

      const drvr3 = this.driverData.blacklistedDrivers?.find(s => s.driverLicenseNumber?.toLowerCase()?.trim() === licenseNumber?.toLowerCase()?.trim() &&
        s.lastName?.toLowerCase()?.trim() === lastName?.toLowerCase()?.trim() && s.isActive === true);

      if (drvr3 && drvr3?.id != _drvr?.id && (licenseNumber.length > 0)) {
        duplicateBlacklistedDriverId = true;
      } else {
        duplicateBlacklistedDriverId = false;
      }
      //#endregion Validate Blacklisted Driver
      this.currDrivers.push(_drvr);
      return (duplicateDriverId === true || duplicateBlacklistedDriverId === true||validateEmpty);
    }
    else {
      
      const drvr3 = this.driverData.blacklistedDrivers?.find(s => s.driverLicenseNumber?.toLowerCase()?.trim() === licenseNumber?.toLowerCase()?.trim() &&
      s.lastName?.toLowerCase()?.trim() === lastName?.toLowerCase()?.trim() && s.isActive === true);

    if (drvr3 && drvr3?.id != _drvr?.id && (licenseNumber.length > 0)) {
      duplicateBlacklistedDriverId = true;
    } else {
      duplicateBlacklistedDriverId = false;
    }
      this.currDrivers.push(_drvr);
      return (false || duplicateBlacklistedDriverId === true||validateEmpty);
    }
   
  }

  private isEmpty(str) {
    return (!str || str.length === 0 );
  }

  calculateAge(dateOfBirth: any, policyEffectiveDate: any) {
    const dateEntry = new Date(dateOfBirth);
    const policyDate = policyEffectiveDate!='Invalid Date' ? new Date(policyEffectiveDate) : new Date();
    const yearDiff = moment(policyDate.toDateString()).diff(moment(dateEntry.toDateString()), 'years');
   
    return dateOfBirth ? Math.abs(yearDiff) : 0;
  }

}
