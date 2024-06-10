import { Component, Input, OnInit } from '@angular/core';
import { BaseComponent } from '../../../../../../../../../../app/shared/base-component';
import { FormGroup, FormBuilder, FormControl, FormArray } from '@angular/forms';
import { SelectItem } from '../../../../../../../../../shared/models/dynamic/select-item';
import { SafetyDeviceConstants } from '../../../../../../../../../shared/constants/risk-specifics/maintenance-safety.label.constants';
@Component({
  selector: 'app-maintenance-questions-chart',
  templateUrl: './maintenance-questions-chart.component.html',
  styles: [`
    textarea{ width: 100%; }
    .col-l{ width: 26%; }
    .col-m{ width: 13%; }
    .col-s{ width: 9%; }

    .category{
      overflow: auto;
    }

    th {
      background: #f0f3f5;
      border-bottom-width: 1px;
      vertical-align: middle;
      word-break: normal;
  
      &:last-child {
        word-break: break-all;
      }
    }
    
    tr:nth-child(even) {
      background-color: #f7f7f7;
    }`
  ]
})
export class MaintenanceQuestionsChartComponent extends BaseComponent implements OnInit {
  get fn() { return 'safetyDevicesForm'; }

  constructor(
    private fb: FormBuilder
  ) {
    super();
  }
  @Input() safetyDeviceCategoryList: SelectItem[] = [];
  @Input() formArray: FormArray;

  public SafetyDeviceConstants = SafetyDeviceConstants;
  fg(index: number) { return this.formArray.controls[index] as FormGroup; }
  fc(index: number) { return this.fg(index).controls; }

  ngOnInit() {
    this.formArray.controls.sort(this.sorter('categoryLabel', 'desc'));
  }

  categoryLabel(id: string): string {
    return this.safetyDeviceCategoryList.find(s => s.value === id).label;
  }

  sort(targetElement) {
    const order = targetElement.getAttribute('data-order');
    const type = targetElement.getAttribute('data-type');
    const field = targetElement.getAttribute('data-name');

    this.formArray.controls.sort(this.sorter(field, order));
    targetElement.setAttribute('data-order', order === 'desc' ? 'asc' : 'desc');

  }
  
  private sorter(property: string, order: string) {
    return (a: any, b: any) => {
      return order === "asc" ?
        a.controls[property].value > b.controls[property].value ? -1 : 1
        : a.controls[property].value < b.controls[property].value ? -1 : 1;
    }
  }

}
