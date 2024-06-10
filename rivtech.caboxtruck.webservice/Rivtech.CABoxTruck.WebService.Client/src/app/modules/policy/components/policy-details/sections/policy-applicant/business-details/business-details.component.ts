import { Component, OnInit, ViewChild } from '@angular/core';
import { ApplicantData } from '../../../../../../../modules/submission/data/applicant/applicant.data';
import { ClickTypes } from '../../../../../../../shared/constants/click-type.constants';
import { PathConstants } from '../../../../../../../shared/constants/path.constants';
import { NavigationService } from '@app/core/services/navigation/navigation.service';

@Component({
  selector: 'app-business-details',
  templateUrl: './business-details.component.html',
  styleUrls: ['./business-details.component.scss']
})
export class BusinessDetailsComponent implements OnInit {

  constructor(private navigationService: NavigationService, private applicantData: ApplicantData) { }

  ngOnInit() {
  }

  public onClick(clickType?) {
    switch (clickType) {
      case ClickTypes.Back:
        this.navigationService.navigatePolicyRoute(`${PathConstants.Submission.Applicant.Index}/${PathConstants.Submission.Applicant.NameAndAddress}`);
        break;
      case ClickTypes.Next:
        this.navigationService.navigatePolicyRoute(`${PathConstants.Submission.Applicant.Index}/${PathConstants.Submission.Applicant.FilingsInformation}`);
        break;
    }
  }
}
