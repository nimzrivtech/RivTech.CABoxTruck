import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { PolicyIssuanceRoutingModule } from './policy-issuance-routing.module';
import { PolicyIssuanceComponent } from './policy-issuance.component';
import { PolicyChangesSectionComponent } from './policy-changes-section/policy-changes-section.component';
import { SharedModule } from '@app/shared/shared.module';
import { PremiumChangesComponent } from './premium-changes/premium-changes.component';
import { EndorsementProcessModalComponent } from './endorsement-process-modal/endorsement-process-modal.component';
import { EndorsementProcessStatusLogRetriever } from '@app/modules/policy/data/endorsement-process-status-log-retriever';
import { ModalModule } from 'ngx-bootstrap';
import { EffectiveDateModalComponent } from './effective-date-modal/effective-date-modal.component';
import { FormsModule } from '@angular/forms';


@NgModule({
  declarations: [
    PolicyIssuanceComponent, 
    PolicyChangesSectionComponent, 
    PremiumChangesComponent, 
    EndorsementProcessModalComponent,
    EffectiveDateModalComponent
  ],
  imports: [
    CommonModule,
    PolicyIssuanceRoutingModule,
    SharedModule,
    ModalModule.forRoot(),
    FormsModule
  ],
  entryComponents: [
    EndorsementProcessModalComponent,
    EffectiveDateModalComponent
  ],
  providers: [
    EndorsementProcessStatusLogRetriever
  ]
})
export class PolicyIssuanceModule { }
