import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HistorySectionComponent } from './history-section.component';
import { DataTableModule } from 'angular2-datatable';
import { SharedModule } from '@app/shared/shared.module';



@NgModule({
  declarations: [HistorySectionComponent],
  imports: [
    CommonModule,
    DataTableModule,
    SharedModule
  ]
})
export class HistorySectionModule { }
