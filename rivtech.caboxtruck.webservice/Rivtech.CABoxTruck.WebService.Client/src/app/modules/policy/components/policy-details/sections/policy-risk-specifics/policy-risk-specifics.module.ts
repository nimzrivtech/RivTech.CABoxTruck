import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { PolicyRiskSpecificsRoutingModule } from './policy-risk-specifics-routing.module';
import { PolicyRiskSpecificsComponent } from './policy-risk-specifics.component';
import { GeneralLiabilityCargoComponent } from './general-liability-cargo/general-liability-cargo.component';
import { CommoditiesChartComponent } from './general-liability-cargo/cargo-section/commodities-chart/commodities-chart.component';
import { CommoditiesChartFormComponent } from './general-liability-cargo/cargo-section/commodities-chart/commodities-chart-form.component';
import { GeneralLiabilitySectionComponent } from './general-liability-cargo/general-liability-section/general-liability-section.component';
import { CargoSectionComponent } from './general-liability-cargo/cargo-section/cargo-section.component';
import { FormsModule } from '@angular/forms';
import { DriverInformationModule } from './driver-information/driver-information.module';
import { DotInformationModule } from './dot-information/dot-information.module';
import { MaintenanceSafetyModule } from './maintenance-safety/maintenance-safety.module';
import { UnderwritingQuestionsModule } from './underwriting-questions/underwriting-questions.module';
import { DestinationInformationModule } from './destination-information/destination-information.module';
import { SharedModule } from '@app/shared/shared.module';
import { IConfig, NgxMaskModule } from 'ngx-mask';
import { RadiusOfOperationsData } from '@app/modules/submission/data/riskspecifics/radius-of-operations.data';
import { DriverInfoData } from '@app/modules/submission/data/riskspecifics/driver-info.data';
import { DriverHiringCriteriaData } from '@app/modules/submission/data/riskspecifics/driver-hiring-criteria.data';
import { DotInfoData } from '@app/modules/submission/data/riskspecifics/dot-info.data';
import { RiskSpecificsData } from '@app/modules/submission/data/risk-specifics.data';
import { UnderwritingQuestionsData } from '@app/modules/submission/data/riskspecifics/underwriting-questions.data';
import { BsModalService } from 'ngx-bootstrap';

const maskConfig: Partial<IConfig> = {
  validation: false,
};

@NgModule({
  declarations: [
    PolicyRiskSpecificsComponent,
    GeneralLiabilityCargoComponent,
    CommoditiesChartComponent,
    CommoditiesChartFormComponent,
    GeneralLiabilitySectionComponent,
    CargoSectionComponent
  ],
  imports: [
    CommonModule,
    FormsModule,
    PolicyRiskSpecificsRoutingModule,
    DriverInformationModule,
    DotInformationModule,
    MaintenanceSafetyModule,
    UnderwritingQuestionsModule,
    DestinationInformationModule,
    SharedModule,
    NgxMaskModule.forRoot(maskConfig)
  ],
  entryComponents: [
    CommoditiesChartFormComponent
  ],
  providers: [
    RadiusOfOperationsData,
    DriverInfoData,
    DriverHiringCriteriaData,
    DotInfoData,
    RiskSpecificsData,
    UnderwritingQuestionsData,
    BsModalService
  ]
})
export class PolicyRiskSpecificsModule { }
