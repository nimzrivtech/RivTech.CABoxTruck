import { Component, OnInit } from '@angular/core';
import {FieldConfig} from '../../../models/dynamic/field.interface';
import {FormGroup} from '@angular/forms';

@Component({
  selector: 'app-checkbox',
  templateUrl: './checkbox.component.html',
  styleUrls: ['./checkbox.component.scss']
})
export class CheckboxComponent implements OnInit {
  field: FieldConfig;
  group: FormGroup;
  constructor() { }

  ngOnInit() {
  }

}
