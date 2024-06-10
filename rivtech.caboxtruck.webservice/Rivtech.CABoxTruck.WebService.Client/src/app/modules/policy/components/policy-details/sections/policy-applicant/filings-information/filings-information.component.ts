import { Component, OnInit, ViewChild } from '@angular/core';
import { ClickTypes } from '../../../../../../../shared/constants/click-type.constants';
import { FilingsInformationSectionComponent } from './filings-information-section/filings-information-section.component';
import { ApplicantData } from '../../../../../../../modules/submission/data/applicant/applicant.data';
import { NavigationService } from '@app/core/services/navigation/navigation.service';
import { PathConstants } from '@app/shared/constants/path.constants';

@Component({
  selector: 'app-filings-information',
  templateUrl: './filings-information.component.html',
  styleUrls: ['./filings-information.component.scss']
})

export class FilingsInformationComponent implements OnInit {

  @ViewChild(FilingsInformationSectionComponent) filingsInformation: FilingsInformationSectionComponent;

  constructor(private navigationService: NavigationService, private applicantData: ApplicantData) { }

  ngOnInit() {
  }

  public onClick(clickType?) {
    switch (clickType) {
      case ClickTypes.Back:
        this.navigationService.navigatePolicyRoute(`${PathConstants.Submission.Applicant.Index}/${PathConstants.Submission.Applicant.BusinessDetails}`);
        break;
      case ClickTypes.Next:
        this.navigationService.navigatePolicyRoute(PathConstants.Policy.AdditionalInterest);
        break;
    }
  }
}
