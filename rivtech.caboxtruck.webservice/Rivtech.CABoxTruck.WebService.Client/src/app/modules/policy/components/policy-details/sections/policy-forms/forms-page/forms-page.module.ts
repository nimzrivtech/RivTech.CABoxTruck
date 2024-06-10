import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsSectionComponent } from './forms-section/forms-section.component';
import { FormsPageComponent } from './forms-page.component';
import { SharedModule } from '../../../../../../../shared/shared.module';
import { FormMcs90Component } from './forms-section/form-popup/form-mcs90/form-mcs90.component';
import { FormExclusionRadiusComponent } from './forms-section/form-popup/form-exclusion-radius/form-exclusion-radius.component';
import { FormHiredPhysdamComponent } from './forms-section/form-popup/form-hired-physdam/form-hired-physdam.component';
import { FormDriverRequirementComponent } from './forms-section/form-popup/form-driver-requirement/form-driver-requirement.component';
import { FormTerritoryExclusionComponent } from './forms-section/form-popup/form-territory-exclusion/form-territory-exclusion.component';
import { FormTrailerInterchangeComponent } from './forms-section/form-popup/form-trailer-interchange/form-trailer-interchange.component';
import { FormGeneralLiabilityComponent } from './forms-section/form-popup/form-general-liability/form-general-liability.component';
import { NgMultiSelectDropDownModule } from 'ng-multiselect-dropdown';
import { FormUploadComponent } from './forms-section/form-upload/form-upload.component';
import { PolicyFormsRoutingModule } from '../policy-forms-routing.module';
import { FormsData } from '../../../../../../../modules/submission/data/forms/forms.data';
import { LimitsData } from '../../../../../../../modules/submission/data/limits/limits.data';
import { BsModalService } from 'ngx-bootstrap';
import { FormLossPayeeComponent } from './forms-section/form-popup/form-loss-payee/form-loss-payee.component';
import { FormPAaddcombFPBComponent } from './forms-section/form-popup/form-pa-addcomb-fpb/form-pa-addcomb-fpb.component';
import { FormCoveredAutoDesignationComponent } from './forms-section/form-popup/form-covered-auto-designation/form-covered-auto-designation.component';


@NgModule({
  declarations: [
    FormsPageComponent,
    FormsSectionComponent,
    FormUploadComponent,
    FormExclusionRadiusComponent,
    FormHiredPhysdamComponent,
    FormDriverRequirementComponent,
    FormTerritoryExclusionComponent,
    FormTrailerInterchangeComponent,
    FormGeneralLiabilityComponent,
    FormMcs90Component,
    FormLossPayeeComponent,
    FormPAaddcombFPBComponent,
    FormCoveredAutoDesignationComponent
  ],
  imports: [
    CommonModule,
    SharedModule,
    PolicyFormsRoutingModule,
    NgMultiSelectDropDownModule.forRoot()
  ],
  entryComponents: [
    FormUploadComponent,
    FormExclusionRadiusComponent,
    FormHiredPhysdamComponent,
    FormDriverRequirementComponent,
    FormTerritoryExclusionComponent,
    FormTrailerInterchangeComponent,
    FormGeneralLiabilityComponent,
    FormMcs90Component,
    FormLossPayeeComponent,
    FormPAaddcombFPBComponent,
    FormCoveredAutoDesignationComponent
  ],
  providers: [
    FormsData,
    LimitsData,
    BsModalService
  ]
})
export class FormsPageModule { }
