import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { PolicyBindRequestRoutingModule } from './policy-bind-request-routing.module';
import { PolicyBindRequestComponent } from './policy-bind-request.component';
import { BindRequirementsComponent } from './bind-requirements/bind-requirements.component';
import { QuoteConditionComponent } from './quote-condition/quote-condition.component';
import { SharedModule } from '@app/shared/shared.module';
import { DataTablesModule } from 'angular-datatables';
import { IConfig, NgxMaskModule } from 'ngx-mask';
import { BindingData } from '@app/modules/submission/data/binding/binding.data';
import { BsModalService } from 'ngx-bootstrap';
import { BindingComponent } from './binding/binding.component';
import { AddUpdateBindingComponent } from './modals/add-binding/add-update-binding.component';
import { UploadDocumentComponent } from './modals/upload-document/upload-document.component';
import { AddQuoteConditionComponent } from './modals/add-quote-condition/add-quote-condition.component';

const maskConfig: Partial<IConfig> = {
  validation: true,
};

@NgModule({
  declarations: [
    PolicyBindRequestComponent,
    BindingComponent,
    BindRequirementsComponent,
    QuoteConditionComponent,
    AddUpdateBindingComponent,
    UploadDocumentComponent,
    AddQuoteConditionComponent
  ],
  imports: [
    CommonModule,
    SharedModule,
    PolicyBindRequestRoutingModule,
    DataTablesModule,
    NgxMaskModule.forRoot(maskConfig)
  ],
  entryComponents: [
    AddUpdateBindingComponent,
    UploadDocumentComponent,
    AddQuoteConditionComponent
  ],
  providers: [
    BindingData,
    BsModalService
  ]
})
export class PolicyBindRequestModule { }
