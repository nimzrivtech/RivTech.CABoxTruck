import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { BsModalRef } from 'ngx-bootstrap';
import { Subject } from 'rxjs';
import { LvBusinessType } from '../../../../../../../../../shared/constants/business-details.options.constants';
import { ApplicantData } from '../../../../../../../../../modules/submission/data/applicant/applicant.data';
import { SubmissionData } from '../../../../../../../../../modules/submission/data/submission.data';
import Utils from '../../../../../../../../../shared/utilities/utils';
import { SaveEntitySubsidiaryDTO } from '../../../../../../../../../shared/models/submission/entitySubsidiaryDto';
import { take } from 'rxjs/operators';
import { ToastrService } from 'ngx-toastr';
import { BusinessDetailsDTO } from '../../../../../../../../../shared/models/submission/applicant/businessDetailsDto';
import { ApplicantBusinessDetailsService } from '../../../../../../../../../modules/submission/services/applicant-business-details.service';
import { BusinessDetailsData } from '../../../../../../../../../modules/submission/data/applicant/business-details.data';

@Component({
  selector: 'app-company-dialog',
  templateUrl: './company-dialog.component.html',
  styleUrls: ['./company-dialog.component.scss']
})
export class CompanyDialogComponent implements OnInit {
  modalTitle: string = 'Add Previous Company';
  modalButton: string = 'Add Company';
  editItem: any;
  isEdit: boolean = false;
  isDisable: boolean = false;

  companyForm: FormGroup;

  public onClose: Subject<boolean>;
  constructor(
    public applicantData: ApplicantData,
    public modalRef: BsModalRef,
    private fb: FormBuilder,
    public submissionData: SubmissionData,
    private applicantBusinessDetailsService: ApplicantBusinessDetailsService,
    private toastr: ToastrService,
    private businessDetailsData: BusinessDetailsData
  ) {
   }

  ngOnInit() {
    this.companyForm = this.fb.group({
      name: [null, Validators.required],
      nameYearOperations: [null, [Validators.required, Validators.min(1)]],
    });
    this.onClose = new Subject();

    if (this.isEdit) {
      this.companyForm.controls['name'].patchValue(this.editItem.name);
      this.companyForm.controls['nameYearOperations'].patchValue(this.editItem.nameYearOperations);
    }
  }

  get f() { return this.companyForm.controls; }

  saveCompany() {
    const data = new SaveEntitySubsidiaryDTO({
      riskDetailId: this.submissionData.riskDetail.id,
      name: this.companyForm.value.name,
      nameYearOperations: this.companyForm.value.nameYearOperations ? parseInt(this.companyForm.value.nameYearOperations) : null,
    });

    if (this.isEdit) {
      data.id = this.editItem.id;
    }

    Utils.blockUI();
    this.applicantBusinessDetailsService.saveApplicantCompany(data).pipe(take(1)).subscribe(data => {
      Utils.unblockUI();
      if (this.isEdit) {
        console.log('Company Edited!');
        this.editItem.name = data.name;
        this.editItem.nameYearOperations = data.nameYearOperations;
        return;
      }

      console.log('Company Added!');
      this.applicantData.companyList.push(data);
      if (this.submissionData.riskDetail.businessDetails == null) {
        this.submissionData.riskDetail.businessDetails = new BusinessDetailsDTO;
      }
      this.submissionData.riskDetail.businessDetails.companies = this.applicantData.companyList;
      this.businessDetailsData.setPreviousCompanyPage(1, this.applicantData.companyList, true);
    }, (error) => {
      Utils.unblockUI();
      this.toastr.error('There was an error on Saving. Please try again.');
    });
    this.modalRef.hide();
  }
}
