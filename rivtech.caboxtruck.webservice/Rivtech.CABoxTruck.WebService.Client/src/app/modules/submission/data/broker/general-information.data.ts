import { Injectable } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { BrokerInfoDto } from '../../../../shared/models/submission/brokerinfo/brokerInfoDto';

import { BrokerInfoService } from '../../services/broker-info.service';
import { catchError, distinctUntilChanged, switchMap, take, takeUntil, tap } from 'rxjs/operators';

import { ToastrService } from 'ngx-toastr';
import { SubmissionData } from '../submission.data';
import Utils from '../../../../shared/utilities/utils';
import { IAngularMyDpOptions } from 'angular-mydatepicker/lib/interfaces/my-options.interface';
import { DriverData } from '../driver/driver.data';
import { Guid } from 'guid-typescript';
import { SummaryData } from '../summary.data';
import { ManuscriptsData } from '../manuscripts/manuscripts.data';
import { ProgramStateAgencyService } from '@app/core/services/management/programstate-agency-service';
import { BaseClass } from '@app/shared/base-class';
import { ProgramStateAgentService } from '@app/core/services/management/programstate-agent-service';
import { RetailerService } from '@app/core/services/management/retailer.service';
import { RetailerAgentService } from '@app/core/services/management/retailer-agent.service';
import { TableData } from '../tables.data';
import { PagerService } from '@app/core/services/pager.service';
import { BehaviorSubject, Observable } from 'rxjs-compat';
import { RaterApiService } from '../../services/rater-api.service';
import { FormsService } from '../../services/forms.service';
import { off } from 'process';

@Injectable(
  { providedIn: 'root' }
)
export class BrokerGenInfoData extends BaseClass {
  formInfo: FormGroup;
  dropdownList: BrokerInfoDropDowns = new BrokerInfoDropDowns();
  minDate = new Date();

  effectiveOptions: IAngularMyDpOptions;
  expirationOptions: IAngularMyDpOptions;

  currEffectiveDate: any;
  currExpirationDate: any;

  efDateHasString: boolean = true;
  exDateHasString: boolean = true;

  contactsList: any = [];

  public onPolicyContactsLoaded$ = new BehaviorSubject<any>(null); 

  constructor(private formBuilder: FormBuilder,
    private brokerInfoService: BrokerInfoService,
    private toastr: ToastrService,
    private submissionData: SubmissionData,
    public driverData: DriverData,
    private summaryData: SummaryData,
    private manuscriptsData: ManuscriptsData,
    private programStateAgencyService: ProgramStateAgencyService,
    private programStateAgentService: ProgramStateAgentService,
    private retailerService: RetailerService,
    private retailerAgentService: RetailerAgentService,
    private tableData: TableData,
    private pagerService: PagerService,
    private raterApi: RaterApiService,
    private formsService: FormsService
  ) {
    super();
  }

  public initiateFormGroup(obj: BrokerInfoDto): FormGroup {
    this.formInfo = this.formBuilder.group({
      id: [this.setInitialValue(obj.id, null)],
      agencyId: [this.setInitialValue(obj.agencyId, null)], // Validators.required
      agencyName: [this.setInitialValue(obj.agencyName, null)],
      agencyPhone: [this.setInitialValue(obj.agencyPhone, null)],

      agentId: [this.setInitialValue(obj.agentId, null)], // Validators.required
      agentName: [this.setInitialValue(obj.agentName, null)],
      agentEmail: [this.setInitialValue(obj.agentEmail, null)],
      agentPhone: [this.setInitialValue(obj.agentPhone, null)],

      subAgencyId: [this.setInitialValue(obj.subAgencyId, null)],
      subAgencyName: [this.setInitialValue(obj.subAgencyName, null)],
      subAgencyPhone: [this.setInitialValue(obj.subAgencyPhone, null)],

      subAgentId: [this.setInitialValue(obj.subAgentId, null)],
      subAgentName: [this.setInitialValue(obj.subAgentName, null)],
      subAgentEmail: [this.setInitialValue(obj.subAgentEmail, null)],
      subAgentPhone: [this.setInitialValue(obj.subAgentPhone, null)],

      reasonMoveId: [this.setInitialValue(obj.reasonMoveId, null)],

      riskDetailId: [this.setInitialValue(obj.riskDetailId, null)],
      riskId: [this.setInitialValue(obj.riskId, null)],

      effectiveDate: [null, Validators.required],
      expirationDate: [null], // Validators.required

      yearsWithAgency: [this.setInitialValue(obj.yearsWithAgency, null)],

      isIncumbentAgency: [this.setInitialValue(obj.isIncumbentAgency, null)],
      isBrokeredAccount: [this.setInitialValue(obj.isBrokeredAccount, null)],
      isMidtermMove: [this.setInitialValue(obj.isMidtermMove, null)],

      assistantUnderwriterId: [this.setInitialValue(obj.assistantUnderwriterId, null)],
      submissionSpecialistEmail: [this.setInitialValue(obj.assistantUnderwriterId, null)],
      psrUserId: [this.setInitialValue(obj.psrUserId, null)],
      psrEmail: [this.setInitialValue(obj.psrUserId, null)],
      teamLeadUserId: [this.setInitialValue(obj.teamLeadUserId, null)],
      teamLeadEmail: [this.setInitialValue(obj.teamLeadUserId, null)],
      internalUWUserId: [this.setInitialValue(obj.internalUWUserId, null), [Validators.required]],
      internalUWEmail: [this.setInitialValue(obj.internalUWUserId, null)],
      productUWUserId: [this.setInitialValue(obj.productUWUserId, null)],
      productUWEmail: [this.setInitialValue(obj.productUWUserId, null)]
    }, { validators: [this.validateStartEndDate, this.validateAgencyFieldsAsRequiredToBind(), this.validateAgentFieldsAsRequiredToBind()] });

    if (obj.effectiveDate) { this.formInfo.get('effectiveDate').setValue({ isRange: false, singleDate: { jsDate: new Date(obj?.effectiveDate) } }); }
    if (obj.expirationDate) { this.formInfo.get('expirationDate').setValue({ isRange: false, singleDate: { jsDate: new Date(obj?.expirationDate) } }); }

    return this.formInfo;
  }

  validateAgencyFieldsAsRequiredToBind() {
    return (form: FormGroup) => {
      const preBindQuote = this.submissionData.riskDetail?.riskDocuments?.some(f => f.fileName === 'Pre-Bind Quote' && f.isConfidential);
      const agencyIdField = form.controls['agencyId'].value ?? '';
      if ((this.submissionData.isQuoteIssued || preBindQuote) && agencyIdField === '') {
        return { 'requiredToBindAgencyId': true };
      }

      return null;
    };
  }

  validateAgentFieldsAsRequiredToBind() {
    return (form: FormGroup) => {
      const preBindQuote = this.submissionData.riskDetail?.riskDocuments?.some(f => f.fileName === 'Pre-Bind Quote' && f.isConfidential);
      const agentIdField = form.controls['agentId'].value ?? '';
      if ((this.submissionData.isQuoteIssued || preBindQuote) && agentIdField === '') {
        return { 'requiredToBindAgentId': true };
      }

      return null;
    };
  }

  public validateStartEndDate(form: FormGroup) {
    const startDate = new Date(form.controls['effectiveDate'].value?.singleDate?.jsDate);
    const endDate = new Date(form.controls['expirationDate'].value?.singleDate?.jsDate);
    return startDate >= endDate ? { 'invalidEffectiveExpirationDate': true } : null;
  }

  public initiateFormValues(obj: BrokerInfoDto) {
    this.formInfo.patchValue(obj);
  }

  public populateBrokerFields() {
    if (this.submissionData?.riskDetail?.brokerInfo) {

      this.formInfo.reset();
      this.formInfo.patchValue(this.submissionData?.riskDetail?.brokerInfo);

      this.isBrokerAccountSwitch(this.submissionData.riskDetail.brokerInfo.isBrokeredAccount ? 'on' : 'off');
      this.IncumbentSwitchChanged(this.submissionData.riskDetail.brokerInfo.isIncumbentAgency ? '' : 'off', true);
      this.MidTermChanged(this.submissionData.riskDetail.brokerInfo.isMidtermMove, true);
    }
  }

  public initiateBrokerFields() {
    if (this.submissionData?.riskDetail?.brokerInfo) {
      this.formInfo.patchValue(this.submissionData?.riskDetail?.brokerInfo);
    }

    let riskCreatedDate = new Date(this.submissionData.currentServerDateTime);

    if (!this.submissionData.riskDetail?.brokerInfo) {
      if (this.submissionData.riskDetail?.createdDate) {
        riskCreatedDate = new Date(this.submissionData.riskDetail.createdDate);
      }

      this.formInfo.get('effectiveDate').setValue({ isRange: false, singleDate: { jsDate: riskCreatedDate } });
      this.formInfo.get('expirationDate').setValue({ isRange: false, singleDate: { jsDate: new Date(riskCreatedDate.getFullYear() + 1, riskCreatedDate.getMonth(), riskCreatedDate.getDate()) } });

      this.currEffectiveDate = this.formInfo.get('effectiveDate').value;
      this.currExpirationDate = this.formInfo.get('expirationDate').value;
    }

    if (this.submissionData.riskDetail?.brokerInfo) {
      //#region EXISTING
      const riskDetailId = this.submissionData.riskDetail?.id;
      this.formInfo.patchValue(this.submissionData.riskDetail.brokerInfo);

      this.isBrokerAccountSwitch(this.submissionData.riskDetail.brokerInfo.isBrokeredAccount ? 'on' : 'off');
      this.IncumbentSwitchChanged(this.submissionData.riskDetail.brokerInfo.isIncumbentAgency ? '' : 'off', true);
      this.MidTermChanged(this.submissionData.riskDetail.brokerInfo.isMidtermMove, true);

      const agencyId = this.submissionData.riskDetail.brokerInfo.agencyId;
      const agentId = this.submissionData.riskDetail.brokerInfo.agentId;
      const subAgencyId = this.submissionData.riskDetail.brokerInfo.subAgencyId;
      const subAgentId = this.submissionData.riskDetail.brokerInfo.subAgentId;
      const reasonMoveId = this.submissionData.riskDetail.brokerInfo.reasonMoveId;

      this.getAgency(() => {
        if (agencyId) {
          this.onChangeAgency(agencyId,
            () => {
              // populate agent
              this.fc['agentId'].setValue(agentId);
              this.onChangeAgent(agentId);
            },
            () => {
              // populate SubAgent
              this.fc['subAgencyId'].setValue(subAgencyId);
              this.onChangeRetail(subAgencyId, () => {
                this.fc['subAgentId'].setValue(subAgentId);
                this.onChangeRetailAgent(subAgentId);
              });
            }
          );
        }
      });

      this.fc['reasonMoveId'].setValue(reasonMoveId);


      if (reasonMoveId) {
        this.getReasonMove(() => {

        });
      }

      if (this.submissionData.riskDetail?.brokerInfo?.isIncumbentAgency == true) { this.IncumbentChanged(); }

      if (this.submissionData.riskDetail?.brokerInfo?.effectiveDate) {
        this.fg.get('effectiveDate').setValue({ isRange: false, singleDate: { jsDate: new Date(this.submissionData.riskDetail?.brokerInfo?.effectiveDate) } });
        this.currEffectiveDate = this.fg.get('effectiveDate').value;
      } else {
        this.fg.get('effectiveDate').setValue({ isRange: false, singleDate: { jsDate: riskCreatedDate } });
        this.currEffectiveDate = this.fg.get('effectiveDate').value;
      }

      if (this.submissionData.riskDetail?.brokerInfo?.expirationDate) {
        this.fg.get('expirationDate').setValue({ isRange: false, singleDate: { jsDate: new Date(this.submissionData.riskDetail?.brokerInfo?.expirationDate) } });
        this.currExpirationDate = this.fg.get('expirationDate').value;
      }

      //#endregion EXISTING
      setTimeout(() => {
        this.touchValidator(this.fg);
      }, 50);
    } else {
      this.getAgency();
    }
  }

  public initiatePolicyContacts(): void {
    this.brokerInfoService.getPolicyContacts().pipe(
      tap(data => {
        this.contactsList = data;
        this.onPolicyContactsLoaded$.next(data);

        if (this.submissionData.riskDetail?.brokerInfo) {
          const submissionUserId = this.submissionData.riskDetail.brokerInfo.assistantUnderwriterId ?? -1;
          const psrUserId = this.submissionData.riskDetail.brokerInfo.psrUserId ?? -1;
          const teamLeadUserId = this.submissionData.riskDetail.brokerInfo.teamLeadUserId ?? -1;
          const internalUWUserId = this.submissionData.riskDetail.brokerInfo.internalUWUserId ?? -1;
          const productUWUserId = this.submissionData.riskDetail.brokerInfo.productUWUserId ?? -1;

          this.onChangeSubmissionContact(submissionUserId);
          this.onChangePSRContact(psrUserId);
          this.onChangeTeamLeadContact(teamLeadUserId);
          this.onChangeInternalUWContact(internalUWUserId);
          this.onChangeProductUWContact(productUWUserId);
        }
      })
    ).subscribe();


  }

  public onChangeAgency(value: any, callbackAgent?: Function, callbackSubAgency?: Function) {
    const obj = this.dropdownList.agencyList.find(t => t.id == value);
    //  if (obj) {
    this.fc['agencyName'].setValue(obj?.companyName);
    this.fc['agencyPhone'].setValue(obj?.workPhone);
    // reset agent
    this.fc['agentId'].setValue(null);
    this.fc['agentName'].setValue(null);
    this.fc['agentEmail'].setValue(null);
    this.fc['agentPhone'].setValue(null);
    this.dropdownList.agentList = [];
    if (value) {
      if (value === '0: null' || value === null) { value = Guid.EMPTY; }
      this.getAgent(value, callbackAgent);
    }
    // reset subagency
    this.fc['subAgencyId'].setValue(null);
    this.fc['subAgencyName'].setValue(null);
    this.fc['subAgencyPhone'].setValue(null);
    // reset subagent
    this.fc['subAgentId'].setValue(null);
    this.fc['subAgentName'].setValue(null);
    this.fc['subAgentEmail'].setValue(null);
    this.fc['subAgentPhone'].setValue(null);

    this.dropdownList.retailerList = [];
    this.dropdownList.retailAgentList = [];
    if (this.fc['isBrokeredAccount'].value == true) {
      if (value) { this.getSubAgency(value, callbackSubAgency); }
    }
  }

  public onChangeAgent(value: any) {
    const obj = this.dropdownList.agentList.find(t => t.id == value);
    // if (obj) {
    //  this.fc['agentId'].setValue(value);
    this.fc['agentName'].setValue(obj?.Name);
    this.fc['agentEmail'].setValue(obj?.email);
    this.fc['agentPhone'].setValue(obj?.workPhone);
    // }
  }

  public onChangeRetail(value: any, callbackSubAgent?: Function) {
    const obj = this.dropdownList.retailerList.find(t => t.id == value);
    this.fc['subAgencyName'].setValue(obj?.companyName);
    this.fc['subAgencyPhone'].setValue(obj?.workPhone);

    this.fc['subAgentId'].setValue(null);
    this.fc['subAgentName'].setValue(null);
    this.fc['subAgentEmail'].setValue(null);
    this.fc['subAgentPhone'].setValue(null);


    if (value === '0: null' || value === null) { value = Guid.EMPTY; }
    try {
      this.getSubAgent(value, callbackSubAgent);
    } catch { }
  }

  public onChangeRetailAgent(value: any) {
    const obj = this.dropdownList.retailAgentList.find(t => t.id == value);
    // if (obj) {
    this.fc['subAgentId'].setValue(obj?.id === undefined ? null : obj?.id);
    this.fc['subAgentName'].setValue(obj?.Name);
    this.fc['subAgentEmail'].setValue(obj?.email);
    this.fc['subAgentPhone'].setValue(obj?.workPhone);
    // }
  }

  public retrieveDropDownValues() {
    this.getAgency();
    this.getReasonMove();
  }

  public getAgency(callback?: Function) {
    this.dropdownList.agencyList = [];
    this.programStateAgencyService.getAllProgramStateAgency().pipe(distinctUntilChanged(), takeUntil(this.stop$)).subscribe(data => {
      let activeAgencies = [];
      let allAgencies = [];
      if (data.length > 0) {
        activeAgencies = data?.filter(x => x.agency.isActive);
        allAgencies = data;
      }
      this.dropdownList.agencyList = activeAgencies.map(obj => ({
        id: obj.agency.id,
        companyName: obj.agency.entity.companyName,
        workPhone: obj.agency.entity.workPhone
      })) as [];
      this.dropdownList.agencyListAll = allAgencies.map(obj => ({
        id: obj.agency.id,
        companyName: obj.agency.entity.companyName,
        workPhone: obj.agency.entity.workPhone
      })) as [];
      Utils.unblockUI();
      if (callback) { callback(); }
    });
    // this.brokerInfoService.getAgencyList().pipe(take(1)).subscribe(data => {
    //   this.dropdownList.agencyList = data.map(obj => ({
    //     companyName: obj.entity.companyName,
    //     id: obj.id,
    //     workPhone: obj.entity.workPhone
    //   })) as [];
    //   if (callback) { callback(); }
    // }, (error) => { this.dropdownList.agencyList = []; });
  }

  public getAgencySync() {
    this.dropdownList.agencyList = [];
    const data = this.brokerInfoService.getAgencyListSyc();
    this.dropdownList.agencyList = data.map(obj => ({
      companyName: obj.entity.companyName,
      id: obj.id,
      workPhone: obj.entity.workPhone
    }));
  }

  public getAgent(id: string, callback?: Function) {
    this.dropdownList.agentList = [];
    this.programStateAgentService.getAllProgramStateAgents(id).pipe(distinctUntilChanged(), takeUntil(this.stop$)).subscribe(data => {
      if (data.length > 0) { data = data?.filter(x => x.agent.isActive); }
      this.dropdownList.agentList = data.map(obj => ({
        Name: obj.agent.entity.fullName,
        id: obj.agent.id,
        workPhone: obj.agent.entity.workPhone,
        email: obj.agent.entity.workEmailAddress
      })) as [];
      Utils.unblockUI();
      if (callback) { callback(); }
    });
    // this.brokerInfoService.getAgentListAsync(id).pipe(take(1)).subscribe(data => {
    //   this.dropdownList.agentList = data.map(obj => ({
    //     Name: obj.entity.fullName,
    //     id: obj.id,
    //     workPhone: obj.entity.workPhone,
    //     email: obj.entity.workEmailAddress
    //   })) as [];
    //   if (callback) { callback(); }
    // }, (error) => { this.dropdownList.agentList = []; });
  }

  public getSubAgency(id: string, callback?: Function) {
    this.dropdownList.retailerList = [];
    this.retailerService.getAllRetailersByAgency(id).pipe(distinctUntilChanged(), takeUntil(this.stop$)).subscribe(data => {
      if (data.length > 0) { data = data?.filter(x => x.isActive); }
      this.dropdownList.retailerList = data.map(obj => ({
        id: obj.id,
        companyName: obj.entity.companyName,
        workPhone: obj.entity.workPhone
      })) as [];
      Utils.unblockUI();
      if (callback) { callback(); }
    });
    // this.brokerInfoService.getSubAgencyList(id).pipe(take(1)).subscribe(data => {
    //   this.dropdownList.retailerList = (data).map(obj => ({
    //     companyName: obj.entity.companyName,
    //     id: obj.id,
    //     workPhone: obj.entity.workPhone,
    //   })) as [];
    //   if (callback) { callback(); }
    // }, (error) => {
    //   this.dropdownList.retailerList = [];
    // });
  }

  public getSubAgent(id: string, callback?: Function) {
    this.dropdownList.retailAgentList = [];
    this.retailerAgentService.getAllAgents(id).pipe(distinctUntilChanged(), takeUntil(this.stop$)).subscribe(data => {
      if (data.length > 0) { data = data?.filter(x => x.isActive); }
      this.dropdownList.retailAgentList = data.map(obj => ({
        Name: obj.entity.fullName,
        id: obj.id,
        workPhone: obj.entity.workPhone,
        email: obj.entity.workEmailAddress
      })) as [];
      Utils.unblockUI();
      if (callback) { callback(); }
    });
    // this.brokerInfoService.getSubAgentList(id).pipe(take(1)).subscribe(data => {
    //   this.dropdownList.retailAgentList = (data).map(obj => ({
    //     Name: obj.entity.fullName,
    //     id: obj.id,
    //     workPhone: obj.entity.workPhone,
    //     email: obj.entity.workEmailAddress
    //   })) as [];
    //   if (callback) { callback(); }
    // }, (error) => { this.dropdownList.retailAgentList = []; });
  }

  public getReasonMove(callback?: Function) {
    this.dropdownList.reasonForMove = [];
    this.brokerInfoService.getReasonMoveList().pipe(take(1)).subscribe(data => {
      this.dropdownList.reasonForMove = (data).map(obj => ({
        Name: obj.description,
        value: obj.id,
      })) as [];
      if (callback) { callback(); }
    }, (error) => { this.dropdownList.reasonForMove = []; });
  }

  public saveBrokerGenInfo(callbackError?: Function) {
    this.formInfo.markAllAsTouched();
    if (this.formInfo.invalid) {
      // i think this portion will be the one to  handle the punctuation mark logo
      // this.toastr.info('Record has invalid information. Data not saved.', 'Info!');
      this.submit(callbackError);
    } else {
      // if ((this.submissionData.risk.brokerInfo !== undefined || this.submissionData.risk.brokerInfo !== null) && this.formInfo.pristine) {
      // this.toastr.info('No changes on Broker Details.', 'Success!');
      // } else {
      this.submit(callbackError);
      // }
    }
  }


  public submit(callbackError?: Function) {
    Utils.blockUI();
    const BrokerInfo: BrokerInfoDto = new BrokerInfoDto(this.formInfo.value);
    let isEffectiveDateChange = false;

    if (this.formInfo !== undefined && this.formInfo.controls['id'].value != null && this.formInfo.controls['id'].value !== '') {
      BrokerInfo.effectiveDate = this.formInfo.get('effectiveDate').value?.singleDate?.jsDate?.toLocaleDateString();
      BrokerInfo.expirationDate = this.formInfo.get('expirationDate').value?.singleDate?.jsDate?.toLocaleDateString();

      if (!BrokerInfo.effectiveDate && this.efDateHasString) {
        this.formInfo.get('effectiveDate').setValue(this.currEffectiveDate);
        BrokerInfo.effectiveDate = this.formInfo.get('effectiveDate').value?.singleDate?.jsDate?.toLocaleDateString();
      }
      if (!BrokerInfo.expirationDate && this.exDateHasString) {
        this.formInfo.get('expirationDate').setValue(this.currExpirationDate);
        BrokerInfo.expirationDate = this.formInfo.get('expirationDate').value?.singleDate?.jsDate?.toLocaleDateString();
      }

      BrokerInfo.isIncumbentAgency = BrokerInfo.isIncumbentAgency ? true : false;
      BrokerInfo.isBrokeredAccount = BrokerInfo.isBrokeredAccount ? true : false;
      BrokerInfo.isMidtermMove = BrokerInfo.isMidtermMove ? true : false;

      isEffectiveDateChange = new Date(BrokerInfo.effectiveDate).valueOf() !== new Date(this.submissionData.riskDetail?.brokerInfo.effectiveDate).valueOf();

      this.brokerInfoService.put(BrokerInfo).pipe(take(1)).subscribe(data => {
        this.summaryData.policyNumber = data.policyNumber;
        this.submissionData.riskDetail.policyNumber = data.policyNumber;
        // this.toastr.success('Broker Details updated!', 'Success!');
        this.submissionData.riskDetail.brokerInfo = data;
        this.submissionData.riskDetail.vehicles = this.submissionData.riskDetail.vehicles.map(vehicle => {
          vehicle.effectiveDate = data.effectiveDate;
          vehicle.expirationDate = data.expirationDate;
          return vehicle;
        });
        // UPDATE driver header
        if (BrokerInfo.effectiveDate && this.submissionData.riskDetail?.driverHeader && isEffectiveDateChange) {
          const drvrHeader = this.submissionData.riskDetail?.driverHeader;
          drvrHeader.mvrHeaderDate = BrokerInfo.effectiveDate;

          if (!this.driverData.formDriverHeader) { this.driverData.initiateHeaderFormGroup(drvrHeader); }
          this.driverData.initiateHeaderValues(drvrHeader);

          this.driverData.addUpdateHeader(drvrHeader).pipe(take(1)).subscribe(drvr => {
            this.submissionData.riskDetail.driverHeader = drvr;
            this.driverData.currMVRHeaderDate = this.driverData.formDriverHeader.get('mvrHeaderDate').value;
          }, (error) => {
          });
        }
        this.updateSummaryHeader(BrokerInfo);
        this.manuscriptsData.updateDates(BrokerInfo);
        Utils.unblockUI();
      }, (error) => {
        this.populateBrokerFields();
        this.toastr.error('There was an error trying to Update a record. Please try again.');
        if (callbackError) { callbackError(); }
        Utils.unblockUI();
      });
    } else {
      BrokerInfo.riskId = this.submissionData.riskDetail?.riskId;

      BrokerInfo.effectiveDate = this.formInfo.get('effectiveDate').value?.singleDate?.jsDate?.toLocaleDateString();
      BrokerInfo.expirationDate = this.formInfo.get('expirationDate').value?.singleDate?.jsDate?.toLocaleDateString();

      if (!BrokerInfo.effectiveDate && this.efDateHasString) {
        this.formInfo.get('effectiveDate').setValue(this.currEffectiveDate);
        BrokerInfo.effectiveDate = this.formInfo.get('effectiveDate').value?.singleDate?.jsDate?.toLocaleDateString();
      }
      if (!BrokerInfo.expirationDate && this.exDateHasString) {
        this.formInfo.get('expirationDate').setValue(this.currExpirationDate);
        BrokerInfo.expirationDate = this.formInfo.get('expirationDate').value?.singleDate?.jsDate?.toLocaleDateString();
      }

      BrokerInfo.isIncumbentAgency = BrokerInfo.isIncumbentAgency ? true : false;
      BrokerInfo.isBrokeredAccount = BrokerInfo.isBrokeredAccount ? true : false;
      BrokerInfo.isMidtermMove = BrokerInfo.isMidtermMove ? true : false;

      isEffectiveDateChange = new Date(BrokerInfo?.effectiveDate).valueOf() !== new Date(this.submissionData.riskDetail?.brokerInfo?.effectiveDate).valueOf();

      this.brokerInfoService.post(BrokerInfo).pipe(take(1)).subscribe(data => {
        this.summaryData.policyNumber = data.policyNumber;
        this.submissionData.riskDetail.policyNumber = data.policyNumber;
        // this.toastr.success('Broker Details has been saved!', 'Success!');
        this.submissionData.riskDetail.brokerInfo = data;
        // UPDATE driver header
        if (BrokerInfo.effectiveDate && this.submissionData.riskDetail?.driverHeader && isEffectiveDateChange) {
          const drvrHeader = this.submissionData.riskDetail?.driverHeader;
          drvrHeader.mvrHeaderDate = BrokerInfo.effectiveDate;

          if (!this.driverData.formDriverHeader) { this.driverData.initiateHeaderFormGroup(drvrHeader); }
          this.driverData.initiateHeaderValues(drvrHeader);
          this.driverData.addUpdateHeader(drvrHeader).pipe(take(1)).subscribe(drvr => {
            this.submissionData.riskDetail.driverHeader = drvr;
          }, (error) => {
          });
        }
        this.updateSummaryHeader(BrokerInfo);
        Utils.unblockUI();
      }, (error) => {
        this.populateBrokerFields();
        this.toastr.error('There was an error on Saving a record. Please try again.');
        if (callbackError) { callbackError(); }
        Utils.unblockUI();
      });
    }
  }

  public saveBrokerGenInfo$(): Observable<any> {
    this.formInfo.markAllAsTouched();
    var hasBrokerInfo: boolean = this.formInfo !== undefined && this.formInfo.controls['id'].value != null && this.formInfo.controls['id'].value !== '';
    if (hasBrokerInfo) {
      return this.updateBrokerInfo$();
    } else {
      return this.addBrokerInfo$();
    }
  }

  private addBrokerInfo$() {
    const BrokerInfo: BrokerInfoDto = new BrokerInfoDto(this.formInfo.value);
    let isEffectiveDateChange = false;
    BrokerInfo.riskId = this.submissionData.riskDetail?.riskId;

    BrokerInfo.effectiveDate = this.formInfo.get('effectiveDate').value?.singleDate?.jsDate?.toLocaleDateString();
    BrokerInfo.expirationDate = this.formInfo.get('expirationDate').value?.singleDate?.jsDate?.toLocaleDateString();

    if (!BrokerInfo.effectiveDate && this.efDateHasString) {
      this.formInfo.get('effectiveDate').setValue(this.currEffectiveDate);
      BrokerInfo.effectiveDate = this.formInfo.get('effectiveDate').value?.singleDate?.jsDate?.toLocaleDateString();
    }
    if (!BrokerInfo.expirationDate && this.exDateHasString) {
      this.formInfo.get('expirationDate').setValue(this.currExpirationDate);
      BrokerInfo.expirationDate = this.formInfo.get('expirationDate').value?.singleDate?.jsDate?.toLocaleDateString();
    }

    BrokerInfo.isIncumbentAgency = BrokerInfo.isIncumbentAgency ? true : false;
    BrokerInfo.isBrokeredAccount = BrokerInfo.isBrokeredAccount ? true : false;
    BrokerInfo.isMidtermMove = BrokerInfo.isMidtermMove ? true : false;

    isEffectiveDateChange = new Date(BrokerInfo?.effectiveDate).valueOf() !== new Date(this.submissionData.riskDetail?.brokerInfo?.effectiveDate).valueOf();

    return this.brokerInfoService.post(BrokerInfo).pipe(
      take(1),
      tap(data => {
        const oldPolicyNumber = this.submissionData?.riskDetail?.policyNumber;
        this.summaryData.policyNumber = data.policyNumber;
        this.submissionData.riskDetail.policyNumber = data.policyNumber;
        // this.toastr.success('Broker Details has been saved!', 'Success!');
        this.submissionData.riskDetail.brokerInfo = data;
        // UPDATE driver header
        if (BrokerInfo.effectiveDate && this.submissionData.riskDetail?.driverHeader && isEffectiveDateChange) {
          const drvrHeader = this.submissionData.riskDetail?.driverHeader;
          drvrHeader.mvrHeaderDate = BrokerInfo.effectiveDate;

          if (!this.driverData.formDriverHeader) { this.driverData.initiateHeaderFormGroup(drvrHeader); }
          this.driverData.initiateHeaderValues(drvrHeader);
          this.driverData.addUpdateHeader(drvrHeader).pipe(take(1)).subscribe(drvr => {
            this.submissionData.riskDetail.driverHeader = drvr;
          }, (error) => {
          });
        }
        this.updateSummaryHeader(BrokerInfo);
        this.submissionData.updatePolicyNumber(oldPolicyNumber);
      }),
    );
  }

  private updateBrokerInfo$() {
    let isEffectiveDateChange = false;
    const BrokerInfo: BrokerInfoDto = new BrokerInfoDto(this.formInfo.value);
    BrokerInfo.effectiveDate = this.formInfo.get('effectiveDate').value?.singleDate?.jsDate?.toLocaleDateString();
    BrokerInfo.expirationDate = this.formInfo.get('expirationDate').value?.singleDate?.jsDate?.toLocaleDateString();

    if (!BrokerInfo.effectiveDate && this.efDateHasString) {
      this.formInfo.get('effectiveDate').setValue(this.currEffectiveDate);
      BrokerInfo.effectiveDate = this.formInfo.get('effectiveDate').value?.singleDate?.jsDate?.toLocaleDateString();
    }
    if (!BrokerInfo.expirationDate && this.exDateHasString) {
      this.formInfo.get('expirationDate').setValue(this.currExpirationDate);
      BrokerInfo.expirationDate = this.formInfo.get('expirationDate').value?.singleDate?.jsDate?.toLocaleDateString();
    }

    BrokerInfo.isIncumbentAgency = BrokerInfo.isIncumbentAgency ? true : false;
    BrokerInfo.isBrokeredAccount = BrokerInfo.isBrokeredAccount ? true : false;
    BrokerInfo.isMidtermMove = BrokerInfo.isMidtermMove ? true : false;

    isEffectiveDateChange = new Date(BrokerInfo.effectiveDate).valueOf() !== new Date(this.submissionData.riskDetail?.brokerInfo.effectiveDate).valueOf();

    return this.brokerInfoService.put(BrokerInfo).pipe(
      take(1),
      tap(data => {
        const oldPolicyNumber = this.submissionData?.riskDetail?.policyNumber;
        this.summaryData.policyNumber = data.policyNumber;
        this.submissionData.riskDetail.policyNumber = data.policyNumber;
        // this.toastr.success('Broker Details updated!', 'Success!');
        this.submissionData.riskDetail.brokerInfo = data;
        this.submissionData.riskDetail.vehicles = this.submissionData.riskDetail.vehicles?.map(vehicle => {
          vehicle.effectiveDate = data.effectiveDate;
          vehicle.expirationDate = data.expirationDate;
          return vehicle;
        });
        // UPDATE driver header
        if (BrokerInfo.effectiveDate && this.submissionData.riskDetail?.driverHeader && isEffectiveDateChange) {
          const drvrHeader = this.submissionData.riskDetail?.driverHeader;
          drvrHeader.mvrHeaderDate = BrokerInfo.effectiveDate;

          if (!this.driverData.formDriverHeader) { this.driverData.initiateHeaderFormGroup(drvrHeader); }
          this.driverData.initiateHeaderValues(drvrHeader);

          this.driverData.addUpdateHeader(drvrHeader).pipe(take(1)).subscribe(drvr => {
            this.submissionData.riskDetail.driverHeader = drvr;
            this.driverData.currMVRHeaderDate = this.driverData.formDriverHeader.get('mvrHeaderDate').value;
          }, (error) => {
          });
        }

        this.updateSummaryHeader(BrokerInfo);
        this.submissionData.updatePolicyNumber(oldPolicyNumber);
        this.manuscriptsData.updateDates(BrokerInfo);
      }),
      switchMap(() => this.formsService.updateFormEffectiveDate(this.submissionData.riskDetail?.id))
    );
  }

  public dateFormatOptions() {
    this.effectiveOptions = {
      dateRange: false,
      dateFormat: 'mm/dd/yyyy'
    };

    this.expirationOptions = {
      dateRange: false,
      dateFormat: 'mm/dd/yyyy'
    };
  }

  public validateExpirationDate(effectiveDate, Expirationdate) {
    if (effectiveDate > Expirationdate) {
      return 'Expiration Date cannot be earlier than Effective Date.';
    } else {
      return '';
    }
  }

  public validateEffectiveDate(effectiveDate, Expirationdate) {
    if (effectiveDate > Expirationdate) {
      return 'Effective Date cannot be later than Expiration Date.';
    } else {
      return '';
    }
  }

  get fc() { return this.fg.controls; }
  get fg() { return this.formInfo; }

  //#region Function Helpers

  public onInputChangeDate(obj, element) {
    if (element === 'effectiveDate') {
      this.efDateHasString = (obj.value.length > 0);
    }

    if (element === 'expirationDate') {
      this.exDateHasString = (obj.value.length > 0);
    }
  }

  public onChangeDate(startDate, element) {
    if (startDate?.jsDate && element === 'effectiveDate') {
      const expirationDate = {
        isRange: false,
        singleDate: {
          jsDate: new Date(startDate?.jsDate.getFullYear() + 1, startDate?.jsDate.getMonth(), startDate?.jsDate.getDate())
        }
      };

      this.fg.get('expirationDate').setValue(expirationDate);
      this.currExpirationDate = this.fg.get('expirationDate').value;
    }
    // if (!obj.jsDate) {
    //   setTimeout(() => {
    //     if (element === 'effectiveDate') {
    //       if (this.currEffectiveDate) {
    //         this.fg.get(element).setValue(this.currEffectiveDate);
    //       } else {
    //         this.fg.get(element).setValue(this.formatDateForPicker(obj.jsDate));
    //       }
    //       this.currEffectiveDate = this.fg.get(element).value;


    //       this.efDateHasString = false;
    //     }

    //     if (element === 'expirationDate') {
    //       if (this.currExpirationDate) {
    //         this.fg.get(element).setValue(this.currExpirationDate);
    //       } else {
    //         this.fg.get(element).setValue(this.formatDateForPicker(obj.jsDate));
    //       }
    //       this.currExpirationDate = this.fg.get(element).value;

    //       this.exDateHasString = false;
    //     }
    //   }, 0);
    // } else {
    //   if (element === 'effectiveDate') {
    //     this.currEffectiveDate = this.formatDateForPicker(obj.jsDate);
    //   }
    //   if (element === 'expirationDate') {
    //     this.currExpirationDate = this.formatDateForPicker(obj.jsDate);
    //   }
    // }
  }

  public isBrokerAccountSwitch(value) {
    if (value == 'on') {
      this.addValidators(['subAgencyId', 'subAgentId', 'subAgentPhone']);

      if (this.fc['agencyId'].value) {
        this.getSubAgency(this.fc['agencyId'].value);
      }
    } else if (value == 'off') {
      this.clearValidators(['subAgencyId', 'subAgentId', 'subAgentPhone']);
      this.dropdownList.retailerList = [];
      this.dropdownList.retailAgentList = [];

      this.fc['subAgencyId'].setValue(null);
      this.fc['subAgencyName'].setValue(null);
      this.fc['subAgencyPhone'].setValue(null);
      this.fc['subAgentId'].setValue(null);
      this.fc['subAgentName'].setValue(null);
      this.fc['subAgentEmail'].setValue(null);
      this.fc['subAgentPhone'].setValue(null);
    }
  }

  public IncumbentSwitchChanged(value, isPageLoad: boolean = false) {
    if (!isPageLoad) {
      this.fc['yearsWithAgency'].setValue(null);
    }

    if (value) {
      this.clearValidators(['yearsWithAgency']);
    } else {
      this.addValidators(['yearsWithAgency']);
    }


  }

  public MidTermChanged(value, isPageLoad: boolean = false) {
    if (!isPageLoad) {
      this.fc['reasonMoveId'].setValue(null);
    }

    if (value) {
      this.addValidators(['reasonMoveId']);
    } else {
      this.clearValidators(['reasonMoveId']);
    }
  }

  public IncumbentChanged() {
    if (this.fc.isIncumbentAgency.value) {
      if (this.fc.yearsWithAgency.value) {
        this.fc.yearsWithAgency.clearValidators();
        this.fc.yearsWithAgency.updateValueAndValidity();

      } else {
        this.addValidators(['yearsWithAgency']);
        this.fc['yearsWithAgency'].setValue(null);
      }
    } else {
      this.addValidators(['yearsWithAgency']);
      this.fc['yearsWithAgency'].setValue(null);
    }
  }

  public clearValidators(controlName: string[]) {
    controlName.forEach(eachControl => {
      this.fc[eachControl].clearValidators();
      this.fc[eachControl].updateValueAndValidity();
    });
  }

  public addValidators(controlName: string[]) {
    // controlName.forEach(eachControl => {
    //   this.fc[eachControl].setValidators([Validators.required]);
    //   this.fc[eachControl].markAsUntouched();
    //   this.fc[eachControl].markAsPristine();
    //   this.fc[eachControl].updateValueAndValidity();
    // });
  }

  public touchValidator(formGroup: FormGroup): void {
    Object.keys(formGroup.controls).forEach((key) => {
      formGroup.get(key).markAsDirty();
      formGroup.get(key).markAsTouched();
    });
  }

  public addEmailValidators(controlName: string[]) {
    controlName.forEach(eachControl => {
      this.fc[eachControl].setValidators([Validators.required, Validators.email]);
      this.fc[eachControl].updateValueAndValidity();
    });
  }

  public formatDateForPicker(dateValue?: any) {
    const dateSet = dateValue ? new Date(dateValue) : new Date();
    return { isRange: false, singleDate: { jsDate: dateSet } };
  }

  public setInitialValue(obj: any, val: any) {
    return (obj == null || obj.length == 0) ? val : obj;
  }
  //#endregion Function helper

  private updateSummaryHeader(brokerInfo: BrokerInfoDto) {
    this.summaryData.effectiveDate = `${brokerInfo?.effectiveDate}`;
    this.summaryData.agency = brokerInfo.agencyName;
    this.summaryData.producer = brokerInfo.agentName;
  }

  public onChangeSubmissionContact(value: any) {
    const obj = this.contactsList.find(t => t.id == value && t.position == "Assistant Underwriter");
    const id = (obj?.id == undefined) ? null : obj?.id;

    if (this.submissionData.riskDetail.brokerInfo) {
      this.submissionData.riskDetail.brokerInfo.assistantUnderwriterId = id;
    }
    this.fc['submissionSpecialistEmail'].setValue(obj?.email);
  }

  public onChangePSRContact(value: any) {
    const obj = this.contactsList.find(t => t.id == value && t.position == "PSR");
    const id = (obj?.id == undefined) ? null : obj?.id;

    if (this.submissionData.riskDetail.brokerInfo) {
      this.submissionData.riskDetail.brokerInfo.psrUserId = id;
    }
    this.fc['psrEmail'].setValue(obj?.email);
  }

  public onChangeTeamLeadContact(value: any) {
    const obj = this.contactsList.find(t => t.id == value && t.position == "Team Leader");
    const id = (obj?.id == undefined) ? null : obj?.id;

    if (this.submissionData.riskDetail.brokerInfo) {
      this.submissionData.riskDetail.brokerInfo.teamLeadUserId = id;
    }
    this.fc['teamLeadEmail'].setValue(obj?.email);
  }

  public onChangeInternalUWContact(value: any) {
    const obj = this.contactsList.find(t => t.id == value && t.position == "Internal Underwriter");
    const id = (obj?.id == undefined) ? null : obj?.id;

    if (this.submissionData.riskDetail.brokerInfo) {
      this.submissionData.riskDetail.brokerInfo.internalUWUserId = id;
    }
    this.fc['internalUWEmail'].setValue(obj?.email);
  }

  public onChangeProductUWContact(value: any) {
    const obj = this.contactsList.find(t => t.id == value && t.position == "Product Underwriter");
    const id = (obj?.id == undefined) ? null : obj?.id;

    if (this.submissionData.riskDetail.brokerInfo) {
      this.submissionData.riskDetail.brokerInfo.productUWUserId = id;
    }
    this.fc['productUWEmail'].setValue(obj?.email);
  }
}

export class BrokerInfoDropDowns {
  agencyList: any[];
  agencyListAll: any[];
  agentList: any[];
  retailerList: any[];
  retailAgentList: any[];
  reasonForMove: any[];
}
