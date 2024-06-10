import { Component, OnDestroy, OnInit } from '@angular/core';
import { ApplicantData, ApplicantInformationDropDowns } from '../../../../../../../../modules/submission/data/applicant/applicant.data';
import { LvAccountCategory, LvBusinessType, LvGVWCategoryBeforeV7, LvGVWCategoryV7, LvMainUse, LvNewVenturePreviousExperience, LvSecondaryBusinessGroup } from '../../../../../../../../shared/constants/business-details.options.constants';
import { SubmissionData } from '../../../../../../../../modules/submission/data/submission.data';
import { BsModalRef, BsModalService } from 'ngx-bootstrap';
import { SubsidiaryDialogComponent } from './subsidiary-dialog/subsidiary-dialog.component';
import NotifUtils from '../../../../../../../../shared/utilities/notif-utils';
import { ApplicantBusinessDetailsService } from '../../../../../../../../modules/submission/services/applicant-business-details.service';
import { CompanyDialogComponent } from './company-dialog/company-dialog.component';
import { TableConstants } from '../../../../../../../../shared/constants/table.constants';
import { TableData } from '../../../../../../../../modules/submission/data/tables.data';
import { NULL_EXPR } from '@angular/compiler/src/output/output_ast';
import { BusinessDetailsData } from '../../../../../../../../modules/submission/data/applicant/business-details.data';
import { Validators } from '@angular/forms';
import { Subscription } from 'rxjs';

@Component({
  selector: 'app-business-details-section',
  templateUrl: './business-details-section.component.html',
  styleUrls: ['./business-details-section.component.scss']
})
export class BusinessDetailsSectionComponent implements OnInit, OnDestroy {
  businessTypeList = LvBusinessType;
  mainUseList = LvMainUse;
  lists: ApplicantInformationDropDowns;

  accountCategoryList = LvAccountCategory;
  newVenturePreviousExperienceList = LvNewVenturePreviousExperience;
  year = (new Date()).getFullYear();
  TableConstants = TableConstants;

  hideMe: boolean = false;

  subscriptions: Subscription[] = [];

  modalRef: BsModalRef;
  dtOptions: DataTables.Settings = {};
  constructor(public applicantData: ApplicantData,
    public submissionData: SubmissionData,
    private modalService: BsModalService,
    private applicantBusinessDetailsService: ApplicantBusinessDetailsService,
    private tableData: TableData,
    private businessDetailsData: BusinessDetailsData
    ) { }

  ngOnInit() {
    // event listener comes first.
    this.subscriptions.push(this.f['yearBusinessInput'].valueChanges.subscribe(() => this.updateNewVentureControls()));
    this.applicantData.setApplicantDataset();
    this.lists = this.applicantData.applicantDropdownList;
    this.businessDetailsData.resetFields(); 
    this.getBusinessDetails();
    this.dtOptions = {
      pagingType: 'full_numbers',
      responsive: true,
      processing: true,
      destroy: true
    };
  }

  ngOnDestroy(): void {
    this.subscriptions.forEach(subs => subs.unsubscribe());
  }

  // convenience getter for easy access to form fields
  get f() { return this.applicantData.applicantForms.businessDetails.controls; }
  get formName() { return 'businessDetailsForm'; }

  get isYearsInBusinessZero() { return this.f['yearBusinessInput'].value <= 0; }

  getBusinessDetails() {
    if (this.submissionData.riskDetail.businessDetails == null) {
      return;
    }
    const defaultValueMainGVW = this.submissionData.raterVersionSupport() ? 5 : 1;
    this.applicantData.applicantForms.businessDetails.controls['newVentureSwitch'].patchValue(this.submissionData.riskDetail.businessDetails.isNewVenture);
    this.applicantData.applicantForms.businessDetails.controls['yearEstablishedInput'].patchValue(this.submissionData.riskDetail.businessDetails.yearEstablished);
    this.applicantData.applicantForms.businessDetails.controls['firstYearUnderCurrentInput'].patchValue(this.submissionData.riskDetail.businessDetails.firstYearUnderCurrentManagement);
    this.applicantData.applicantForms.businessDetails.controls['newVenturePreviousExperienceDropdown'].patchValue(this.submissionData.riskDetail.businessDetails.newVenturePreviousExperienceId);
    this.applicantData.applicantForms.businessDetails.controls['yearBusinessInput'].patchValue(this.submissionData.riskDetail.businessDetails.yearsInBusiness);
    this.applicantData.applicantForms.businessDetails.controls['yearUnderCurrentInput'].patchValue(this.submissionData.riskDetail.businessDetails.yearsUnderCurrentManagement);
    this.applicantData.applicantForms.businessDetails.controls['businessTypeDropdown'].patchValue(this.submissionData.riskDetail.businessDetails.businessTypeId);
    // this.applicantData.applicantForms.businessDetails.controls['mainUseDropdown'].patchValue(this.submissionData.riskDetail.businessDetails?.mainUseId ?? 1);
    // this.applicantData.applicantForms.businessDetails.controls['mainGVW'].patchValue(this.submissionData.riskDetail.businessDetails?.mainGVWId ?? defaultValueMainGVW);
    // this.applicantData.applicantForms.businessDetails.controls['secondaryBusinessGroupDropdown'].patchValue(this.submissionData.riskDetail.businessDetails?.secondaryBusinessGroupId ?? 1);
    // this.applicantData.applicantForms.businessDetails.controls['accountCategoryDropdown'].patchValue(this.submissionData.riskDetail.businessDetails?.accountCategoryId ?? 1);
    // this.applicantData.applicantForms.businessDetails.controls['accountCategoryUWDropdown'].patchValue(this.submissionData.riskDetail.businessDetails?.accountCategoryUWId ?? 1);
    this.applicantData.applicantForms.businessDetails.controls['descOperationsInput'].patchValue(this.submissionData.riskDetail.businessDetails.descOperations);
    this.applicantData.applicantForms.businessDetails.controls['federalIdInput'].patchValue(this.submissionData.riskDetail.businessDetails.federalIDNumber);
    this.applicantData.applicantForms.businessDetails.controls['naicsCodeInput'].patchValue(this.submissionData.riskDetail.businessDetails.naicsCode);
    this.applicantData.applicantForms.businessDetails.controls['anySubsidiariesCompaniesSwitch'].patchValue(this.submissionData.riskDetail.businessDetails.hasSubsidiaries);
    this.applicantData.applicantForms.businessDetails.controls['companyPastSwitch'].patchValue(this.submissionData.riskDetail.businessDetails.hasCompanies);

    this.applicantData.subsidiaryList = this.submissionData.riskDetail.businessDetails.subsidiaries ? this.submissionData.riskDetail.businessDetails.subsidiaries : [] ;
    this.applicantData.companyList = this.submissionData.riskDetail.businessDetails.companies ? this.submissionData.riskDetail.businessDetails.companies : [];

    this.setLabel();
    this.businessDetailsData.setSubsidiaryPage(1, this.applicantData.subsidiaryList);
    this.businessDetailsData.setPreviousCompanyPage(1, this.applicantData.companyList);

    if (!this.submissionData.riskDetail.businessDetails.hasSubsidiaries) {
      if (this.applicantData.subsidiaryList.length > 0) {
        this.applicantBusinessDetailsService.deleteAllApplicantSubsidiary(this.submissionData.riskDetail.id).subscribe(() => {
          this.applicantData.subsidiaryList = [];
          this.submissionData.riskDetail.businessDetails.subsidiaries = [];
          this.businessDetailsData.subsidiaryListItems = [];
          this.businessDetailsData.subsidiaryListPager = {};
        });
      }
    }

    if (!this.submissionData.riskDetail.businessDetails.hasCompanies) {
      if (this.applicantData.companyList.length > 0) {
        this.applicantBusinessDetailsService.deleteAllApplicantCompany(this.submissionData.riskDetail.id).subscribe(() => {
          this.applicantData.companyList = [];
          this.submissionData.riskDetail.businessDetails.companies = [];
          this.businessDetailsData.companyListItems = [];
          this.businessDetailsData.companyListPager = {};
        });
      }
    }
  }

  /**
   * Updates New Venture controls when Years In Business input changes.
   */
  updateNewVentureControls() {
    let ctrlNewVentureDropdown = this.f['newVenturePreviousExperienceDropdown'];
    let ctrlNewVentureSwitch = this.f['newVentureSwitch'];
    if(this.isYearsInBusinessZero) {
      ctrlNewVentureDropdown.setValidators([Validators.required]);
      ctrlNewVentureSwitch.patchValue(true);
      ctrlNewVentureSwitch.disable();
    } else {
      ctrlNewVentureDropdown.clearValidators();
      ctrlNewVentureSwitch.enable();
    }
    ctrlNewVentureSwitch.updateValueAndValidity();
    ctrlNewVentureDropdown.updateValueAndValidity();
  }

  onSwitchClick(fieldName: string) {
    const field = this.f[fieldName];
    switch (fieldName) {
      case 'newVentureSwitch': {
        if (field.value === true) {
          this.setAsRequired('newVenturePreviousExperienceDropdown');
        } else {
          this.resetField('newVenturePreviousExperienceDropdown');
        }
        break;
      }
      case 'anySubsidiariesCompaniesSwitch': {
        if (field.value === false && this.applicantData.subsidiaryList.length > 0) {
          NotifUtils.showConfirmMessage('Switching back to NO will remove any changes made in the Subsidiaries/Affiliated Companies.<br><br>Are you sure you want to continue?', () => {
            this.applicantBusinessDetailsService.deleteAllApplicantSubsidiary(this.submissionData.riskDetail.id).subscribe(() => {
              this.applicantData.subsidiaryList = [];
              this.submissionData.riskDetail.businessDetails.subsidiaries = [];
              this.businessDetailsData.subsidiaryListItems = [];
              this.businessDetailsData.subsidiaryListPager = {};
            });
          }, () => {
            this.f[fieldName].patchValue(true);
          });
        }
        break;
      }
      case 'companyPastSwitch': {
        if (field.value === false && this.applicantData.companyList.length > 0) {
          NotifUtils.showConfirmMessage('Switching back to NO will remove any changes made in the Previous Companies.<br><br>Are you sure you want to continue?', () => {
            this.applicantBusinessDetailsService.deleteAllApplicantCompany(this.submissionData.riskDetail.id).subscribe(() => {
              this.applicantData.companyList = [];
              this.submissionData.riskDetail.businessDetails.companies = [];
              this.businessDetailsData.companyListItems = [];
              this.businessDetailsData.companyListPager = {};
            });
          }, () => {
            this.f[fieldName].patchValue(true);
          });
        }
        break;
      }
    }
  }

  computeYearsBusiness(value) {
    if (value === '' || value === undefined || value > this.year || value < 1900) {
      this.f['firstYearUnderCurrentInput'].patchValue(null);
      this.f['yearBusinessInput'].patchValue(null);
      this.f['yearUnderCurrentInput'].patchValue(null);
      return;
    }
    let yearBusiness = this.year - value;
    if (yearBusiness < 0) {
      yearBusiness = 0;
    }
    this.f['yearBusinessInput'].patchValue(yearBusiness);

    this.f['firstYearUnderCurrentInput'].patchValue(value);
    this.f['yearUnderCurrentInput'].patchValue(yearBusiness);
  }

  computeYearsUnderCurrent(value) {
    if (value === '' || value === undefined) {
      this.f['yearUnderCurrentInput'].patchValue(null);
      return;
    }
    let yearUnderCurrent = this.year - value;
    if (yearUnderCurrent < 0) {
      yearUnderCurrent = null;
    }
    this.f['yearUnderCurrentInput'].patchValue(yearUnderCurrent);
  }

  changeAccountCategory() {
    this.f['accountCategoryUWDropdown'].patchValue(this.f['accountCategoryDropdown'].value);
  }

  setTooltip(field, dataList) {
    if (field && dataList?.length > 0) {
      const data = dataList.find(x => x.value == field);
      if (data) {
        return data.label;
      }
    }
  }

  private resetField(fieldName: string) {
    // this.f[fieldName].reset();
    // this.f[fieldName].clearValidators();
    // this.f[fieldName].updateValueAndValidity();
  }

  private setAsRequired(fieldName: string) {
    // this.f[fieldName].setValidators(Validators.required);
    // this.f[fieldName].updateValueAndValidity();
  }

  public ToggleHiding(): void {
    this.hideMe = !this.hideMe;
  }

  setLabel() {
    this.applicantData.subsidiaryList.forEach(data => {
      data.typeOfBusiness = LvBusinessType.find(x => Number(x.value) === data.typeOfBusinessId)?.label;
    });
  }

  openAddSubsidiaryModal() {
    this.modalRef = this.modalService.show(SubsidiaryDialogComponent, {
      initialState: {
        isEdit: false
      },
      backdrop: 'static',
      keyboard: false,
      class: 'modal-md'
    });
  }

  openEditSubsidiaryModal(item) {
    this.modalRef = this.modalService.show(SubsidiaryDialogComponent, {
      initialState: {
        isEdit: true,
        modalTitle: 'Edit Subsidiary/Affiliate',
        modalButton: 'Save',
        editItem: item
      },
      backdrop: 'static',
      keyboard: false,
      class: 'modal-md'
    });
  }

  deleteSubsidiary(item) {
    NotifUtils.showConfirmMessage(`Are you sure you want to delete '${item?.name}'?`, () => {
      this.applicantBusinessDetailsService.deleteApplicantSubsidiary(item.id).subscribe(() => {
        const index: number = this.applicantData.subsidiaryList.indexOf(item);
        if (index !== -1) {
          this.applicantData.subsidiaryList.splice(index, 1);
          this.submissionData.riskDetail.businessDetails.subsidiaries = this.applicantData.subsidiaryList;
          this.businessDetailsData.setSubsidiaryPage(this.businessDetailsData.subsidiaryListCurrentPage, this.applicantData.subsidiaryList);
        }
      });
    });
  }

  openAddCompanyModal() {
    this.modalRef = this.modalService.show(CompanyDialogComponent, {
      initialState: {
        isEdit: false
      },
      backdrop: 'static',
      keyboard: false,
      class: 'modal-md'
    });
  }

  openEditCompanyModal(item) {
    this.modalRef = this.modalService.show(CompanyDialogComponent, {
      initialState: {
        isEdit: true,
        modalTitle: 'Edit Previous Company',
        modalButton: 'Save',
        editItem: item
      },
      backdrop: 'static',
      keyboard: false,
      class: 'modal-md'
    });
  }

  deleteCompany(item) {
    NotifUtils.showConfirmMessage(`Are you sure you want to delete '${item?.name}'?`, () => {
      this.applicantBusinessDetailsService.deleteApplicantCompany(item.id).subscribe(() => {
        const index: number = this.applicantData.companyList.indexOf(item);
        if (index !== -1) {
          this.applicantData.companyList.splice(index, 1);
          this.submissionData.riskDetail.businessDetails.companies = this.applicantData.companyList;
          this.businessDetailsData.setPreviousCompanyPage(this.businessDetailsData.companyListCurrentPage, this.applicantData.companyList);
        }
      });
    });
  }
}
