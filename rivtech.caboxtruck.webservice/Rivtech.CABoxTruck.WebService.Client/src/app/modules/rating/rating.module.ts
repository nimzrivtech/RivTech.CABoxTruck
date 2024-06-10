import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { PremiumCalculateButtonComponent } from './components/premium-calculate-button/premium-calculate-button.component';
import { LaddaModule } from 'angular2-ladda';



@NgModule({
  declarations: [
    PremiumCalculateButtonComponent,
  ],
  imports: [
    CommonModule,
    LaddaModule,
  ],
  exports: [
    PremiumCalculateButtonComponent
  ]
})
export class RatingModule { }
