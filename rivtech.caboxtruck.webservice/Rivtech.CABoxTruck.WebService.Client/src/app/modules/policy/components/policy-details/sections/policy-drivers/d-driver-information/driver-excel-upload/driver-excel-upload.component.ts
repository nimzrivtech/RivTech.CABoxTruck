import { Component, Input, OnInit } from '@angular/core';
import { SubmissionData } from '../../../../../../../../modules/submission/data/submission.data';
import { DriverData } from '../../../../../../../../modules/submission/data/driver/driver.data';
import { DriverDto } from '../../../../../../../../shared/models/submission/Driver/DriverDto';
import { ToastrService } from 'ngx-toastr';
import { Guid } from 'guid-typescript';
import { PolicySummaryData } from '@app/modules/policy/data/policy-summary.data';

@Component({
  selector: 'app-driver-excel-upload',
  templateUrl: './driver-excel-upload.component.html',
  styleUrls: ['./driver-excel-upload.component.scss']
})

export class DriverExcelUploadComponent implements OnInit {
  private savedDrivers: Array<DriverDto> = new Array<DriverDto>();
  @Input() excelDrivers: DriverDto[] = [];

  constructor(private driverData: DriverData,
    private submissionData: SubmissionData,
    private policySummaryData: PolicySummaryData,
    private toastr: ToastrService) { }

  ngOnInit(): void {
  }

  onSubmit() {
    this.savedDrivers = [];

    if (!this.excelDrivers?.length) {
      return this.toastr.success('No Record found to be saved!', 'Success!');
    }

    //#region remap statefull to states
    this.excelDrivers.forEach(d => {
      const state = this.driverData.driverDropdownsList.stateList.find(f => f.value.toLowerCase() === d.stateFull?.toLowerCase() || f.label.toLowerCase() === d.stateFull?.toLowerCase())?.value;
      d.state = state;
      d.isOutOfState = (state === this.driverData.defaultPolicyState || state?.length == 0) ? false : true;
      d.id = Guid.create().toString();
      d.riskDetailId = this.submissionData.riskDetail?.id;
      d.options = this.driverData.getOptionQuote;

      if (d.age != null && Number(d.age) <= 22) {
        d.options = '';
      }

      this.driverData.formDriverInfo.reset();
      this.driverData.formDriverInfo.controls['state'].enable();
      this.driverData.initiateFormValues(d);

      this.savedDrivers.push(d);
      if (this.driverData.formDriverInfo.invalid || this.validateDuplicates(d)) {
        d.isValidated = false;
      } else {
        d.isValidated = true;
      }
    });
    //#endregion  remap statefull to states

    this.driverData.insertDrivers(this.excelDrivers).subscribe(data => {
      this.policySummaryData.updateSubtatusForPendingEndorsement();
      // this.driverData.renderDriverList().subscribe(() => {
      //   this.driverData.setPage(this.driverData.currentPage);
      // });
      data.forEach(d => {
        this.driverData.driverInfoList.push(new DriverDto(d));
        this.submissionData.riskDetail.drivers = this.driverData.driverInfoList;
      });
      this.toastr.success('Upload Successful!', 'Success!');
      this.driverData.setPage(this.driverData.currentPage, true, data.length);
      this.driverData.changeShowRowsNumber(this.driverData.currentPage, 10, true);
    }, (error) => {
      this.toastr.error('Error encounter while saving the data. Please try again.', 'Failed!');
    });
  }

  validateDuplicates(_drvr: DriverDto): boolean {
    const currDrivers = Object.assign([], this.submissionData?.riskDetail?.drivers);

    const firstName: string = _drvr['firstName'] ?? '';
    const lastName: string = _drvr['lastName'] ?? '';
    const licenseNumber: string = _drvr['licenseNumber'] ?? '';
    const state: string = _drvr['state'] ?? '';

    let duplicateName: boolean = false;
    let duplicateDriverId: boolean = false;
    let duplicateBlacklistedDriverId: boolean = false;

    if (Object.assign([], currDrivers, this.savedDrivers)) {

      //#region validate Name
      // const drvr1 = Object.assign([], currDrivers, this.savedDrivers)?.find(s =>
      // (s.firstName?.toLowerCase()?.trim() === firstName?.toLowerCase()?.trim() &&
      //   s.lastName?.toLowerCase()?.trim() === lastName?.toLowerCase()?.trim()));

      // if (drvr1 && drvr1?.id != _drvr?.id && (lastName.length > 0 || firstName.length > 0 || licenseNumber.length > 0)) {
      //   duplicateName = true;
      // } else {
      //   duplicateName = false;
      // }
      //#endregion validate Name

      //#region Validate Driver Id
      const drvr2 = Object.assign([], currDrivers, this.savedDrivers)?.find(s => s.licenseNumber?.toLowerCase()?.trim() === licenseNumber?.toLowerCase()?.trim() && 
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

      return (duplicateDriverId === true || duplicateBlacklistedDriverId === true);
    }
  }
}
