import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { GeneralInformationComponent } from './general-information.component';
import {SharedModule} from '../../../../../../../shared/shared.module';
import { RiskSpecificsRoutingModule } from '../risk-specifics-routing.module';
import { NgxMaskModule, IConfig } from 'ngx-mask';
import { SubmissionDetailsSharedModule } from '../../../submission-details-shared/submission-details-shared.module';

const maskConfig: Partial<IConfig> = {
  validation: true,
};

@NgModule({
  declarations: [
    GeneralInformationComponent
  ],
  imports: [
    CommonModule,
    FormsModule,
    SharedModule,
    RiskSpecificsRoutingModule,
    NgxMaskModule.forRoot(maskConfig),    
    ReactiveFormsModule,
    SubmissionDetailsSharedModule,
  ],
  exports: [
    GeneralInformationComponent
  ],  
  providers:[
  ]
})
export class GeneralInformationModule { }
