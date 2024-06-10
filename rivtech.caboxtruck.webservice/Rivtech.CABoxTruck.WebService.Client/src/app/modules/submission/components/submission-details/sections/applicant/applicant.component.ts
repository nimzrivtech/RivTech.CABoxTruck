import { Component, OnInit } from '@angular/core';
import { ApplicantData } from '../../../../../../modules/submission/data/applicant/applicant.data';

@Component({
  selector: 'app-applicant',
  templateUrl: './applicant.component.html',
  styleUrls: ['./applicant.component.scss']
})
export class ApplicantComponent implements OnInit {

  constructor(
    public applicantData: ApplicantData
  ) { }

  ngOnInit() {
  }

}
