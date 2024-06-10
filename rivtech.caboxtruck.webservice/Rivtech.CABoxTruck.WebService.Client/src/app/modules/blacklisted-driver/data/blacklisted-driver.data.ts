import {Injectable} from '@angular/core';
import {BlacklistedDriverDTO} from '../../../shared/models/blacklisted-driver/blacklisted-driver';
import {FormBuilder, FormGroup, Validators} from '@angular/forms';
import {BlacklistedDriverService} from '../../dashboard/services/blacklisted-driver.service';
import {BaseClass} from '../../../shared/base-class';
import {finalize, take, takeUntil} from 'rxjs/operators';
import Utils from '../../../shared/utilities/utils';
import {PagerService} from '@app/core/services/pager.service';
import {TableData} from '@app/modules/submission/data/tables.data';
import {Subject} from 'rxjs-compat';
import {Observable} from 'rxjs';
import { DriverData } from '@app/modules/submission/data/driver/driver.data';
import { DriverDto } from '@app/shared/models/submission/Driver/DriverDto';
import { SubmissionData } from '@app/modules/submission/data/submission.data';
import { Console } from 'console';

@Injectable()

export class BlacklistedDriverData extends BaseClass {
  blacklistedDrivers: BlacklistedDriverDTO[] = [];
  blackListedDriverForm: FormGroup;
  drivers: DriverDto[] = [];
  existingActiveDriverErrorMessage: string = '';
  policyNumberWithExistingBlacklistedDriver: string = '';

  searchFilter: string;
  pager: any = {};
  currentPage: number = 1;
  pagedItems: BlacklistedDriverDTO[];
  loading: boolean;
  mode: string = '';

  private subject = new Subject<any>();

  constructor(private fb: FormBuilder,
              private blacklistedDriverService: BlacklistedDriverService,
              private pagerService: PagerService,
              private driverData: DriverData,
              private submissionData: SubmissionData,
              private tableData: TableData) {
    super();
  }

  initializeFormFields(): FormGroup {
    this.blackListedDriverForm = this.fb.group(
      {
        id: [null],
        firstName: [null, Validators.required],
        lastName: [null, Validators.required],
        driverLicenseNumber: [null, Validators.required],
        isActive: [true, Validators.required],
        comments: [null]
      },
      {
        validators: [
          this.validateDuplicateDriverName(),
          this.validateDuplicateLicenseNumber(),
          this.validateActivatedBlacklistedDriver()
        ]
      }
    );
    return this.blackListedDriverForm;
  }

  get formValidateRequired() {
    return this.validateRequired();
  }

  sendCloseAddDriverEvent() {
    this.subject.next();
  }

  getCloseAddDriverEvent(): Observable<any> {
    return this.subject.asObservable();
  }

  validateRequired() {
    return this.blackListedDriverForm.controls['firstName'].valid
      && this.blackListedDriverForm.controls['lastName'].valid
      && !this.blackListedDriverForm.errors?.invalidDuplicateDriverName
      && !this.blackListedDriverForm.errors?.invalidDuplicateDriverId
      && !this.blackListedDriverForm.errors?.unableToActivateBlacklistDriver
      && this.blackListedDriverForm.controls['driverLicenseNumber'].valid;
  }

  populateFields(blacklistedDriver: BlacklistedDriverDTO): void {
    this.blackListedDriverForm.controls['id'].setValue(blacklistedDriver.id);
    this.blackListedDriverForm.controls['firstName'].setValue(blacklistedDriver.firstName);
    this.blackListedDriverForm.controls['lastName'].setValue(blacklistedDriver.lastName);
    this.blackListedDriverForm.controls['driverLicenseNumber'].setValue(blacklistedDriver.driverLicenseNumber);
    this.blackListedDriverForm.controls['isActive'].setValue(blacklistedDriver.isActive);
    this.blackListedDriverForm.controls['comments'].setValue(blacklistedDriver.comments);
  }

  update(blacklistedDriverDTO: BlacklistedDriverDTO) {
    return this.blacklistedDriverService.post(blacklistedDriverDTO);
  }

  searchDriver(driverName: string, driverLicenseNumber: string) {
    Utils.blockUI();
    return this.blacklistedDriverService.searchDriver(driverName, driverLicenseNumber).pipe(finalize( () => Utils.unblockUI()), takeUntil(this.stop$)).subscribe((result) => {
      this.blacklistedDrivers = result;
      this.setPage(this.currentPage, false);
    });
  }

  setPage(page: number, hasAdded: boolean = false) {
    if (page < 1) {
      return;
    }

    let blacklistedDrivers = Object.assign([], this.blacklistedDrivers);
    if (this.searchFilter) {
      blacklistedDrivers = this.blacklistedDrivers.filter(v => v.lastName?.trim().toLocaleLowerCase().includes(this.searchFilter.toLocaleLowerCase())
        || v.firstName?.trim().toLocaleLowerCase().includes(this.searchFilter.toLocaleLowerCase())
        || v.driverLicenseNumber?.trim().toLocaleLowerCase().includes(this.searchFilter.toLocaleLowerCase())) as any[];
    }

    this.pager = this.pagerService.getPager(blacklistedDrivers.length, page);

    if (hasAdded) {
      this.currentPage = this.pager.currentPage = this.pager.totalPages;
      this.pager.endIndex = this.pager.totalItems;
      this.pager.startIndex = ((this.pager.currentPage * this.pager.pageSize) - this.pager.pageSize);

      this.pagedItems = blacklistedDrivers.slice(this.pager.startIndex, this.pager.endIndex + 1);
    } else {
      while (this.pager.totalItems <= ((this.pager.currentPage * this.pager.pageSize) - this.pager.pageSize)) {
        this.pager.currentPage--;
        this.pager.startIndex = ((this.pager.currentPage * this.pager.pageSize) - this.pager.pageSize);
      }

      this.pagedItems = blacklistedDrivers.sort((a, b) => a?.firstName.localeCompare(b?.firstName)).slice(this.pager.startIndex, this.pager.endIndex + 1);
      this.currentPage = page;
    }

    this.mode = ''; // reset mode after adding/updating
  }

  sort(targetElement) {
    const blacklistedDrivers = Object.assign([], this.blacklistedDrivers);
    const sortData = this.tableData.sortPagedData(targetElement, blacklistedDrivers, this.currentPage);
    this.pagedItems = sortData.pagedItems;
    this.blacklistedDrivers = sortData.sortedData;
  }

  validateDuplicateDriverName() {
    // const currentBlacklistedDrivers = this.blacklistedDriverData.blacklistedDrivers;
    // const searchKeyword = (blackListedDriver.firstName.trim() + blackListedDriver.lastName.trim()).toLowerCase();
    // const filterBlacklistedDriver = currentBlacklistedDrivers.filter(bld => (bld.firstName.trim() + bld.lastName.trim()).toLowerCase().search(searchKeyword) > -1 && bld.isActive);
    // const hasMatchedDriverName = filterBlacklistedDriver.length > 0;
    // return hasMatchedDriverName;
    return (form: FormGroup) => {
      const savedDrivers = this.blacklistedDrivers;
      const firstName: string = form.controls['firstName'].value ?? '';
      const lastName: string = form.controls['lastName'].value ?? '';
      const licenseNumber: string = form.controls['driverLicenseNumber'].value ?? '';

      if (savedDrivers.length > 0) {
        const drvr = savedDrivers?.find(s =>
         (s.firstName?.toLowerCase()?.trim() === firstName?.toLowerCase()?.trim() &&
          s.lastName?.toLowerCase()?.trim() === lastName?.toLowerCase()?.trim()));

        if (drvr && drvr?.id != form.controls?.id?.value && (lastName.length > 0 || firstName.length > 0 || licenseNumber.length > 0)) {
          return { 'invalidDuplicateDriverName': true };
        } else {
          return null;
        }
      }
    };
  }

  validateDuplicateLicenseNumber() {
    // const currentBlacklistedDrivers = this.blacklistedDriverData.blacklistedDrivers;
    // const searchKeyword = blackListedDriver.driverLicenseNumber.toLowerCase();
    // const filterBlacklistedDriver = currentBlacklistedDrivers.filter(bld => (bld.driverLicenseNumber).toLowerCase().search(searchKeyword) > -1 && bld.isActive);
    // const hasMatchedLicenseNumber = filterBlacklistedDriver.length > 0;
    // return hasMatchedLicenseNumber;

    return (form: FormGroup) => {
      const savedDrivers = this.blacklistedDrivers;
      const licenseNumber: string = form.controls['driverLicenseNumber'].value ?? '';
      if (savedDrivers.length > 0) {
        // const drvr = savedDrivers?.find(s => s.firstName?.toLowerCase()?.trim() === firstName?.toLowerCase()?.trim() && s.lastName?.toLowerCase()?.trim() === lastName?.toLowerCase()?.trim());
        const drvr = savedDrivers?.find(s => s.driverLicenseNumber?.toLowerCase()?.trim() === licenseNumber?.toLowerCase()?.trim());

        if (drvr && drvr?.id != form.controls?.id?.value && (licenseNumber.length > 0)) {
          return { 'invalidDuplicateDriverId': true };
        } else {
          return null;
        }
      }
    };
  }

  validateActivatedBlacklistedDriver() {
    return (form: FormGroup) => {
      const savedDrivers = this.drivers;
      const blacklistedDriverStatus: boolean = form.controls['isActive'].value ?? false;
      const blacklistedDriverLicenseNumber: string = form.controls['driverLicenseNumber'].value ?? '';
      const blacklistedDriverLastName: string = form.controls['lastName'].value ?? '';
      
      if (blacklistedDriverStatus) {

        if(blacklistedDriverLastName !== '' && blacklistedDriverLicenseNumber !== '')
        {
          this.driverData.fetchDriversByLastNameAndLicenseNumber(blacklistedDriverLastName, blacklistedDriverLicenseNumber).pipe(take(1)).subscribe((data) => {
            const existingDrivers = data as DriverDto[];

            var submissionNumbers = existingDrivers.filter(function(s) {
              if (s.policyNumber == null && s.submissionNumber != null) {
                return true;
              }
              return false;
            }).map(function(s) { return s.submissionNumber; });

            var policyNumbers = existingDrivers.filter(function(s) {
              if (s.policyNumber != null && s.submissionNumber != null) {
                return true;
              }
              return false;
            }).map(function(s) { return s.policyNumber; });

            const driversSubmissionNumbers = submissionNumbers.join(', ');
            const driversPolicyNumbers = policyNumbers.join(', ');

            if(existingDrivers?.length > 0)
            {
              let submissionNumberMessage = '';
              let policyNumberMessage = '';
              
              if(submissionNumbers?.length > 1)
              {
                submissionNumberMessage = `submissions ${driversSubmissionNumbers}`;
              }
              else if(submissionNumbers?.length == 1)
              {
                submissionNumberMessage = `submission ${driversSubmissionNumbers}`;
              }
              
              if(policyNumbers?.length > 1)
              {
                policyNumberMessage = ` and policies ${driversPolicyNumbers}`;
              }
              else if(policyNumbers?.length == 1)
              {
                policyNumberMessage = ` and policy ${driversPolicyNumbers}`;
              }

              this.existingActiveDriverErrorMessage = `There is a driver on ${submissionNumberMessage}${policyNumberMessage} with the same Last Name and Driver's License Number`;
            }                                                 
          });
        }
        
        const isDriverUsedInSubmissions = savedDrivers?.find(s => s?.isValidated && s?.options != null
                                                            && s.licenseNumber?.toLowerCase()?.trim() == blacklistedDriverLicenseNumber?.toLowerCase()?.trim()
                                                            && s.lastName?.toLowerCase()?.trim() == blacklistedDriverLastName?.toLowerCase()?.trim());
        
        if (isDriverUsedInSubmissions != null) {
          return { 'unableToActivateBlacklistDriver': true };
        } else {
          this.policyNumberWithExistingBlacklistedDriver = '';
          return null;
        }
      }
    };
  }

  fetchDrivers() {
    this.drivers = [];
    this.driverData.fetchAllDrivers().pipe(take(1)).subscribe((data) => {
      this.drivers = data as DriverDto[];
    });
  }
}
