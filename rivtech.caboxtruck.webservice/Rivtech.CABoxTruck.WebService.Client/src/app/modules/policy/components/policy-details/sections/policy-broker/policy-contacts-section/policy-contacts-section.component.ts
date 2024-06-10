import { Component, OnInit } from '@angular/core';
import { Subject } from 'rxjs';
import { BsModalRef, BsModalService } from 'ngx-bootstrap';
import { ToastrService } from 'ngx-toastr';
import { TableConstants } from '@app/shared/constants/table.constants';
import { SubmissionData } from '@app/modules/submission/data/submission.data';
import NotifUtils from '@app/shared/utilities/notif-utils';
import { RiskPolicyContactDTO } from '@app/shared/models/submission/RiskPolicyContactDto';
import { PolicyContactsData } from '@app/modules/submission/data/broker/policy-contacts.data';
import { PolicyContactFormComponent } from './policy-contacts-form.component';
import { PolicyContactsService } from '@app/modules/submission/services/policy-contacts.service';

@Component({
  selector: 'app-policy-contacts-section',
  templateUrl: './policy-contacts-section.component.html',
  styleUrls: ['./policy-contacts-section.component.scss']
})
export class PolicyContactSectionComponent implements OnInit {

  riskDetailId: string = '';
  modalRef: BsModalRef | null;
  dtOptions: DataTables.Settings = {};
  dtTrigger: Subject<any> = new Subject();
  hideMe: boolean = false;
  TableConstants = TableConstants;
  currentServerDateTime: Date;

  constructor(private modalService: BsModalService,
    public submissionData: SubmissionData,
    public policyContactsData: PolicyContactsData,
    private policyContactService: PolicyContactsService,
    private toastr: ToastrService,
  ) { }

  ngOnInit() {
    this.riskDetailId = this.submissionData.riskDetail?.id;
    this.policyContactsData.initiateFormGroup();
    this.dtOptions = {
      pagingType: 'full_numbers',
      responsive: true,
      processing: true,
      destroy: true
    };
    if (this.riskDetailId != null) {
      setTimeout(() => {
        this.policyContactsData.pagedItems = [];
        this.policyContactsData.getPolicyContacts(true);
        console.log(this.policyContactsData.pagedItems);
      }, 1000);
    }
  }

  get form() { return this.policyContactsData.formInfo; }
  get contacts() { return this.policyContactsData.policyContacts; }

  editPolicyContact(policyContact: RiskPolicyContactDTO) {
    this.policyContactsData.formInfo.patchValue(policyContact);
    this.policyContactsData.isEdit = true;
    this.EditDialog();
  }

  addPolicyContact() {
    const policyContact = new RiskPolicyContactDTO();
    this.policyContactsData.formInfo.reset();
    this.policyContactsData.formInfo.patchValue(policyContact);
    this.policyContactsData.isEdit = false;
    this.addDialog();
  }

  deletePolicyContact(param: any) {
    NotifUtils.showConfirmMessage(`Are you sure you want to delete '${param?.type}'?`, () => {
      this.policyContactService.delete(param.id).subscribe(() => {
        this.policyContactsData.policyContacts = this.policyContactsData.policyContacts.filter(ai => ai.id !== param.id);
        setTimeout(() => {
          this.policyContactsData.getPolicyContacts();
        }, 1000);
      });
    });
  }

  private addDialog() {
    this.form.markAsUntouched();
    this.form.markAsPristine();
    this.modalRef = this.modalService.show(PolicyContactFormComponent, {
      initialState: {
        isEdit: false,
      },
      backdrop: true,
      ignoreBackdropClick: true,
    });
  }

  private EditDialog() {
    this.form.markAsUntouched();
    this.form.markAsPristine();
    this.modalRef = this.modalService.show(PolicyContactFormComponent, {
      initialState: {
        isEdit: true,
        modalTitle: 'Edit Policy Contact',
        modalButton: 'Save',
      },
      backdrop: true,
      ignoreBackdropClick: true,
    });
  }

  public ToggleHiding(): void {
    this.hideMe = !this.hideMe;
  }
}
