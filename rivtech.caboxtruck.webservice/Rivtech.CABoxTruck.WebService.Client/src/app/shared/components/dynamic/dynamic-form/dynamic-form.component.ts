import {Component, EventEmitter, Input, OnInit, Output} from '@angular/core';
import {FormBuilder, FormControl, FormGroup, Validators} from '@angular/forms';
import {QuestionService} from '../../../../core/services/submission/question.service';
import {FieldConfig} from '../../../models/dynamic/field.interface';

@Component({
  selector: 'app-dynamic-form',
  templateUrl: './dynamic-form.component.html',
  styleUrls: ['./dynamic-form.component.scss']
})
export class DynamicFormComponent implements OnInit {
  @Input() fields: FieldConfig[] = [];
  @Input() data: {};
  @Output() parentOnInit: EventEmitter<any> = new EventEmitter<any>();

  constructor(
    protected fb: FormBuilder,
    private questionService: QuestionService
  ) 
  {

  }
  form: FormGroup;

  get value() {
    return this.form.value;
  }

  formMarkAsTouched(): FormGroup {
    this.form.markAllAsTouched();
    return this.form;
  }

  getControlByName(value: string): FormControl {
    return this.form.controls[value] as FormControl;
  }  

  ngOnInit() {    
    this.form = this.createControl();
    // this.populateData();
    this.parentOnInit.emit(this.form);
  }

  createControl(): FormGroup {
    const group = this.fb.group({});

    this.fields.forEach(field => {
      if (field.type === 'button') { return; }

      //add form control to form with validation
      const control = this.fb.control(
        field.value,
        this.bindValidations(field.validations || [])
      );
      group.addControl(field.name, control);

      //set required value for display
      if(field.validations && field.validations.some(x => x.name == 'required')){
        field.required = true;
      }

      //set hide/show of field
      if(field.parentField){   
        let formField = group.get(field.parentField.name);      
        field.isVisible = formField.value == field.parentField.value;

        group.get(field.parentField.name).valueChanges.subscribe(val => {
          field.isVisible = val == field.parentField.value;
          if(field.isVisible){
            this.setAsRequired(group, field.name);
            return;
          }
          this.resetField(group, field.name);
        });
      }
      else
        field.isVisible = true;
    });
    return group;
  }

  populateData() {
    this.form.patchValue(this.data);
  }

  bindValidations(validations: any) {
    if (validations.length > 0) {
      const validList = [];
      validations.forEach(valid => {
        validList.push(valid.validator);
      });
      return Validators.compose(validList);
    }
    return null;
  }

  validateAllFormFields(formGroup: FormGroup) {
    Object.keys(formGroup.controls).forEach(field => {
      const control = formGroup.get(field);
      control.markAsTouched({ onlySelf: true });
    });
  }

  private resetField(form: FormGroup, fieldName){    
    let field = form.controls[fieldName];
    field.reset();
    field.clearValidators();
    field.updateValueAndValidity();    
  }

  private setAsRequired(form: FormGroup, fieldName){  
    let field = form.controls[fieldName];    
    field.setValidators(Validators.required);
    field.updateValueAndValidity();   
  }

}
