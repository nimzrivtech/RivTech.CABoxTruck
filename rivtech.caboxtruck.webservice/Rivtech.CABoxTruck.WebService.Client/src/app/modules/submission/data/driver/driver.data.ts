import { Injectable } from '@angular/core';
import { AbstractControl, FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { SelectItem } from '../../../../../app/shared/models/dynamic/select-item';
import { DriverDto, DriverHeaderDto } from '../../../../shared/models/submission/Driver/DriverDto';

import { DriverService } from '../../services/driver.service';
import { finalize, map, take, takeUntil, tap } from 'rxjs/operators';

import { DatePipe, formatDate } from '@angular/common';
import { Observable } from 'rxjs';
import { ApplyFilter } from '../../../../shared/constants/vehicle-driver.options.constants';
import { DriverIncidentDto } from '../../../../shared/models/submission/Driver/DriverIncidentDto';
import { IAngularMyDpOptions } from 'angular-mydatepicker/lib/interfaces/my-options.interface';
import { ApplicantNameAddressService } from '../../services/applicant-name-address.service';
import { AddressDTO } from '../../../../../app/shared/models/submission/addressDto';
import { SubmissionData } from '../submission.data';
import { PagerService } from '../../../../core/services/pager.service';
import { DriverFilterPipe } from '../.././../../shared/components/customPipes/driver-filter.pipe';
import { TableData } from '../tables.data';
import { PolicyHistoryDTO } from '@app/shared/models/policy/PolicyHistoryDto';
import { PolicyHistoryData } from '@app/modules/policy/data/policy-history.data';
import { BlacklistedDriverService } from '@app/modules/dashboard/services/blacklisted-driver.service';
import Utils from '@app/shared/utilities/utils';
import * as moment from 'moment';

@Injectable(
  { providedIn: 'root'}
)
export class DriverData {
 
  formDriverInfo: FormGroup;
  formDriverHeader: FormGroup;
  formDriverIncident: FormGroup;

  driverDropdownsList: DriverInformationDropDowns = new DriverInformationDropDowns();
  driverInfoList: Array<DriverDto> = new Array<DriverDto>();
  driverIncidentList: Array<DriverIncidentDto> = new Array<DriverIncidentDto>();
  driverIncidentList_old: Array<DriverIncidentDto> = new Array<DriverIncidentDto>();

  currMVRHeaderDate: any;

  maxDate: any;
  minDate: any;

  datepipe: DatePipe;

  dobOptions: IAngularMyDpOptions;
  mvrOptions: IAngularMyDpOptions;
  hireOptions: IAngularMyDpOptions;

  incidentOptions: IAngularMyDpOptions;
  convictionOptions: IAngularMyDpOptions;
  mvrHeaderOptions: IAngularMyDpOptions;

  pager: any = {};
  sortOptions: any = {};
  loading: boolean;
  currentPage: number = 1;
  currentTotalItems: number = 1;
  pagedItems: DriverDto[];
  sortField: string;
  order: string;

  showRowsNumber: number = 10;

  defaultPolicyState: string;
  policyHistories: any[];
  blacklistedDrivers: any[];

  constructor(private formBuilder: FormBuilder,
    private driverService: DriverService,
    private datePipe: DatePipe,
    private applicantNameAddressService: ApplicantNameAddressService,
    private submissionData: SubmissionData,
    private pagerService: PagerService,
    private tableData: TableData,
    private policyHistoryData: PolicyHistoryData,
    private blacklistedDriverService: BlacklistedDriverService
  ) {
    this.datepipe = datePipe;
  }

  initiateFormGroup(obj: DriverDto): FormGroup {
    this.formDriverInfo = this.formBuilder.group({
      id: [this.setInitialValue(obj.id, null)],
      birthDate: [null, Validators.required],

      age: [this.setInitialValue(obj.age, ''), this.driverMinAge(15)],
      stateId: [this.setInitialValue(obj.stateId, '')],
      state: [this.setInitialValue(obj.state, ''), Validators.required],
      outStateDriver: [this.setInitialValue(obj.outStateDriver, false)],
      yrsDrivingExp: [this.setInitialValue(obj.yrsDrivingExp, ''), this.driverMaxYrsExp(63)], // , Validators.required

      yrsCommDrivingExp: [this.setInitialValue(obj.yrsCommDrivingExp, ''), [Validators.required, this.driverMaxYrsExp(63), this.driverYrsMin(1)]],

      include: [this.setInitialValue(obj.include, '')],
      lastName: [this.setInitialValue(obj.lastName, ''), Validators.required],
      firstName: [this.setInitialValue(obj.firstName, ''), Validators.required],
      middleName: [this.setInitialValue(obj.firstName, '')],
      isMvr: [this.setInitialValue(obj.outStateDriver, true)],
      isOutOfState: [this.setInitialValue(obj.outStateDriver, false)],
      mvrDate: [null, Validators.required],
      hireDate: [null], // Validators.required

      licenseNumber: [this.setInitialValue(obj.licenseNumber, ''), Validators.required],

      isExcludeKO: [this.setInitialValue(obj.isExcludeKO, false)],
      koReason: [this.setInitialValue(obj.koReason, '')],

      yearPoint: [this.setInitialValue(obj.yearPoint, '')],
      ageFactor: [this.setInitialValue(obj.ageFactor, '')],
      totalFactor: [this.setInitialValue(obj.totalFactor, '')],

      riskDetailId: [this.setInitialValue(obj.riskDetailId, null)],
      entityId: [this.setInitialValue(obj.entityId, null)],

      createdBy: [this.setInitialValue(obj.createdBy, null)],
      createdDate: [this.setInitialValue(obj.createdDate, null)],

      isActive: [this.setInitialValue(obj.isActive, null)],
      isValidated: [this.setInitialValue(obj.isValidated, null)],

      deletedDate: [this.setInitialValue(obj.deletedDate, null)],
      savedDrivers: [[]]
    }, {
      validators: [this.validateDuplicateDriverId(), this.validateBlacklistedDriverName()]
    });

    if (obj.birthDate) { this.formDriverInfo.get('birthDate').setValue({ isRange: false, singleDate: { jsDate: new Date(obj?.birthDate) } }); }
    if (obj.hireDate) { this.formDriverInfo.get('hireDate').setValue({ isRange: false, singleDate: { jsDate: new Date(obj?.hireDate) } }); }
    if (obj.mvrDate) { this.formDriverInfo.get('mvrDate').setValue({ isRange: false, singleDate: { jsDate: new Date(obj?.mvrDate) } }); }

    this.formDriverInfo.patchValue({ savedDrivers: this.submissionData?.riskDetail?.drivers });
    return this.formDriverInfo;
  }

  initiateHeaderFormGroup(obj?: DriverHeaderDto): FormGroup {
    this.formDriverHeader = this.formBuilder.group({
      id: [this.setInitialValue(obj.id, null)],
      driverRatingTab: [this.setInitialValue(obj.driverRatingTab, true)],
      accidentInfo: [this.setInitialValue(obj.accidentInfo, true)],
      mvrHeaderDate: [null],
      // applyFilter: [new Array<SelectItem>(), Validators.required],
      applyFilter: [new Array<SelectItem>()],
      accDriverFactor: [this.setInitialValue(obj.accDriverFactor, null)],
      driverVehicleRatio: [this.setInitialValue(obj.driverVehicleRatio, null)],
      filter: [''],
    }, {
    });

    return this.formDriverHeader;
  }

  initiateFormIncident(obj: DriverIncidentDto): FormGroup {
    this.formDriverIncident = this.formBuilder.group({
      id: [this.setInitialValue(obj.id, null)],
      incidentType: [this.setInitialValue(obj.incidentType, ''), Validators.required],
      incidentDate: [this.setInitialValue(obj.incidentDate, ''), Validators.required],
      convictionDate: [this.setInitialValue(obj.convictionDate, ''), Validators.required],
    }, {
      validators: this.validateIncidentDate
    });

    return this.formDriverIncident;
  }

  initiateFormValues(driver: DriverDto, isView: boolean = false) {
    this.formDriverInfo.patchValue(driver);

    if (driver.birthDate) { this.formDriverInfo.get('birthDate').patchValue({ isRange: false, singleDate: { jsDate: new Date(driver?.birthDate) } }); }
    if (driver.hireDate) { this.formDriverInfo.get('hireDate').patchValue({ isRange: false, singleDate: { jsDate: new Date(driver?.hireDate) } }); }
    if (driver.mvrDate) { this.formDriverInfo.get('mvrDate').patchValue({ isRange: false, singleDate: { jsDate: new Date(driver?.mvrDate) } }); }

    this.formDriverInfo.patchValue({ savedDrivers: this.submissionData?.riskDetail?.drivers });
    // this.formDriverInfo.patchValue({ savedDrivers: this.driverInfoList });

    if (isView) {
      this.formDriverInfo.get('state').disable();
      this.formDriverInfo.get('outStateDriver').disable();
      this.formDriverInfo.get('yrsDrivingExp').disable();
      this.formDriverInfo.get('yrsCommDrivingExp').disable();
      this.formDriverInfo.get('include').disable();
      this.formDriverInfo.get('lastName').disable();
      this.formDriverInfo.get('firstName').disable();
      this.formDriverInfo.get('middleName').disable();
      this.formDriverInfo.get('isOutOfState').disable();
      this.formDriverInfo.get('isExcludeKO').disable();
      this.formDriverInfo.get('koReason').disable();
      this.formDriverInfo.get('yearPoint').disable();
      this.formDriverInfo.get('ageFactor').disable();
      this.formDriverInfo.get('totalFactor').disable();
    } else {
      this.formDriverInfo.get('outStateDriver').enable();
      this.formDriverInfo.get('yrsDrivingExp').enable();
      this.formDriverInfo.get('yrsCommDrivingExp').enable();
      this.formDriverInfo.get('include').enable();
      this.formDriverInfo.get('lastName').enable();
      this.formDriverInfo.get('firstName').enable();
      this.formDriverInfo.get('middleName').enable();
      this.formDriverInfo.get('isOutOfState').enable();
      this.formDriverInfo.get('isExcludeKO').enable();
      this.formDriverInfo.get('koReason').enable();
      this.formDriverInfo.get('yearPoint').enable();
      this.formDriverInfo.get('ageFactor').enable();
      this.formDriverInfo.get('totalFactor').enable();
    }
  }

  getAllBlacklisterDriver(callBackFn?: Function) {
    return this.blacklistedDriverService.searchDriver('', '').pipe(finalize( () => Utils.unblockUI()), take(1)).subscribe((result) => {
      this.blacklistedDrivers = result;
      console.log(result);
      if (callBackFn) { callBackFn(); }
    });
  }

  populateDriverHeaderField() {
    if (this.submissionData?.riskDetail?.driverHeader) {
      const driverHeader: DriverHeaderDto = this.submissionData.riskDetail.driverHeader;

      this.formDriverHeader.patchValue({
        id: driverHeader?.id,
        riskDetailId: driverHeader?.riskDetailId,
        driverRatingTab: driverHeader?.driverRatingTab,
        accidentInfo: driverHeader?.accidentInfo,
        mvrHeaderDate: driverHeader?.mvrHeaderDate,
        accDriverFactor: driverHeader?.accDriverFactor,
        driverVehicleRatio: driverHeader?.driverVehicleRatio,
        applyFilter: [new Array<SelectItem>()],
      });

      if (driverHeader?.mvrHeaderDate) {
        this.formDriverHeader.get('mvrHeaderDate').setValue({ isRange: false, singleDate: { jsDate: new Date(driverHeader?.mvrHeaderDate) } });
      }

      if (driverHeader) {
        let items: SelectItem[] = new Array<SelectItem>();
        items = driverHeader.applyFilter?.split(',').filter(x => x !== '').map(x => {
          return ApplyFilter.filter(y => y.value === Number(x))[0];
        });

        items = Array.from(new Set(items)); // filter unique
        this.formDriverHeader.patchValue({ applyFilter: items });
      }
    }
  }

  initiateHeaderValues(value: { [key: string]: any; }) {
    Object.keys(value).forEach(key => {
      if (this.formDriverHeader.get(key) && key === 'mvrHeaderDate' && value?.mvrHeaderDate) {
        this.formDriverHeader.get('mvrHeaderDate').setValue({ isRange: false, singleDate: { jsDate: new Date(value[key]) } });
        return;
      }

      if (this.formDriverHeader.get(key) && key === 'applyFilter') {
        let items: SelectItem[] = new Array<SelectItem>();
        items = value.applyFilter?.split(',').filter(x => x !== '').map(x => {
          return ApplyFilter.filter(y => y.value === Number(x))[0];
        });

        items = Array.from(new Set(items)); // filter unique
        this.formDriverHeader.get(key).setValue(items);
        return;
      }

      if (this.formDriverHeader.get(key)) { this.formDriverHeader.get(key).setValue(value[key]); }
    });
  }

  setPage(page: number, hasListUpdated: boolean = false, itemCount: number = 0) {
    let totalPages: number = 0;

    if (this.driverInfoList.length > 0 && page === 0) {
      page = 1;
    }

    if (page < 1) {
      return;
    }

    let drivers = Object.assign([], this.driverInfoList);

    if (this.formDriverHeader.controls.filter?.value) {
      drivers = new DriverFilterPipe().transform(this.driverInfoList, this.formDriverHeader.controls.filter?.value);
    }

    totalPages = Math.ceil(drivers.length / this.showRowsNumber);

    this.currentPage = (page > totalPages) ? totalPages : 
                       (hasListUpdated) ? totalPages : page;
    this.pager = this.pagerService.getPagerMod(drivers.length, this.currentPage, this.showRowsNumber);

    if (hasListUpdated) {
      this.pager.currentPage = this.pager.totalPages;
      this.pager.endIndex = this.pager.totalItems;
      this.pager.startIndex = ((this.pager.currentPage * this.pager.pageSize) - this.pager.pageSize);

      this.pagedItems = drivers.slice(this.pager.startIndex, this.pager.endIndex + 1);
    } else {
      while (this.pager.totalItems <= ((this.pager.currentPage * this.pager.pageSize) - this.pager.pageSize)) {
        this.pager.currentPage--;
        this.pager.startIndex = ((this.pager.currentPage * this.pager.pageSize) - this.pager.pageSize);
      }

      this.pagedItems = drivers.slice(this.pager.startIndex, this.pager.endIndex + 1);
    }

    this.currentTotalItems = this.pager.totalItems;
  }

  changeShowRowsNumber(page: number, value: number, hasAdded: boolean = false) {
    if (this.showRowsNumber === value) { return; }

    this.showRowsNumber = value;
    let drivers = Object.assign([], this.driverInfoList);

    if (this.formDriverHeader.controls.filter?.value) {
      drivers = new DriverFilterPipe().transform(this.driverInfoList, this.formDriverHeader.controls.filter?.value);
    }

    this.currentPage = (hasAdded) ? Math.ceil(drivers.length / this.showRowsNumber) : page;

    this.pager = this.pagerService.getPagerMod(drivers.length, this.currentPage, value);
    this.pagedItems = drivers.slice(this.pager.startIndex, this.pager.endIndex + 1);
    this.currentTotalItems = this.pager.totalItems;
  }

  retrieveDropDownValues() {

    const filteredState: Array<SelectItem> = new Array<SelectItem>();
    try {
      this.driverService.getStates().pipe(take(1)).subscribe(data => {
        this.driverDropdownsList.stateList = data.map(s => ({
          value: s.stateCode?.toString(),
          label: s.fullStateName?.toString()?.replace('[E]', '')
        }) as SelectItem) as SelectItem[];
      });

    } catch {
      this.driverDropdownsList.stateList = filteredState;
    }

    this.driverDropdownsList.applyFilter = ApplyFilter;

    this.driverDropdownsList.incidentTypeList = [
      { value: '1', label: 'Majors' }, //
      { value: '2', label: 'DUI/DWI/Drug' }, //
      { value: '3', label: 'Minors (No Speed)' },
      { value: '4', label: 'At-Fault' }, //
      { value: '5', label: 'Major License Issues (Foreign, Unverified)' },  //
      { value: '6', label: 'Not At-Fault' },
      { value: '7', label: 'Equipment' },
      { value: '8', label: 'Speed' },  //
      { value: '9', label: 'Other' },
    ];
  }

  dateFormatOptions() {
    const currentServerDateTime = this.submissionData.currentServerDateTime ?? new Date(formatDate(new Date(), 'MM/dd/yyyy', 'en', 'EST'));
    this.dobOptions = {
      dateRange: false,
      dateFormat: 'mm/dd/yyyy',
      disableUntil: {
        year: 1799,
        month: 12,
        day: 31,
      },
      disableSince: {
        year: currentServerDateTime.getFullYear(),
        month: currentServerDateTime.getMonth() + 1,
        day: currentServerDateTime.getDate() + 1,
      },
    };

    this.incidentOptions = this.convictionOptions = this.hireOptions = this.mvrHeaderOptions = this.mvrOptions = {
      dateRange: false,
      dateFormat: 'mm/dd/yyyy'
    };
  }

  getPolicyHistories() {
    this.policyHistories = [];
    this.policyHistoryData.getEndorsementHistory().subscribe((data: PolicyHistoryDTO[]) => {
      this.policyHistories = data;
    });
  }

  s37KoRuleImplem(driver: DriverDto) {
    const brokerEffectiveDate = this.submissionData?.riskDetail?.brokerInfo?.effectiveDate;
    const majorIncidents = ["1", "2", "5"];
    const minorIncidents = ["3", "4", "6", "7", "8", "9"];
    const movingViolations = ['1', '2', '4', '5', '8'];
    const end = new Date(brokerEffectiveDate);
    end.setHours(0, 0, 0, 0);

    const prev3years = new Date(brokerEffectiveDate);
    prev3years.setMonth(prev3years.getMonth() - 36);
    prev3years.setHours(0, 0, 0, 0);

    const prev5years = new Date(brokerEffectiveDate);
    prev5years.setMonth(prev5years.getMonth() - 60);
    prev5years.setHours(0, 0, 0, 0);

    const driverAge = Number(driver.age);

    const minorIncidentCount = driver.driverIncidents?.filter(({incidentDate, incidentType}) => {
      return new Date(incidentDate) >= prev3years && new Date(incidentType) <= end && minorIncidents.includes(incidentType);
    })?.length ?? 0;

    const majorIncidentCount = driver.driverIncidents?.filter(({incidentDate, incidentType}) => {
      return new Date(incidentDate) >= prev5years && new Date(incidentType) <= end && majorIncidents.includes(incidentType);
    })?.length ?? 0;

    const hasMv = (driver.driverIncidents?.filter((item: DriverIncidentDto) => {
      return (movingViolations.find(x => x === item.incidentType));
    })?.length ?? 0) > 0;

    if ((driverAge == 24 || driverAge == 25)){
      if(minorIncidentCount >= 3)
        return { id: "KOUnder25", hasMv: hasMv, message: 'Driver has 3+ minor incidents in previous 3 years. Default Options not included.' };
      if(minorIncidentCount >= 1)
        return { id: "KOUnder25", hasMv: hasMv, message: 'Driver does not meet acceptable Age/Incident parameters. Default Options not included.' };
    }
    if(driverAge <= 22)
      return { id: "KOUnder25", hasMv: hasMv, message: 'Applicant must be 23 years old or higher. Default Options not included.' };
    if (minorIncidentCount >= 3)
      return { id: "KOUnder25", hasMv: hasMv, message: 'Driver has 3+ minor incidents in previous 3 years. Default Options not included.' };
    if (majorIncidentCount >= 1)
      return { id: "KOUnder25", hasMv: hasMv, message: 'Driver has 1+ Major Incidents in the previous 5 years. Default Options not included.' };
    if (driver.yrsCommDrivingExp < 2)
      return { id: "KOUnder25", hasMv: hasMv, message: 'Driver has less than 2 years commercial driving experience. Default Options not included.' };
    if (driverAge > 70)
      return { id: "", hasMv: hasMv, message: 'Driver age >70. Default Options not included unless medical documentation received.' };
    
    return null;
  }
  optAgeValidation(driver: DriverDto) {
    const brokerEffectiveDate = this.submissionData?.riskDetail?.brokerInfo?.effectiveDate;
    const isNewKoRuleImplem = this.submissionData.isNewFeatureApplicableByQuoteDate('S37KORulesUpdate');
    const movingViolations = ['1', '2', '4', '5', '8'];
    const hasMv = (driver.driverIncidents?.filter((item: DriverIncidentDto) => {
      return (movingViolations.find(x => x === item.incidentType));
    })?.length ?? 0) > 0;

    if(isNewKoRuleImplem)
      return this.s37KoRuleImplem(driver);

    if (Number(driver.age) <= 22) {
      return { id: "KOUnder25", hasMv: hasMv, message: 'Applicant must be 23 years old or higher. <br/>Default Options not included.' };
    }

    if ((Number(driver.age) == 23 || Number(driver.age) == 24) && driver.isMvr == false) {
      return { id: "KOUnder25", hasMv: hasMv, message: 'Applicant below 25 years old should have cleared MVR. <br/>Default Options not included.' };
    }

    if (Number(driver.age) >= 25) {
      if (driver.driverIncidents) {
        const end = new Date(brokerEffectiveDate);
        const start = new Date(brokerEffectiveDate);

        end.setHours(0, 0, 0, 0);

        start.setMonth(start.getMonth() - 36);
        start.setHours(0, 0, 0, 0);

        // '1','Majors' },
        // '2','DUI/DWI/Drug' },
        // '4','At-Fault' },
        // '5','Major License Issues (Foreign, Unverified)' },
        // '8','Speed' },

        const arrIncident = driver.driverIncidents.filter((item: DriverIncidentDto) => {
          return (new Date(item.incidentDate) <= end && new Date(item.incidentDate) >= start && ['1', '2', '4', '5', '8'].find(x => x === item.incidentType));
        });

        if (arrIncident.length > 1) {
          return { id: "KOMovingViolations", hasMv: true, message: 'Applicant have acquired more than one incident within 36 months. <br/>Default Options not included.' };
        }
      }
    }

    return null;
  }

  validateMVRdateEffective(mvrdate, effectivedate) {
    const dateDiff = this.computeDateDiff(mvrdate, effectivedate);
    if (dateDiff >= 60 && mvrdate) {
      return `MVR Date: ${new Date(mvrdate).toLocaleDateString()} is 60 days older than policy Effective Date : ${new Date(effectivedate).toLocaleDateString()}`;
    }
    return '';
  }

  renderDriverList() {
    const riskDetailId = this.submissionData.riskDetail?.id;

    if (riskDetailId) {
      return this.driverService.getRiskDrivers(riskDetailId)
        .pipe(
          take(1),
          map((data) => {
            this.driverInfoList = data as DriverDto[];
            this.driverInfoList.forEach((v: DriverDto) => v.isMainRow = true);
            this.submissionData.riskDetail.drivers = this.driverInfoList;
          })
        );
    } else {
      throw new Error(`The parameter 'id' must be defined.`);
    }
  }

  sortDriver() {
    this.driverInfoList.sort(this.sortByLabel);
  }

  reinstate(id: string, arg1: boolean): Observable<any> {
    return this.driverService.reinstate(id).pipe(
      tap(() => {
        this.submissionData.riskDetail.drivers = this.submissionData.riskDetail?.drivers.filter(t => t.id !== id && t.deletedDate == null);
      })
    );
  }

  sortByLabel(a: DriverDto, b: DriverDto) {
    return a.licenseNumber < b.licenseNumber ? -1 : a.licenseNumber > b.licenseNumber ? 1 : 0;
  }

  public getDefaultPolicyState(riskDetailId): Promise<unknown> {
    // tslint:disable-next-line: no-shadowed-variable
    const promise = new Promise((resolve, reject) => {
      try {
        this.applicantNameAddressService.getNameAndAddress(riskDetailId).pipe(take(1)).subscribe(data => {
          const address = data.addresses.filter(t => (t.address as AddressDTO).isMainGarage == true);
          const state = (address[0].address as AddressDTO).state;
          this.defaultPolicyState = state;
          resolve(state);
        }, (error) => {
          reject('');
        });
      } catch {
        reject('');
      }
    });
    return promise;
  }


  public updateOption(obj: DriverDto) {
    const objDriverLst: DriverDto[] = [];
    objDriverLst.push(obj);
    const objDriver = objDriverLst.map(o => ({ riskDetailId: o.riskDetailId, id: o.id, options: o.options }));
    return this.driverService.PutListOptions(objDriver);
  }

  public update(obj: DriverDto): Observable<DriverDto> {
    return this.driverService.put(obj);
  }

  public updateEndorsement(obj: DriverDto): Observable<DriverDto> {
    return this.driverService.put(obj, true);
  }

  public insert(obj: DriverDto): Observable<DriverDto> {
    return this.driverService.post(obj);
  }

  insertDrivers(drivers: DriverDto[]): Observable<DriverDto[]> {
    return this.driverService.postDrivers(drivers);
  }


  public delete(id: string): Observable<DriverDto> {
    return this.driverService.delete(id);
  }

  public deleteAll(): Observable<DriverDto> {
    const riskDetailId = this.submissionData.riskDetail?.id;
    return this.driverService.deleteAll(riskDetailId);
  }

  public deleteUnboundDrivers() {
    const risk = this.submissionData?.riskDetail;
    const bindOption = Number(risk.binding?.bindOptionId) ?? 0;
    const drivers = risk.drivers.filter(driver => !driver.options?.split(',').includes(`${bindOption}`));
    if (drivers?.length < 1) { return; }
    drivers.forEach((driver) => {
      this.delete(driver.id).pipe(take(1)).subscribe(data => {
        this.driverInfoList = risk.drivers.filter(t => t.id !== driver.id);
        this.submissionData.riskDetail.drivers = this.driverInfoList;
      }, (error) => {
        console.error('There was an error trying to remove a record. Please try again.');
      });
    });
  }

  public addUpdateHeader(obj: DriverHeaderDto): Observable<DriverHeaderDto> {
    obj.mvrHeaderDate = this.formDriverHeader.get('mvrHeaderDate').value?.singleDate?.jsDate?.toLocaleDateString();
    if (Array.isArray(obj.applyFilter)) { obj.applyFilter = this.formDriverHeader.get('applyFilter').value.map(x => x.value).join(); }

    obj.accidentInfo = obj.accidentInfo ? true : false;
    obj.driverRatingTab = obj.driverRatingTab ? true : false;

    if (obj.applyFilter?.length === 0) { obj.applyFilter = null; }

    if (!obj.riskDetailId) { obj.riskDetailId = this.submissionData.riskDetail?.id; }
    return this.driverService.addUpdateHeader(obj);
  }

  enableKORule() {
    const driver: DriverDto = new DriverDto(this.formDriverInfo.value);
    driver.driverIncidents = this.driverIncidentList;
    driver.birthDate = this.formDriverInfo.get('birthDate').value?.singleDate?.jsDate?.toLocaleDateString();
    driver.mvrDate = this.formDriverInfo.get('mvrDate').value?.singleDate?.jsDate?.toLocaleDateString();
    driver.hireDate = this.formDriverInfo.get('hireDate').value?.singleDate?.jsDate?.toLocaleDateString();

    let validateAgeOptions = this.optAgeValidation(driver);

    const age = driver.age;
    if (age == null || age == undefined || age === '') {
      validateAgeOptions = null;
    }

    return (validateAgeOptions != null);
  }

  public triggerHeaderUpdate(drvrHeaderUpdate: DriverHeaderDto): Promise<DriverHeaderDto> {
    // tslint:disable-next-line: no-shadowed-variable
    const promise = new Promise((resolve, reject) => {
      try {
        this.addUpdateHeader(drvrHeaderUpdate).pipe(take(1)).subscribe(data => {
          resolve(data);
        }, (error) => {
          if (this.submissionData.riskDetail.driverHeader) {
            reject(this.submissionData.riskDetail.driverHeader);
          }
        });
      } catch {
        reject(this.submissionData.riskDetail.driverHeader);
      }
    });
    return promise;
  }

  public saveDriverheader(callBackFn?: Function) {
    const fgHeader = this.formDriverHeader;
    const drvrHeader: DriverHeaderDto = new DriverHeaderDto(fgHeader.value);

    this.triggerHeaderUpdate(drvrHeader).then((driverHeader) => {
      this.submissionData.riskDetail.driverHeader = driverHeader;
    }).catch((obj) => {
      this.initiateHeaderValues(this.submissionData.riskDetail?.driverHeader);
    }).finally(() => {
      if (callBackFn) { callBackFn(); }
    });
  }

  uploadExcelFile(inputData?: any): Observable<any> {
    return this.driverService.uploadExcelFile(inputData);
  }

  downloadTemplate() {
    return this.driverService.downloadTemplate();
  }

  //#region Function Helpers
  public setInitialValue(obj: any, val: any) {
    return (obj == null || obj.length == 0) ? val : obj;
  }

  public formatDateForPicker(dateValue?: any) {
    const dateSet = dateValue ? new Date(dateValue) : new Date();
    return { isRange: false, singleDate: { jsDate: dateSet } };
  }


  public calculateAge(dateOfBirth: any, policyEffectiveDate: any) {
    const dateEntry = new Date(dateOfBirth);
    const policyDate = policyEffectiveDate!='Invalid Date' ? new Date(policyEffectiveDate) : new Date();
    const yearDiff = moment(policyDate.toDateString()).diff(moment(dateEntry.toDateString()), 'years');
   
    return dateOfBirth ? Math.abs(yearDiff) : '';
  }

  computeDateDiff(date1, date2) {
    const dt1 = new Date(date1);
    const dt2 = new Date(date2);
    return Math.floor((Date.UTC(dt2.getFullYear(), dt2.getMonth(), dt2.getDate()) - Date.UTC(dt1.getFullYear(), dt1.getMonth(), dt1.getDate())) / (1000 * 60 * 60 * 24));
  }

  public validateIncidentDate(form: FormGroup) {
    const value = form.get('incidentType').value;
    const startDate = new Date(form.controls['incidentDate'].value?.singleDate?.jsDate);
    const endDate = new Date(form.controls['convictionDate'].value?.singleDate?.jsDate);

    // '4' == 'At-Fault'
    // '6' == 'Not at Fault'
    return (value != '4' && value != '6') && startDate > endDate ? { 'invalidIncidentDate': true } : null;
  }

  validateDuplicateDriverName() {
    return (form: FormGroup) => {
      const savedDrivers = form?.controls['savedDrivers'].value as Array<DriverDto>;
      const firstName: string = form.controls['firstName'].value ?? '';
      const lastName: string = form.controls['lastName'].value ?? '';
      const licenseNumber: string = form.controls['licenseNumber'].value ?? '';

      if (savedDrivers) {
        // const drvr = savedDrivers?.find(s => s.firstName?.toLowerCase()?.trim() === firstName?.toLowerCase()?.trim() && s.lastName?.toLowerCase()?.trim() === lastName?.toLowerCase()?.trim());
        savedDrivers.sort(this.sortByCreatedDate);
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

  public sortByCreatedDate(a: DriverDto, b: DriverDto) {
    return a.createdDate < b.createdDate ? -1 : a.createdDate > b.createdDate ? 1 : 0;
  }

  validateDuplicateDriverId() {
    return (form: FormGroup) => {
      const savedDrivers = form?.controls['savedDrivers'].value as Array<DriverDto>;
      const licenseNumber: string = form.controls['licenseNumber'].value ?? '';
      const state: string = form.controls['state'].value ?? '';
      if (savedDrivers) {
        const drvr = savedDrivers?.find(s => s.licenseNumber?.toLowerCase()?.trim() === licenseNumber?.toLowerCase()?.trim() && s.state === state);

        if (drvr && drvr?.id != form.controls?.id?.value && (licenseNumber.length > 0)) {
          return { 'invalidDuplicateDriverId': true };
        } else {
          return null;
        }
      }
    };
  }

  validateBlacklistedDriverName() {
    return (form: FormGroup) => {
      const lastName: string = form.controls['lastName'].value ?? '';
      const licenseNumber: string = form.controls['licenseNumber'].value ?? '';

      if (lastName !== '' && licenseNumber !== '') {
        if (this.blacklistedDrivers.some(x => x.lastName.toLowerCase() === lastName.toLowerCase() && x.driverLicenseNumber === licenseNumber && x.isActive === true)) {
          form.get('licenseNumber').setErrors({'invalidBlacklistedDriver': true});
          form.get('lastName').setErrors({'invalidBlacklistedDriver': true});
          return;
        } else {
          form.get('licenseNumber').setErrors(null);
          form.get('lastName').setErrors(null);
          return null;
        }

        // this.blacklistedDriverService.validateDriver(lastName, licenseNumber).pipe(take(1)).subscribe((result) => {
        //   if (result) {
        //     form.get('licenseNumber').setErrors({'invalidBlacklistedDriver': true});
        //     form.get('lastName').setErrors({'invalidBlacklistedDriver': true});
        //     return;
        //   } else {
        //     form.get('licenseNumber').setErrors(null);
        //     form.get('lastName').setErrors(null);
        //     return null;
        //   }
        // });
      }
    };
  }


  public driverMinAge(age: number) {
    return function (control: AbstractControl) {
      const val = control.value;
      if (((val ?? '').toString().length > 0) && val <= age) {
        return { driverMinAge: true };
      }
      return null;
    };
  }

  public driverMaxYrsExp(num: number) {
    return function (control: AbstractControl) {
      const val = control.value;
      if (((val ?? '').toString().length > 0) && val > num) {
        return { driverMaxYrsExp: true };
      }
      return null;
    };
  }

  public driverYrsMin(num: number) {
    return function (control: AbstractControl) {
      const val = control.value;
      if (((val ?? '').toString().length > 0) && val < num) {
        return { driverYrsMin: true };
      }
      return null;
    };
  }

  validatedriverMaxYrsExp() {
    return (form: FormGroup) => {
      const val: string = form.controls['yrsCommDrivingExp'].value ?? '';

      if (((val ?? '').toString().length > 0) && Number(val) > 63) {
        return { 'validatedriverMaxYrsExp': true };
      }
      return null;
    };
  }

  public IncidentTypeConviction(value): boolean {
    // '4' == 'At-Fault'
    // '6' == 'Not at Fault'
    return (value == '4' || value == '6');
  }

  public getDriverState(driver: DriverDto) {
    return this.driverDropdownsList.stateList.find(f => f.value.toLowerCase() === driver.state?.toLowerCase())?.value ?? this.defaultPolicyState;
  }

  public getStatesFiltered(isNew: boolean, isOutState: boolean): any {
    // if (isNew && isOutState) {
    if (isOutState) {
      return this.driverDropdownsList?.stateList.filter(f => f.value != this.defaultPolicyState);
    } else {
      return this.driverDropdownsList?.stateList;
    }
  }

  get getOptionQuote(): string {
    const opt = this.submissionData?.riskDetail?.riskCoverages?.map(x => x.optionNumber)?.toString();
    return !((opt ?? '').length) ? '1' : opt;
  }

  sort(targetElement) {
    let drivers = Object.assign([], this.driverInfoList);
    let sortData = this.tableData.sortPagedData(targetElement, drivers, this.currentPage, this.showRowsNumber);

    this.sortOptions = sortData.sortOptions;
    this.sortField = sortData.sortOptions.property;
    this.order = sortData.sortOptions.order;
    this.pagedItems = sortData.pagedItems;
    this.driverInfoList = sortData.sortedData;
  }

  sortWithProperty(setOrder = null) {
    let drivers = Object.assign([], this.driverInfoList);
    let fromDriver = 0;

    if (setOrder === "loadDriverHeader") {
      fromDriver = 1;
      this.sortField = 'createdDate';
      this.order = 'asc';
    }
   
    let sortData = this.tableData.sortPagedDataWithProperty(this.sortField, this.order, drivers, this.currentPage, this.showRowsNumber,fromDriver);

    this.sortOptions = sortData.sortOptions;
    this.sortField = sortData.sortOptions.property;
    this.order = sortData.sortOptions.order;
    this.pagedItems = sortData.pagedItems;
    this.driverInfoList = sortData.sortedData;
  }

  setDefaultSort(){
    this.sortField = 'createdDate';
    this.order = 'asc';

    this.sortWithProperty();
  }

  loadPreviousDriver(driver: DriverDto): Observable<any> {
    return this.driverService.getPreviousDrivers(driver).pipe(
      tap((data: DriverDto[]) => {
        driver.previousDrivers = data;
      })
    );
  }

  fetchAllDrivers() {
    return this.driverService.getAllDriverData().pipe(
      tap((data: DriverDto[]) => {
        return data;
      })
    );
  }

  fetchDriversByLastNameAndLicenseNumber(lastName: string, licenseNumber: string) {
    return this.driverService.getDriversByLastNameAndLicenseNumber(lastName, licenseNumber).pipe(
      tap((data: DriverDto[]) => {
        return data;
      })
    );
  }

  //#endregion Function helpers

  public updateDriverExclusionEndorsementForm$(riskDetailId: string) : Observable<any> {
    return this.driverService.updateDriverExclusionEndorsementForm$(riskDetailId);
  }

  loadDriverHeader$() : Observable<any> {
    let riskDetailId = this.submissionData.riskDetail.id;
    this.sortWithProperty("loadDriverHeader",);
    return this.driverService.getDriverHeader$(riskDetailId).pipe(
      tap((data) => {
        this.submissionData.riskDetail.driverHeader = data;
        console.log(this.submissionData.riskDetail.driverHeader);
      })
    );
  }

  get reportHeaders() {
    return {
      firstName: 'First Name',
      middleName: 'Middle Name',
      lastName: 'Last Name',
      licenseNumber: 'DL Number',
      birthDate: 'DOB',
      yrsCommDrivingExp: 'Years Commercial Driving Exp'
    };
  }

  get reportPolicyHeaders() {
    return {
      firstName: 'First Name',
      middleName: 'Middle Name',
      lastName: 'Last Name',
      licenseNumber: 'DL Number',
      birthDate: 'DOB',
      yrsCommDrivingExp: 'Years Commercial Driving Exp',
      totalFactor: 'Total Factor',
      effectiveDate :'Effective Date',
      expirationDate :'Expiration Date'
    };
  }
 
}

//test merge

export class DriverInformationDropDowns {
  stateList: SelectItem[];
  applyFilter: SelectItem[];
  incidentTypeList: SelectItem[];
}
