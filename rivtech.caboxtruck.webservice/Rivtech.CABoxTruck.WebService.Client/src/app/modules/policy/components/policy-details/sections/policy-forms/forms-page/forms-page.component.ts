import { Component, OnInit } from '@angular/core';
import { ClickTypes } from '../../../../../../../shared/constants/click-type.constants';
import { NavigationService } from '../../../../../../../core/services/navigation/navigation.service';
import { PathConstants } from '../../../../../../../shared/constants/path.constants';

@Component({
  selector: 'app-forms-page',
  templateUrl: './forms-page.component.html',
  styleUrls: ['./forms-page.component.scss']
})
export class FormsPageComponent implements OnInit {

  constructor(
    private navigationService: NavigationService
  ) { }

  ngOnInit() {
  }

  public onClick(clickType?) {
    switch (clickType) {
      case ClickTypes.Back:
        this.navigationService.navigatePolicyRoute(PathConstants.Submission.Bind.Index);
        break;
      case ClickTypes.Next:
        this.navigationService.navigatePolicyRoute(PathConstants.Policy.Rating);
        break;
    }
  }
}
