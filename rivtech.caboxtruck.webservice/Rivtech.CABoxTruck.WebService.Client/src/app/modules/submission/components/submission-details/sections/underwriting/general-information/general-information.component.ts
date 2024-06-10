import { Component, OnInit, ViewChild } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ConfirmationModalComponent } from '../../../../../../../shared/components/confirmation-modal/confirmation-modal.component';
import { DataTableDirective } from 'angular-datatables';
import { BsModalRef, BsModalService } from 'ngx-bootstrap';
import { Subject } from 'rxjs';
import { Router } from '@angular/router';

import { ToggleCollapseComponent } from '../../../../../../../shared/components/toggle-collapse/toggle-collapse.component';

@Component({
  selector: 'app-general-information',
  templateUrl: './general-information.component.html',
  styleUrls: ['./general-information.component.scss']
})
export class GeneralInformationComponent implements OnInit {
  underwritingGeneralInformation: FormGroup;
  additionalInterestForm: FormGroup;
  submitted = false;

  hideMe: boolean = false;

  constructor(private formBuilder: FormBuilder,
    private modalService: BsModalService,
    private router: Router
    ) { }

    @ViewChild(ToggleCollapseComponent) toggleCollapse : ToggleCollapseComponent;

  hasIsCameraDiscount: boolean = false;
  hasIsCrashAvoidDiscount: boolean = false;
  hasIsDistractedDiscount: boolean = false;
  hasIsMileageTrackingEquipment: boolean = false;
  hasIsNavigationGPS: boolean = false;

  // table settings
  @ViewChild(DataTableDirective)
  dtElement: DataTableDirective;
  dtOptions: DataTables.Settings = {};
  dtTrigger: Subject<any> = new Subject();
  isDtInitialized: boolean = false;

  modalRef: BsModalRef | null;


  ngOnInit() {

    this.underwritingGeneralInformation = this.formBuilder.group({
      cameraDiscountSwitch:[''],
      crashAvoidDiscountSwitch:[''],
      distractedDiscountSwitch:[''],
      mileageTrackingEquipmentSwitch:[''],
      navigationGPSSwitch:[''],
      creditScoreDropdown: ['', Validators.required],
      onlineProfileScoreDropdown: ['', Validators.required],
      hiringCriteriaDropdown: ['', Validators.required],
      highLowTurnOverDropdown: ['', Validators.required],
      safetyCriteriaDropdown: ['', Validators.required],
      driverLicenseInput: ['', Validators.required],
      anyVehiclesOperatedDropdown: ['', Validators.required],
      policyLevelAccidentInput: ['', Validators.required],
      ownerOperatorInput: ['', Validators.required],
      claimsFreeLastThreeYearsDropdown: ['', Validators.required]

    });

    this.additionalInterestForm = this.formBuilder.group({
      additionalInsuredInput: ['', Validators.required],
      waiverSubrogationInput: ['', Validators.required],
      primaryContributorCoverageInput: ['', Validators.required],
    });

    this.dtOptions = {
      pagingType: 'full_numbers',
      responsive: true,
      processing: true,
      destroy: true
    };

    this.renderTable();
  }

  // convenience getter for easy access to form fields
  get f() { return this.underwritingGeneralInformation.controls; }
  get g() { return this.additionalInterestForm.controls; }

  onSubmit() {
      this.submitted = true;

      // stop here if form is invalid
      // if (this.brokerGeneralInformation.invalid) {
      //     return;
      // }
      this.router.navigate(['/submissions/new/vehicle/vehicle-info']);
      // display form values on success
  }

  onReset() {
      this.submitted = false;
      this.underwritingGeneralInformation.reset();
  }

  modelOwnerOperatorChanged(value){
    if (value) {
      this.underwritingGeneralInformation.controls['ownerOperatorInput'].clearValidators();
      this.underwritingGeneralInformation.controls['ownerOperatorInput'].updateValueAndValidity();
    } else {
      this.underwritingGeneralInformation.controls['ownerOperatorInput'].setValidators([Validators.required]);
      this.underwritingGeneralInformation.controls['ownerOperatorInput'].updateValueAndValidity();
    }
  }

  modelAdditionalInsuredChanged(value){
    if (value) {
      this.additionalInterestForm.controls['additionalInsuredInput'].clearValidators();
      this.additionalInterestForm.controls['additionalInsuredInput'].updateValueAndValidity();
    } else {
      this.additionalInterestForm.controls['additionalInsuredInput'].setValidators([Validators.required]);
      this.additionalInterestForm.controls['additionalInsuredInput'].updateValueAndValidity();
    }
  }

  modelWaiverSubrogationChanged(value){
    if (value) {
      this.additionalInterestForm.controls['waiverSubrogationInput'].clearValidators();
      this.additionalInterestForm.controls['waiverSubrogationInput'].updateValueAndValidity();
    } else {
      this.additionalInterestForm.controls['waiverSubrogationInput'].setValidators([Validators.required]);
      this.additionalInterestForm.controls['waiverSubrogationInput'].updateValueAndValidity();
    }
  }

  modelPrimaryCoverageChanged(value){
    if (value) {
      this.additionalInterestForm.controls['primaryContributorCoverageInput'].clearValidators();
      this.additionalInterestForm.controls['primaryContributorCoverageInput'].updateValueAndValidity();
    } else {
      this.additionalInterestForm.controls['primaryContributorCoverageInput'].setValidators([Validators.required]);
      this.additionalInterestForm.controls['primaryContributorCoverageInput'].updateValueAndValidity();
    }
  }


  isCameraDiscountSwitch(value){
    /** Fill this if condition in camera discount has */
    if (value == 'on') {
    } else if(value == 'off') {
    }
  }

  isDistractedDiscountSwitch(value){
    /** Fill this if condition in camera discount has */
    if (value == 'on') {
    } else if(value == 'off') {
    }
  }

  isCrashAvoidDiscountSwitch(value){
    /** Fill this if condition in camera discount has */
    if (value == 'on') {
    } else if(value == 'off') {
    }
  }

  isMileageTrackingEquipmentSwitch(value){
    /** Fill this if condition in camera discount has */
    if (value == 'on') {
    } else if(value == 'off') {
    }
  }

  isNavigationGPSSwitch(value){
    /** Fill this if condition in camera discount has */
    if (value == 'on') {
    } else if(value == 'off') {
    }
  }

  renderTable(): void {
    if (this.isDtInitialized) {
      this.dtElement.dtInstance.then((dtInstance: DataTables.Api) => {
        // Destroy the table first
        dtInstance.destroy();
        // Call the dtTrigger to rerender again
        this.dtTrigger.next();
      });
    }
    else {
      this.dtTrigger.next();
      this.isDtInitialized = true;
    }
  }

  onDelete() {
    this.modalRef = this.modalService.show(ConfirmationModalComponent, {
      initialState: { message: 'Are you sure you want to remove ?' },
      class: 'modal-sm modal-dialog-centered',
      backdrop: 'static',
      keyboard: false
    });

    this.modalRef.content.event.subscribe((result) => {
      // if (result.data) {
      //   this.deleteAttachedStructure.emit(attachedStructure);
      // }
      this.modalRef.hide();
    });
  }

  onEdit(id){
    //this.getRiskAdditionalInterest(id);
  }

  clearValidators(controlName: string[]) {
    controlName.forEach(eachControl => {
      this.underwritingGeneralInformation.controls[eachControl].clearValidators();
      this.underwritingGeneralInformation.controls[eachControl].updateValueAndValidity();
    });
  }

  addValidators(controlName: string[]) {
    controlName.forEach(eachControl => {
      this.underwritingGeneralInformation.controls[eachControl].setValidators([Validators.required]);
      this.underwritingGeneralInformation.controls[eachControl].updateValueAndValidity();
    });
  }


  clearValidatorsAdditionalInterest(controlName: string[]) {
    controlName.forEach(eachControl => {
      this.underwritingGeneralInformation.controls[eachControl].clearValidators();
      this.underwritingGeneralInformation.controls[eachControl].updateValueAndValidity();
    });
  }

  addValidatorsAdditionalInterest(controlName: string[]) {
    controlName.forEach(eachControl => {
      this.underwritingGeneralInformation.controls[eachControl].setValidators([Validators.required]);
      this.underwritingGeneralInformation.controls[eachControl].updateValueAndValidity();
    });
  }
}
