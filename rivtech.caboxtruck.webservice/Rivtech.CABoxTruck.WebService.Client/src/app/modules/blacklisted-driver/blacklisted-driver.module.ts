import {NgModule} from '@angular/core';
import {SharedModule} from '../../shared/shared.module';
import { CommonModule } from '@angular/common';
import {ModalModule} from 'ngx-bootstrap/modal';

import { BlacklistedDriverData } from './data/blacklisted-driver.data';
import {AddDriverVerificationModalComponent} from './components/blacklisted-driver-modal/add-blacklisted-driver-modal/add-driver-verification-modal.component';
import { BlacklistedDriverModalComponent } from './components/blacklisted-driver-modal/blacklisted-driver-modal.component';

@NgModule({
  declarations: [
    AddDriverVerificationModalComponent,
    BlacklistedDriverModalComponent
  ],
  imports: [
    CommonModule,
    SharedModule,
    ModalModule.forRoot(),
  ],
  providers: [BlacklistedDriverData],
  entryComponents: [AddDriverVerificationModalComponent, BlacklistedDriverModalComponent]
})
export class BlacklistedDriverModule { }
