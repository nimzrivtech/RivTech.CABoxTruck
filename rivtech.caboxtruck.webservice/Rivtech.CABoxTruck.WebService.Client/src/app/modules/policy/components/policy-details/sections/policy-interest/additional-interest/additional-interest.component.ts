import { Component, OnInit, ViewChild } from '@angular/core';
import { ClickTypes } from '../../../../../../../shared/constants/click-type.constants';
import { PathConstants } from '../../../../../../../shared/constants/path.constants';
import { NavigationService } from '../../../../../../../core/services/navigation/navigation.service';

@Component({
  selector: 'app-additional-interest',
  templateUrl: './additional-interest.component.html',
  styleUrls: ['./additional-interest.component.scss']
})
export class AdditionalInterestComponent implements OnInit {

  isScrollPage: Boolean = true;
  constructor(private navigationService: NavigationService) { }

  ngOnInit() {
  }

  public onClick(clickType?) {
    switch (clickType) {
      case ClickTypes.Back:
        this.navigationService.navigatePolicyRoute(`${PathConstants.Submission.Applicant.Index}/${PathConstants.Submission.Applicant.FilingsInformation}`);
        break;
      case ClickTypes.Next:
        this.navigationService.navigatePolicyRoute(PathConstants.Policy.Manuscript);
        break;
    }
  }
}
