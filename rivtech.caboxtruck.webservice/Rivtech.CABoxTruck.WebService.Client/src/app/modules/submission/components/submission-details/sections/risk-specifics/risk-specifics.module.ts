import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RiskSpecificsComponent } from './risk-specifics.component';
import { FormsModule } from '@angular/forms';
import { RiskSpecificsRoutingModule } from './risk-specifics-routing.module';
import { GeneralInformationModule } from './general-information/general-information.module';
import { DriverInformationModule } from './driver-information/driver-information.module';
import { RouterModule } from '@angular/router';
import { MaintenanceSafetyModule } from './maintenance-safety/maintenance-safety.module';
import { SharedModule } from '../../../../../../shared/shared.module';
import { UnderwritingQuestionsModule } from './underwriting-questions/underwriting-questions.module';
import { DestinationInformationModule } from './destination-information/destination-information.module';
import { GeneralLiabilityCargoComponent } from './general-liability-cargo/general-liability-cargo.component';
import { GeneralLiabilitySectionComponent } from './general-liability-cargo/general-liability-section/general-liability-section.component';
import { CargoSectionComponent } from './general-liability-cargo/cargo-section/cargo-section.component';
import { DotInformationModule } from './dot-information/dot-information.module';
import { CommoditiesChartComponent } from './general-liability-cargo/cargo-section/commodities-chart/commodities-chart.component';
import { CommoditiesChartFormComponent } from './general-liability-cargo/cargo-section/commodities-chart/commodities-chart-form.component';
import { IConfig, NgxMaskModule } from 'ngx-mask';
import { SubmissionDetailsSharedModule } from '../../submission-details-shared/submission-details-shared.module';

const maskConfig: Partial<IConfig> = {
  validation: false,
};

@NgModule({
  declarations: [
    RiskSpecificsComponent,
    GeneralLiabilityCargoComponent,
    CommoditiesChartComponent,
    CommoditiesChartFormComponent,
    GeneralLiabilitySectionComponent,
    CargoSectionComponent
  ],
  imports: [
    CommonModule,
    FormsModule,
    RouterModule,
    RiskSpecificsRoutingModule,
    GeneralInformationModule,
    DriverInformationModule,
    DotInformationModule,
    MaintenanceSafetyModule,
    UnderwritingQuestionsModule,
    DestinationInformationModule,
    SharedModule,
    NgxMaskModule.forRoot(maskConfig),
    SubmissionDetailsSharedModule,
  ],
  exports: [
  ]
})
export class RiskSpecificsModule { }
