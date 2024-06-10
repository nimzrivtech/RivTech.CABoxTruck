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
import { BusinessDetailsData } from '@app/modules/submission/data/applicant/business-details.data';

@Component({
  selector: 'app-subsidiary-dialog',
  templateUrl: './subsidiary-dialog.component.html',
  styleUrls: ['./subsidiary-dialog.component.scss']
})
export class SubsidiaryDialogComponent implements OnInit {
  modalTitle: string = 'Add Subsidiary / Affiliate';
  modalButton: string = 'Add Subsidiary';
  editItem: any;
  isEdit: boolean = false;
  isDisable: boolean = false;

  subsidiaryForm: FormGroup;
  businessTypeList = LvBusinessType;

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
    this.subsidiaryForm = this.fb.group({
      name: [null, Validators.required],
      typeOfBusinessId: [null, Validators.required],
      numVechSizeComp: [null, [Validators.required, Validators.min(1)]],
      relationship: [null, Validators.required],
    });
    this.onClose = new Subject();

    if (this.isEdit) {
      this.subsidiaryForm.controls['name'].patchValue(this.editItem.name);
      this.subsidiaryForm.controls['typeOfBusinessId'].patchValue(this.editItem.typeOfBusinessId);
      this.subsidiaryForm.controls['numVechSizeComp'].patchValue(this.editItem.numVechSizeComp);
      this.subsidiaryForm.controls['relationship'].patchValue(this.editItem.relationship);
    }
  }

  get f() { return this.subsidiaryForm.controls; }

  saveSubsidiary() {
    const data = new SaveEntitySubsidiaryDTO({
      riskDetailId: this.submissionData.riskDetail.id,
      name: this.subsidiaryForm.value.name,
      typeOfBusinessId: this.subsidiaryForm.value.typeOfBusinessId ? parseInt(this.subsidiaryForm.value.typeOfBusinessId) : null,
      numVechSizeComp: this.subsidiaryForm.value.numVechSizeComp ? parseInt(this.subsidiaryForm.value.numVechSizeComp) : null,
      relationship: this.subsidiaryForm.value.relationship,
    });

    if (this.isEdit) {
      data.id = this.editItem.id;
    }

    Utils.blockUI();
    this.applicantBusinessDetailsService.saveApplicantSubsidiary(data).pipe(take(1)).subscribe(data => {
      Utils.unblockUI();
      if (this.isEdit) {
        console.log('Subsidiaries Edited!');
        this.editItem.name = data.name;
        this.editItem.typeOfBusinessId = data.typeOfBusinessId;
        this.editItem.typeOfBusiness = LvBusinessType.find(x => Number(x.value) === data.typeOfBusinessId).label;
        this.editItem.numVechSizeComp = data.numVechSizeComp;
        this.editItem.relationship = data.relationship;
        return;
      }

      console.log('Subsidiaries Added!');
      this.applicantData.subsidiaryList.push(data);
      if (this.submissionData.riskDetail.businessDetails == null) {
        this.submissionData.riskDetail.businessDetails = new BusinessDetailsDTO;
      }
      this.submissionData.riskDetail.businessDetails.subsidiaries = this.applicantData.subsidiaryList;
      this.setLabel();
      this.businessDetailsData.setSubsidiaryPage(1, this.applicantData.subsidiaryList, true);
    }, (error) => {
      Utils.unblockUI();
      this.toastr.error('There was an error on Saving. Please try again.');
    });
    this.modalRef.hide();
  }

  setLabel() {
    this.applicantData.subsidiaryList.forEach(data => {
      data.typeOfBusiness = LvBusinessType.find(x => Number(x.value) === data.typeOfBusinessId).label;
    });
  }
}
