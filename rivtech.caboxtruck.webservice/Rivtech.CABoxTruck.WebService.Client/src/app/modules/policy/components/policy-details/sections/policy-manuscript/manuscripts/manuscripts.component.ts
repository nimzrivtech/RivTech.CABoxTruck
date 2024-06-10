import { Component, OnInit, ViewChild } from '@angular/core';
import { ClickTypes } from '../../../../../../../shared/constants/click-type.constants';
import { NavigationService } from '@app/core/services/navigation/navigation.service';
import { PathConstants } from '@app/shared/constants/path.constants';

@Component({
  selector: 'app-manuscripts',
  templateUrl: './manuscripts.component.html',
  styleUrls: ['./manuscripts.component.scss']
})
export class ManuscriptsComponent implements OnInit {

  isScrollPage: Boolean = true;
  constructor(private navigationService: NavigationService) { }

  ngOnInit() {
  }

  public onClick(clickType?) {
    switch (clickType) {
      case ClickTypes.Back:
        this.navigationService.navigatePolicyRoute(PathConstants.Policy.AdditionalInterest);
        break;
      case ClickTypes.Next:
        this.navigationService.navigatePolicyRoute(PathConstants.Submission.Broker.Index);
        break;
    }
  }
}
