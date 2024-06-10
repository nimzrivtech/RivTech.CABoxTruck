import { Injectable } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { BrokerInfoDto } from '../../../../shared/models/submission/brokerinfo/brokerInfoDto';

import { BrokerInfoService } from '../../services/broker-info.service';
import { catchError, distinctUntilChanged, take, takeUntil, tap } from 'rxjs/operators';

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
import { PolicyContactsService } from '../../services/policy-contacts.service';
import { SelectItem } from '@app/shared/models/dynamic/select-item';
import { RiskPolicyContactDTO } from '@app/shared/models/submission/RiskPolicyContactDto';
import { Observable, throwError } from 'rxjs';

@Injectable(
  { providedIn: 'root' }
)
export class PolicyContactsData extends BaseClass {
  formInfo: FormGroup;

  pager: any = {};
  currentPage: number = 0;
  pagedItems: any[];
  loading: boolean;

  policyContacts: any = [];

  entityTypesList: any[] = [];
  policyContactTypesDropdown: any[] = [];
  tempPolicyContactTypes: any[] = [];
  disablePolicyContactTypesDropdown: boolean = false;
  isEdit: boolean = false;

  constructor(private formBuilder: FormBuilder,
    private toastr: ToastrService,
    private submissionData: SubmissionData,
    private policyContactsService: PolicyContactsService,
    private tableData: TableData,
    private pagerService: PagerService
  ) {
    super();
   }

  initiateFormGroup(): FormGroup {
    this.entityTypesList = [];
    this.loadPolicyContactTypesDropdown().subscribe(); // fetch policy contact types

    this.formInfo = this.formBuilder.group({
      id: [null],
      typeId: [null, Validators.required],
      firstName: [null, Validators.required],
      lastName: [null, Validators.required],
      email: [null, [Validators.required, Validators.pattern(
        '^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\\.[a-zA-Z]{2,5}$'
      )]],
      phone: [null, [Validators.required, Validators.minLength(10)]],
      address: [null, Validators.required],
      city: [null, Validators.required],
      state: [null, Validators.required],
      zipCode: [null, Validators.required],
    });

    return this.formInfo;
  }

  setInitialValue(obj: any, val: any) {
    return (obj == null || obj.length == 0) ? val : obj;
  }

  initiateFormValues(obj: BrokerInfoDto) {
    this.formInfo.patchValue(obj);
  }

  savePolicyContacts(callbackError?: Function) {
    this.formInfo.markAllAsTouched();
    if (this.formInfo.invalid) {
      this.save(callbackError);
    } else {
      this.save(callbackError);
    }
  }

  save(callbackError?: Function) {
    Utils.blockUI();
    const policyContactModel: RiskPolicyContactDTO = new RiskPolicyContactDTO(this.formInfo.value);
    policyContactModel.typeId = parseInt(this.formInfo.value.typeId);
    if (this.formInfo !== undefined && this.formInfo.controls['id'].value != null && this.formInfo.controls['id'].value !== '') {
      this.policyContactsService.put(policyContactModel).pipe(take(1)).subscribe(data => {
        this.getPolicyContacts();
        Utils.unblockUI();
      }, (error) => {
        this.toastr.error('There was an error trying to Update a record. Please try again.');
        if (callbackError) { callbackError(); }
        Utils.unblockUI();
      });
    } else {
      policyContactModel.riskDetailId = this.submissionData.riskDetail?.id;
      this.policyContactsService.post(policyContactModel).pipe(take(1)).subscribe(data => {
        this.getPolicyContacts(true);
        Utils.unblockUI();
      }, (error) => {
        this.toastr.error('There was an error on Saving a record. Please try again.');
        if (callbackError) { callbackError(); }
        Utils.unblockUI();
      });
    }
  }

  get fc() { return this.fg.controls; }
  get fg() { return this.formInfo; }

  getPolicyContacts(isNew: boolean = false) {
    this.tempPolicyContactTypes = [];
    let tempPolicyContactTypesDropdown = [];
    const riskDetailId = this.submissionData.riskDetail?.id;
    this.policyContactsService.getAll(riskDetailId).subscribe(data => {
      this.policyContacts = data;
      if (this.policyContacts.length > 0) {
        this.policyContacts.forEach(ai => {
          ai.type = this.getEntityTypeName(ai.typeId, isNew);
        });

        if (this.policyContacts?.length < 5) {
          this.disablePolicyContactTypesDropdown = false;
          this.entityTypesList.forEach((entityType) => {
            const currentEntity = this.tempPolicyContactTypes.find(et => et?.id === entityType?.id);
            if (currentEntity === undefined) {
              tempPolicyContactTypesDropdown.push(entityType);
            }
          });
          this.policyContactTypesDropdown = tempPolicyContactTypesDropdown;
        } else {
          this.disablePolicyContactTypesDropdown = true;
        }
      } else {
        this.policyContactTypesDropdown = this.entityTypesList;
      }
      this.setPage(1, this.policyContacts.sort((a, b) => a?.type < b?.type ? -1 : 1));
      this.submissionData.riskDetail.riskPolicyContacts = this.policyContacts;
    });
  }

  getEntityTypeName(id: number, isNew: boolean = false) {
    const entityType = this.entityTypesList.find(et => et?.id == id); // strict (===) comparison results to undefined
    // if (isNew) {
    //   this.policyContactTypesDropdown = this.policyContactTypesDropdown.filter(et => et?.id != id);
    // } else {
    //   if (this.policyContactTypesDropdown?.length < 1) {
    //     this.policyContactTypesDropdown.push(entityType);
    //   } else {
    //     this.policyContactTypesDropdown = this.entityTypesList.filter(et => et?.id != id);
    //   }
    //   this.policyContactTypesDropdown.sort((a, b) => a?.id > b?.id ? -1 : 1);
    // }
    this.tempPolicyContactTypes.push(entityType); // hold all type with record

    return entityType?.description ?? '';
  }

  sortPolicyContacts(targetElement) {
    const sortData = this.tableData.sortPagedData(targetElement, this.policyContacts, this.currentPage);
    this.policyContacts = sortData.sortedData;
    this.pagedItems = sortData.pagedItems;
  }

  setPage(page: number, data: any[], isNew: boolean = false) {
    this.pager = {};
    this.pagedItems = [];
    if (page < 1) {
      return;
    }

    this.pager = this.pagerService.getPager(data.length, page);

    if (isNew) {
      this.currentPage = this.pager.currentPage = this.pager.totalPages;
      this.pager.endIndex = this.pager.totalItems;
      this.pager.startIndex = ((this.pager.currentPage * this.pager.pageSize) - this.pager.pageSize);
    } else {
      while (this.pager.totalItems <= ((this.pager.currentPage * this.pager.pageSize) - this.pager.pageSize)) {
        this.pager.currentPage--;
        this.pager.startIndex = ((this.pager.currentPage * this.pager.pageSize) - this.pager.pageSize);
      }
      this.currentPage = page;
    }

    this.pagedItems = data.slice(this.pager.startIndex, this.pager.endIndex + 1);
  }

  loadPolicyContactTypesDropdown(): Observable<any> {
    return this.policyContactsService.getPolicyContactTypes().pipe(
        tap((data) => {
            this.entityTypesList = data;
            this.policyContactTypesDropdown = data;
        }),
        catchError((error) => {
            console.error(error);
            return throwError(error);
        })
    );
  }
}

export class BrokerInfoDropDowns {
  agencyList: any[];
  agentList: any[];
  retailerList: any[];
  retailAgentList: any[];
  reasonForMove: any[];
}
