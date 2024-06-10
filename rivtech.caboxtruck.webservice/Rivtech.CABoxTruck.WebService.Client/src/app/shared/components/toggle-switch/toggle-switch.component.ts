import { EventEmitter, Input, Output } from '@angular/core';
import { Component, OnInit } from '@angular/core';
import { FormGroup } from '@angular/forms';
import { RiskSpecificsData } from '../../../modules/submission/data/risk-specifics.data';
import { ComponentField, ComponentListConstants } from '../../constants/component-list.constants';

@Component({
  selector: 'toggle-switch',
  templateUrl: './toggle-switch.component.html',
  styleUrls: ['./toggle-switch.component.scss']
})

export class ToggleSwitchComponent implements OnInit {
  @Input() formGroup: FormGroup;
  @Input() formName: string;
  @Input() fieldName: string;
  @Input() tabIndex: string = "0";

  @Output() myChange = new EventEmitter();
  itemList: ComponentField[];
  field: ComponentField;
  nextIndex: number;

  get formField() { return this.formGroup.controls[this.fieldName]; }

  constructor() { }

  ngOnInit() {
    this.itemList = ComponentListConstants.find(x => x.FormName === this.formName)?.ComponentList;
    const fieldIndex = this.itemList.findIndex(x => x.fieldName === this.fieldName);
    if (fieldIndex > -1) {
      this.field = this.itemList[fieldIndex];
      this.nextIndex = fieldIndex + 1;
    }
  }

  switchKeyPress(key: string) {
    key = key.toLowerCase();
    switch (key) {
      case 'y':
        this.formField.setValue(true);
        this.focusOnItem();
        break;
      case 'n':
        this.formField.setValue(false);
        this.focusOnItem();
        break;
      default: break;
    }
  }

  switchChange() {
    this.myChange.emit(true);
  }

  private focusOnItem() {
    if (!this.field) { return; } // do nothing if field not found on list
    let tempNextField = this.nextIndex;

    if (this.field.isParent) {
      // go to next field if trigger of child field not satisfied
      if (this.formField.value !== this.itemList[this.nextIndex].triggerToShow && this.itemList.length >= this.nextIndex + 1) {
        tempNextField = tempNextField + 1;
      }
    }

    setTimeout(() => { document.getElementById(this.itemList[tempNextField].fieldName).focus(); }, 1);
  }

}
