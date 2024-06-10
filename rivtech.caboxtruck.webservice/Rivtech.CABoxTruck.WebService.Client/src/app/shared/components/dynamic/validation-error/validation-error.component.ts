import { Component, Input, OnInit, ViewEncapsulation } from '@angular/core';
import { FormControl } from '@angular/forms';

@Component({
  selector: 'app-validation-error',
  templateUrl: './validation-error.component.html',
  styleUrls: ['./validation-error.component.scss'],
  host: {
    'class': 'invalid-feedback'
  }

})
export class ValidationErrorComponent implements OnInit {

  @Input() control: FormControl;
  @Input() inputName: string;

  ERROR_MESSAGES = {
    required: () => this.inputName? `${this.inputName} is required` : 'This field is required',
    minlength: error => this.inputName? `${this.inputName} should be at least ${error.requiredLength} characters` : `Input should be at least ${error.requiredLength} characters`,
    maxlength: error => this.inputName? `${this.inputName} should be at most ${error.requiredLength} characters` : `Input should be at most ${error.requiredLength} characters`,
    email: () => `Please enter a valid email`,
    pattern: () => 'Please enter a valid email',
    min: () => this.inputName ? `${this.inputName} is required` : 'This field is required'
  };

  constructor() {}

  ngOnInit() {}

  shouldShowErrors(): boolean {
    return this.control && this.control.errors && (this.control.touched || this.control.dirty);
  }

  listOfErrors(): string[] {
    return Object.keys(this.control.errors).map(
      error =>
      {
        if(error in this.ERROR_MESSAGES)
          return this.ERROR_MESSAGES[error](this.control.getError(error))
        else
          return "";
      }
    );
  }

}
