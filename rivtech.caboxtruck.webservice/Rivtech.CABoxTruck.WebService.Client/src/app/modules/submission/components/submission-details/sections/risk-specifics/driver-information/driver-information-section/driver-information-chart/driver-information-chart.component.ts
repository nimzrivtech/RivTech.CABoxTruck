import { Component, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, FormControl, Validators } from '@angular/forms';
import { BaseComponent } from '../../../../../../../../../shared/base-component';

@Component({
  selector: 'app-driver-information-chart',
  templateUrl: './driver-information-chart.component.html',
  styles: [`
  .col-s{ width: 22%; }`
  ]
})
export class DriverInformationChartComponent extends BaseComponent implements OnInit {
  form: FormGroup; 

  constructor( private fb: FormBuilder) {
    super();
    this.form = fb.group({
      maxHrsPerDayDrive: new FormControl('', [Validators.required]),
      maxHrsPerDayDuty: new FormControl('', [Validators.required]),
      maxHrsPerWeekDrive: new FormControl('', [Validators.required]),
      maxHrsPerWeekDuty: new FormControl('', [Validators.required])
    });  
  }

  ngOnInit() {
  }

}
