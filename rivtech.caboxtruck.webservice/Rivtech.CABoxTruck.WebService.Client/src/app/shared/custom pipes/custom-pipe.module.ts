import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { NameCasePipe } from './name-pipe';
import { TimerPipe } from './timer-pipe';

@NgModule({
  declarations: [NameCasePipe,  TimerPipe],
  imports: [
    CommonModule
  ],
  exports: [NameCasePipe,  TimerPipe]
})
export class CustomPipesModule { }
