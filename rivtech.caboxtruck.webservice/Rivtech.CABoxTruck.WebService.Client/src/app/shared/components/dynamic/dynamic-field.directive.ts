import {ComponentFactoryResolver, Directive, Input, OnInit, ViewContainerRef} from '@angular/core';
import {InputComponent} from './input/input.component';
import {CheckboxComponent} from './checkbox/checkbox.component';
import {FieldConfig} from '../../models/dynamic/field.interface';
import {FormGroup} from '@angular/forms';
import {DateComponent} from './date/date.component';
import {SelectComponent} from './select/select.component';
import { RadiobuttonComponent } from './radiobutton/radiobutton.component';
import { TextAreaComponent } from './text-area/text-area.component';
import { MultipleSelectComponent } from './multiselect/multiselect.component';

@Directive({
  selector: '[appDynamicField]'
})
export class DynamicFieldDirective implements OnInit {

  @Input() field: FieldConfig;
  @Input() group: FormGroup;

  componentRef: any;

  constructor(private resolver: ComponentFactoryResolver,
              private container: ViewContainerRef) { }

  componentMapper = {
    input: InputComponent,
    select: SelectComponent,
    checkbox: CheckboxComponent,
    date: DateComponent,
    radiobutton: RadiobuttonComponent,
    textarea: TextAreaComponent,
    multiselect: MultipleSelectComponent

    /*
    radiobutton: RadiobuttonComponent,
    button: ButtonComponent,
     */
  };

  ngOnInit() {
    const factory = this.resolver.resolveComponentFactory(
      (this.componentMapper)[this.field.type]
    );
    this.componentRef = this.container.createComponent(factory);
    this.componentRef.instance.field = this.field;
    this.componentRef.instance.group = this.group;
  }
}
