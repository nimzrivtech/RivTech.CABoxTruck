import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router'

@Component({
  selector: 'app-key-management-personnel-info-section',
  templateUrl: './key-management-personnel-info-section.component.html',
  styleUrls: ['./key-management-personnel-info-section.component.scss']
})
export class KeyManagementPersonnelInfoSectionComponent implements OnInit {
  personelInfo: FormGroup;
  submitted = false;

  constructor(private formBuilder: FormBuilder, public router: Router) { }

  ngOnInit() {
    this.personelInfo = this.formBuilder.group({
      businessPrincipalInput: ['', Validators.required],
      personelEmailInput: ['', [Validators.required, Validators.email]],
      personelPhoneExtensionInput: ['', Validators.required],
      pcofNameInput:[''],
      pcofPhoneExtensionInput:[''],
      pcofYearsInput:[''],
      omNameInput:[''],
      omPhoneExtensionInput:[''],
      omYearsInput:[''],
      sdNameInput:[''],
      sdPhoneExtensionInput:[''],
      sdYearsInput:[''],
      mdNameInput:[''],
      mdPhoneExtensionInput:[''],
      mdYearsInput:['']
    });
  }

  // convenience getter for easy access to form fields
  get f() { return this.personelInfo.controls; }

  onSubmit() {
      this.submitted = true;

      // stop here if form is invalid
      // if (this.personelInfo.invalid) {
      //     return;
      // }
      this.router.navigate(['/submissions/new/applicant/subsidaries-affiliated']);
      // display form values on success
  }

  onReset() {
      this.submitted = false;
      this.personelInfo.reset();
  }

}
