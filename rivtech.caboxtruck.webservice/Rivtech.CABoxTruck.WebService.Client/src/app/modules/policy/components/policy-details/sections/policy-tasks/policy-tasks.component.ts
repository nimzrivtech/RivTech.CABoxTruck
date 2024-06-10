import { Component, OnInit, ViewChild } from '@angular/core';
import { NavigationService } from '../../../../../../core/services/navigation/navigation.service';
import { ToggleCollapseComponent } from '../../../../../../shared/components/toggle-collapse/toggle-collapse.component';
import { ClickTypes } from '../../../../../../shared/constants/click-type.constants';
import { PathConstants } from '../../../../../../shared/constants/path.constants';

@Component({
  selector: 'app-policy-tasks',
  templateUrl: './policy-tasks.component.html',
  styleUrls: ['./policy-tasks.component.css']
})
export class PolicyTasksComponent implements OnInit {
  @ViewChild(ToggleCollapseComponent) toggleCollapse: ToggleCollapseComponent;
  hideMe: boolean = false;
  disableItems = {
    updateIcon: true,
    deleteIcon: true,
  };

  constructor(private navigationService: NavigationService
    ) { }

  ngOnInit(): void {
  }

  public ToggleHiding(): void {
    this.hideMe = !this.hideMe;
  }

  public onClick(clickType?) {
    switch (clickType) {
      case ClickTypes.Back:
        this.navigationService.navigatePolicyRoute(PathConstants.Policy.Notes);
        break;
      case ClickTypes.Next:
        this.navigationService.navigatePolicyRoute(PathConstants.Policy.Tasks);
        break;
    }
  }

}
