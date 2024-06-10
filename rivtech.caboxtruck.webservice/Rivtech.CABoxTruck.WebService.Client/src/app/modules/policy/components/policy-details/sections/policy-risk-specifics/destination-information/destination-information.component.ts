import { Component, OnInit, ViewChild } from '@angular/core';
import { ToggleCollapseComponent } from '../../../../../../../shared/components/toggle-collapse/toggle-collapse.component';
import { ClickTypes } from '../../../../../../../shared/constants/click-type.constants';
import { NavigationService } from '@app/core/services/navigation/navigation.service';
import { PathConstants } from '@app/shared/constants/path.constants';

@Component({
  selector: 'app-destination-information',
  templateUrl: './destination-information.component.html',
  styleUrls: ['./destination-information.component.scss']
})
export class DestinationInformationComponent implements OnInit {

  @ViewChild(ToggleCollapseComponent) toggleCollapse: ToggleCollapseComponent;


  mexicoDestination: boolean = false;
  nyc5Boroughs: boolean = false;
  constructor(private navigationService: NavigationService) { }

  ngOnInit() {
  }

  public onClick(clickType?) {
    switch (clickType) {
      case ClickTypes.Back:
        this.navigationService.navigatePolicyRoute(PathConstants.Submission.Claims.Index);
        break;
      case ClickTypes.Next:
        this.navigationService.navigatePolicyRoute(`${PathConstants.Submission.RiskSpecifics.Index}/${PathConstants.Submission.RiskSpecifics.DriverInfo}`);
        break;
    }
  }

  updateMexicoNyc5(states: string[]) {
    this.mexicoDestination = states.some(s => ['NM', 'TX', 'CA', 'AZ', 'LA'].includes(s));
    this.nyc5Boroughs = states.some(s => ['NJ', 'NY', 'CT', 'PA'].includes(s));
  }
}
