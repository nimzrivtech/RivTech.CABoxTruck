import { Component, OnInit, ViewChild } from '@angular/core';
import { ToggleCollapseComponent } from '../../../../../../../shared/components/toggle-collapse/toggle-collapse.component';
import { ClickTypes } from '../../../../../../../shared/constants/click-type.constants';
import { NavigationService } from '@app/core/services/navigation/navigation.service';
import { PathConstants } from '@app/shared/constants/path.constants';
import { DriverInfoData } from '@app/modules/submission/data/riskspecifics/driver-info.data';

@Component({
  selector: 'app-driver-information',
  templateUrl: './driver-information.component.html',
  styleUrls: ['./driver-information.component.scss']
})
export class DriverInformationComponent implements OnInit {
  @ViewChild(ToggleCollapseComponent) toggleCollapse: ToggleCollapseComponent;

  constructor(private navigationService: NavigationService,
              public driverInfoData: DriverInfoData) { }

  ngOnInit() {
  }

  public onClick(clickType?) {
    switch (clickType) {
      case ClickTypes.Back:
        this.navigationService.navigatePolicyRoute(`${PathConstants.Submission.RiskSpecifics.Index}/${PathConstants.Submission.RiskSpecifics.DestinationInfo}`);
        break;
      case ClickTypes.Next:
        this.navigationService.navigatePolicyRoute(`${PathConstants.Submission.RiskSpecifics.Index}/${PathConstants.Submission.RiskSpecifics.DotInfo}`);
        break;
    }
  }
}
