import { Component, OnInit } from '@angular/core';
import {FieldConfig} from '../../../models/dynamic/field.interface';
import {FormGroup} from '@angular/forms';

@Component({
  selector: 'app-text-area',
  templateUrl: './text-area.component.html',
  styleUrls: ['./text-area.component.scss']
})
export class TextAreaComponent implements OnInit {
  field: FieldConfig;
  group: FormGroup;

  constructor() { }

  ngOnInit() {
  }
  
  get formControl() { return this.group.controls[this.field.name]; }
}
