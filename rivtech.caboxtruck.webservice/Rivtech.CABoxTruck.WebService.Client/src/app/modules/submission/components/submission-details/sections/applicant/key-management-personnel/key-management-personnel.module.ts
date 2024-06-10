import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { KeyManagementPersonnelInfoSectionComponent } from './key-management-personnel-info-section/key-management-personnel-info-section.component';
import { SubsidiariesAffiliatedCompanySectionComponent } from './subsidiaries-affiliated-company-section/subsidiaries-affiliated-company-section.component';
import { KeyManagementPersonnelComponent } from './key-management-personnel.component';
import { NgxMaskModule, IConfig } from 'ngx-mask';
import { ApplicantRoutingModule } from '../applicant-routing.module';
import { SharedModule } from '../../../../../../../shared/shared.module';


const maskConfig: Partial<IConfig> = {
  validation: true,
};

@NgModule({
  declarations: [
    KeyManagementPersonnelComponent,
    KeyManagementPersonnelInfoSectionComponent,
    SubsidiariesAffiliatedCompanySectionComponent
  ],
  imports: [
    CommonModule,
    SharedModule,
    ApplicantRoutingModule,
    NgxMaskModule.forRoot(maskConfig),
    SharedModule
  ],
  exports: [
    KeyManagementPersonnelComponent,
    KeyManagementPersonnelInfoSectionComponent,
    SubsidiariesAffiliatedCompanySectionComponent
  ]
})
export class KeyManagementPersonnelModule { }
