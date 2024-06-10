import { Component, OnInit } from '@angular/core';
import {FieldConfig} from '../../../models/dynamic/field.interface';
import {FormGroup} from '@angular/forms';

@Component({
  selector: 'app-select',
  templateUrl: './select.component.html',
  styleUrls: ['./select.component.scss']
})
export class SelectComponent implements OnInit {
  field: FieldConfig;
  group: FormGroup;

  constructor() { }

  ngOnInit() {
  }
  
  get formControl() { return this.group.controls[this.field.name]; }

  trackByFn(index, item) {
    return index; // or item.id
  }

}
