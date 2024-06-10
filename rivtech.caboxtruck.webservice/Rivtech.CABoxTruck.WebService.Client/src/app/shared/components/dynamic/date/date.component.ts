import { Component, OnInit } from '@angular/core';
import {FieldConfig} from '../../../models/dynamic/field.interface';
import {FormGroup} from '@angular/forms';

@Component({
  selector: 'app-date',
  templateUrl: './date.component.html',
  styleUrls: ['./date.component.scss']
})
export class DateComponent implements OnInit {
  field: FieldConfig;
  group: FormGroup;
  constructor() { }

  ngOnInit() {
  }

  get formControl() { return this.group.controls[this.field.name]; }
}
