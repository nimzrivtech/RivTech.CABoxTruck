import { Component, OnInit } from '@angular/core';
import { Subject } from 'rxjs';
import { BsModalRef, BsModalService , PageChangedEvent} from 'ngx-bootstrap';
import { ApplicantLabelsConstants } from '../../../../../../../../shared/constants/applicant.labels.constants';
import { SaveRiskAdditionalInterestDTO } from '../../../../../../../../shared/models/submission/RiskAdditionalInterestDto';
import { ApplicantAdditionalInterestService } from '../../../../../../../../modules/submission/services/applicant-additional-interest.service';
import { ApplicantData } from '../../../../../../../../modules/submission/data/applicant/applicant.data';
import { AdditionalInterestFormComponent } from './additional-interest-form.component';
import { SubmissionData } from '../../../../../../data/submission.data';
import NotifUtils from '../../../../../../../../shared/utilities/notif-utils';
import { TableConstants } from '../../../../../../../../shared/constants/table.constants';
import { ToastrService } from 'ngx-toastr';
import Utils from '@app/shared/utilities/utils';

@Component({
  selector: 'app-additional-interest-section',
  templateUrl: './additional-interest-section.component.html',
  styleUrls: ['./additional-interest-section.component.scss']
})
export class AdditionalInterestSectionComponent implements OnInit {
  public showRowsNumber: number = 10;
  riskDetailId: string = '';
  LabelMessage = ApplicantLabelsConstants;
  modalRef: BsModalRef | null;
  dtOptions: DataTables.Settings = {};
  dtTrigger: Subject<any> = new Subject();
  hideMe: boolean = false;
  TableConstants = TableConstants;
  itemPerPage: number = 10;
  isNumRowsChanged : boolean = false;
  rowsAfterChange : number;
  localEvent: PageChangedEvent = { itemsPerPage:10, page:1};

  constructor(private modalService: BsModalService,
    public additionalInterestService: ApplicantAdditionalInterestService,
    public submissionData: SubmissionData,
    public applicantData: ApplicantData,
    private toastr: ToastrService,
  ) { }

  ngOnInit() {
    this.riskDetailId = this.submissionData.riskDetail?.id;
    this.applicantData.order ='ASC';
    this.applicantData.sortField='AddProcessDate';
    this.dtOptions = {
      pagingType: 'full_numbers',
      responsive: true,
      processing: true,
      destroy: true
    };
    if (this.riskDetailId != null) {
      this.applicantData.pagedItems = [];
      setTimeout(() => {
        this.applicantData.getAdditionalInterests();
        this.setBlanketCoverage('isWaiverOfSubrogation', this.blanketCoverage?.isWaiverOfSubrogation, true);
        this.setBlanketCoverage('isPrimaryNonContributory', this.blanketCoverage?.isPrimaryNonContributory, true);
        this.disableBlanketCoverage();
      }, 1000);
    }
  }

  get form() { return this.applicantData.applicantForms.additionalInterestForm; }
  get interests() { return this.applicantData.additionalInterests; }
  set interests(newValue: any) { this.applicantData.additionalInterests = newValue; }

  get blanketCoverageForm() {
    return this.applicantData.applicantForms.blanketCoverage;
  }

  get blanketCoverage() {
    return this.submissionData.riskDetail?.blanketCoverage;
  }

  editAdditionalInterest(interest: SaveRiskAdditionalInterestDTO) {
    this.form.patchValue(interest);
    this.formatDate(true);
    this.setCurrentNames(interest.id);
    this.EditDialog();
  }

  addAdditionalInterest() {
    const interest = new SaveRiskAdditionalInterestDTO();
    interest.isWaiverOfSubrogation = this.blanketCoverage?.isWaiverOfSubrogation ?? false;
    interest.isPrimaryNonContributory = this.blanketCoverage?.isPrimaryNonContributory ?? false;
    interest.isAutoLiability = interest.isGeneralLiability = false; // Test fix to weird Test bug, may not be needed.
    this.form.patchValue(interest);
    this.formatDate();
    this.setCurrentNames();
    this.addDialog();
  }

  setBlanketCoverage(fieldName: string, isChecked: boolean, isInitial?: boolean) {
    isChecked ? this.form.get(fieldName)?.disable() : this.form.get(fieldName)?.enable();
    if (!isInitial) {
      this.applicantData.saveBlanketCoverage();
      if (isChecked && this.interests.length === 0) {
        this.toastr.warning('You have selected a blanket coverage but no Additional Interests exist');
      }
    }
  }

  deleteAdditionalInterest(param: any) {
    NotifUtils.showConfirmMessage(`Are you sure you want to delete '${param?.entityType}'?`, () => {
      Utils.blockUI();
      this.additionalInterestService.delete(param.id).subscribe(() => {
        this.applicantData.additionalInterests = this.applicantData.additionalInterests.filter(ai => ai.id !== param.id);
        this.applicantData.getAdditionalInterests();
        this.applicantData.refreshQuoteOptionsAndSummaryHeader();
        Utils.unblockUI();
      });
    });
  }

  interestChanged(interest: any, value: boolean, field: string) {
    interest[field] = value;
    this.applicantData.saveAdditionalInterest(interest);
  }

  private addDialog() {
    this.form.markAsUntouched();
    this.form.markAsPristine();
    this.modalRef = this.modalService.show(AdditionalInterestFormComponent, {
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
    this.modalRef = this.modalService.show(AdditionalInterestFormComponent, {
      initialState: {
        isEdit: true,
        modalTitle: 'Edit Additional Interest',
        modalButton: 'Save',
      },
      backdrop: true,
      ignoreBackdropClick: true,
    });
  }

  private formatDate(isEdit?: boolean) {
    const effectiveDate = this.form.get('effectiveDate');
    const expirationDate = this.form.get('expirationDate');
    const policyEffectiveDate = this.submissionData.riskDetail?.brokerInfo?.effectiveDate ?? new Date(this.submissionData.currentServerDateTime);
    if (isEdit) {
      effectiveDate.setValue(this.formatDateForPicker(effectiveDate.value));
      expirationDate.setValue(this.formatDateForPicker(expirationDate.value));
    } else {
      effectiveDate.setValue(this.formatDateForPicker(policyEffectiveDate));
      expirationDate.setValue(this.formatDateForPicker(this.afterYearDate(policyEffectiveDate)));
    }
  }

  private formatDateForPicker(dateValue?: any) {
    const dateSet = dateValue ? new Date(dateValue) : new Date(this.submissionData.currentServerDateTime);
    return { isRange: false, singleDate: { jsDate: dateSet } };
  }

  private afterYearDate(dateValue?: any) {
    const afterYearDate = dateValue ? new Date(dateValue) : new Date(this.submissionData.currentServerDateTime);
    afterYearDate.setFullYear(afterYearDate.getFullYear() + 1);
    return afterYearDate;
  }

  private setCurrentNames(editingId?: string) {
    const savedInterests = this.applicantData.additionalInterests.filter(n => n.id !== editingId)
    .map(i => ({additionalInterestTypeId: i.additionalInterestTypeId, name: i.name, address: i.address, city: i.city}));
    this.form.get('currentInterests')?.setValue(savedInterests);
  }

  public ToggleHiding(): void {
    this.hideMe = !this.hideMe;
  }

  disableBlanketCoverage() {
    if (this.submissionData?.isIssued) {
      this.blanketCoverageForm.get('isAdditionalInsured')?.disable();
      this.blanketCoverageForm.get('isWaiverOfSubrogation')?.disable();
      this.blanketCoverageForm.get('isPrimaryNonContributory')?.disable();
    }
  }

  changeShowRowsNumber(value): void{
    this.showRowsNumber = value;
    this.applicantData.itemPerPage = value;
    this.localEvent.itemsPerPage = value;
    this.localEvent.page =this.localEvent.page>1 ? 1: this.localEvent.page;
    this.isNumRowsChanged = true;
    this.aiPageChanged(this.localEvent);
  }

  aiPageChanged(event: PageChangedEvent): void {
    this.localEvent = event;
    event.itemsPerPage = this.isNumRowsChanged? this.showRowsNumber  : event.itemsPerPage;
    this.applicantData.getAiList(this.applicantData.getAIListPayload(event.page, event.itemsPerPage, false)) ;
  }

  aiList(currentPage:number,itemPerPage:number)
  {
   this.applicantData.getAiList(this.applicantData.getAIListPayload(currentPage, itemPerPage, false)) 
  }

  ngOnDestroy() {
    this.applicantData.currentPage = 1;
    this.applicantData.itemPerPage = 10;
  }
}
