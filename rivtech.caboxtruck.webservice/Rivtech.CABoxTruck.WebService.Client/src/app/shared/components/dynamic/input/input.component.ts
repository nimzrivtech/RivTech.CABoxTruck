import { Component, OnInit } from '@angular/core';
import {FieldConfig} from '../../../models/dynamic/field.interface';
import {FormGroup} from '@angular/forms';
import { BaseComponent } from '../../../../shared/base-component';

@Component({
  selector: 'app-input',
  templateUrl: './input-v2.component.html',
  styleUrls: ['./input.component.scss']
})
export class InputComponent extends BaseComponent implements OnInit {
  field: FieldConfig;
  group: FormGroup;
  
  constructor() {
    super();
  }

  ngOnInit() {
  }

  get formControl() { return this.group.controls[this.field.name]; }
}
