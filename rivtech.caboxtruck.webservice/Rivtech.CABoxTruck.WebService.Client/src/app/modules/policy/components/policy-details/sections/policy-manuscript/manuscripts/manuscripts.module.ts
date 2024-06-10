import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ManuscriptsComponent } from './manuscripts.component';
import { ManuscriptsSectionComponent } from './manuscripts-section/manuscripts-section.component';
import { ManuscriptDialogComponent } from './manuscripts-section/manuscript-dialog.component';
import { SharedModule } from '../../../../../../../shared/shared.module';
import { NgMultiSelectDropDownModule } from 'ng-multiselect-dropdown';
import { DataTablesModule } from 'angular-datatables';
import { ModalModule, TabsModule, PaginationModule} from 'ngx-bootstrap';
import { PolicyManuscriptRoutingModule } from '../policy-manuscript-routing.module';


@NgModule({
  declarations: [
    ManuscriptsComponent,
    ManuscriptsSectionComponent,
    ManuscriptDialogComponent
  ],
  imports: [
    CommonModule,
    PolicyManuscriptRoutingModule,
    NgMultiSelectDropDownModule.forRoot(),
    DataTablesModule,
    ModalModule.forRoot(),
    TabsModule.forRoot(),
    SharedModule,
    PaginationModule.forRoot()
  ],
  exports: [
    ManuscriptDialogComponent,
  ],
  entryComponents: [
    ManuscriptDialogComponent
  ]
})
export class ManuscriptsModule { }
