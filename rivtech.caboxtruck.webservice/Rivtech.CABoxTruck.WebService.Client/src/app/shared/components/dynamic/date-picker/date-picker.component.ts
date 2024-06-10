import { Component, forwardRef, Input, Output, ViewChild, EventEmitter, AfterViewChecked, ChangeDetectorRef } from '@angular/core';
import { ControlValueAccessor, FormControl, NG_VALUE_ACCESSOR } from '@angular/forms';
import { ErrorMessageConstant } from '../../../../shared/constants/error-message.constants';
import { AngularMyDatePickerDirective, IAngularMyDpOptions, IMyDateModel, IMyInputFieldChanged } from 'angular-mydatepicker';

@Component({
  selector: 'app-date-picker',
  templateUrl: './date-picker.component.html',
  styleUrls: ['./date-picker.component.scss'],
  providers: [
    {
      provide: NG_VALUE_ACCESSOR,
      useExisting: forwardRef(() => DatePickerComponent),
      multi: true
    }
  ]
})
/**
   * Reference: angular-mydatepicker
   * Date Options should be an object
   * Example:
   *  option = {
        dateRange: false,
        dateFormat: 'mm/yyyy' -> your custom date format
      };
   *
   */
export class DatePickerComponent implements ControlValueAccessor, AfterViewChecked {
  public ErrorMessageConstant = ErrorMessageConstant;
  @ViewChild('dp') myDp: AngularMyDatePickerDirective;
  @ViewChild('val') val: FormControl;

  @Input() options: IAngularMyDpOptions;
  @Input() isInvalid: boolean;
  @Input() isTouched: boolean;
  @Output() dateChange = new EventEmitter();
  @Output() inputFieldChanged = new EventEmitter();
  @Input() customErrorMessage: string;
  @Input() customRequiredErrorMessage: string;
  @Input() isDisabled: boolean = false;
  @Input() hasMarginRight: boolean = false;
  @Input() isMMYYYFormat: boolean = false;

  @Input() customId: string = null;

  innerValue: any = '';
  private onTouchedCallback: () => void = () => { };
  private onChangeCallback: (_: any) => void = () => { };

  constructor(private cdr: ChangeDetectorRef) { }
  ngAfterViewChecked() {
    this.cdr.detectChanges();
  }

  get value(): any {
    return this.innerValue;
  }
  set value(newValue: any) {
    if (newValue === this.innerValue) {
      return;
    }
    this.innerValue = newValue;
    this.onChangeCallback(newValue);
  }
  onBlur() {
    this.onTouchedCallback();
  }
  writeValue(val: any): void {
    if (val !== this.innerValue) {
      this.innerValue = val;
    }
  }
  registerOnChange(fn: any): void {
    this.onChangeCallback = fn;
  }
  registerOnTouched(fn: any): void {
    this.onTouchedCallback = fn;
  }

  toggleCalendar(): void {
    this.myDp.toggleCalendar();
  }

  onDateChanged(event: IMyDateModel) {
    this.dateChange.emit(event.singleDate);
  }

  onInputFieldChanged(event: IMyInputFieldChanged) {
    this.inputFieldChanged.emit(event);
  }

}
