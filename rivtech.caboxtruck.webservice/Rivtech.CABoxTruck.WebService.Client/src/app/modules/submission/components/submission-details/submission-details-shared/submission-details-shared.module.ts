import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { NextBackButtonComponent } from './components/next-back-button/next-back-button.component';
import { SubmissionActionBarComponent } from './components/submission-action-bar/submission-action-bar.component';
import { LaddaModule } from 'angular2-ladda';
import { RatingModule } from '@app/modules/rating/rating.module';
import { XpRaterUpdateAlertComponent } from './components/xp-rater-update-alert/xp-rater-update-alert.component';



@NgModule({
  declarations: [
    NextBackButtonComponent,
    SubmissionActionBarComponent,
    XpRaterUpdateAlertComponent,
  ],
  exports: [
    NextBackButtonComponent,
    SubmissionActionBarComponent,
    XpRaterUpdateAlertComponent,
  ],
  imports: [
    CommonModule,
    RatingModule,
  ],
  entryComponents: [
  ]
})
export class SubmissionDetailsSharedModule { }
