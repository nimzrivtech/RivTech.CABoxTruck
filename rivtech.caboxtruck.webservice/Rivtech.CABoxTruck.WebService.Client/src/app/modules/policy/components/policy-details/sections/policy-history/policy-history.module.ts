import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { PolicyHistoryRoutingModule } from './policy-history-routing.module';
import { PolicyHistoryComponent } from './policy-history.component';
import { HistorySectionModule } from './history-section/history-section.module';


@NgModule({
  declarations: [PolicyHistoryComponent],
  imports: [
    CommonModule,
    PolicyHistoryRoutingModule,
    HistorySectionModule
  ]
})
export class PolicyHistoryModule { }
