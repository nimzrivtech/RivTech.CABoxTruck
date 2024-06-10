import { Component, OnInit } from '@angular/core';
import { FormGroup } from '@angular/forms';
import { FieldConfig } from '../../../../shared/models/dynamic/field.interface';
import { IDropdownSettings } from 'ng-multiselect-dropdown';

@Component({
  selector: 'app-multiselect',
  templateUrl: './multiselect.component.html',
  styleUrls: ['./multiselect.component.scss']
})
export class MultipleSelectComponent implements OnInit {  
  field: FieldConfig;
  group: FormGroup;
  dropdownSettings: IDropdownSettings = {};
  constructor() { }
  
  get formControl() { return this.group.controls[this.field.name]; }

  ngOnInit() {
    this.dropdownSettings = {
      singleSelection: false,
      idField: 'value',
      textField: 'label',
      selectAllText: 'Select All',
      unSelectAllText: 'Unselect All',
      itemsShowLimit: 3,
      allowSearchFilter: true
    };
  }

  onItemSelect(item: any) {
    console.log(item);
  }
  onSelectAll(items: any) {
    console.log(items);
  }
}
