import { Component, OnInit, ViewChild } from '@angular/core';
import { Router } from '@angular/router';
import { MenuOverrideService } from '../../../../../../../core/services/layout/menu-override.service';
import { ToggleCollapseComponent } from '../../../../../../../shared/components/toggle-collapse/toggle-collapse.component';
import { ClickTypes } from '../../../../../../../shared/constants/click-type.constants';
import { Location } from '@angular/common';
import { RadiusOfOperationsData } from '../../../../../data/riskspecifics/radius-of-operations.data';
import { DestinationInfoValidationService } from '../../../../../../../core/services/validations/destination-info-validation.service';
import { SubmissionNavValidateService } from '../../../../../../../core/services/navigation/submission-nav-validate.service';
import { NextBackButtonComponent } from '../../../submission-details-shared/components/next-back-button/next-back-button.component';

@Component({
  selector: 'app-destination-information',
  templateUrl: './destination-information.component.html',
  styleUrls: ['./destination-information.component.scss']
})
export class DestinationInformationComponent implements OnInit {

  @ViewChild(ToggleCollapseComponent) toggleCollapse: ToggleCollapseComponent;
  @ViewChild(NextBackButtonComponent) nextBack: NextBackButtonComponent;


  mexicoDestination: boolean = false;
  nyc5Boroughs: boolean = false;
  constructor(private router: Router,
    private menuOverrideService: MenuOverrideService,
    private location: Location,
    public radiusOfOperationsData: RadiusOfOperationsData,
    public destinationValidation: DestinationInfoValidationService,
    public formValidation: SubmissionNavValidateService,
  ) { }

  ngOnInit() {
  }

  public onClick(clickType?: any) {
    this.nextBack.location = this.location;
    switch (clickType) {
      case ClickTypes.Back:
        this.router.navigate([this.nextBack.getPrev()]);
        break;
      case ClickTypes.Next:
        this.menuOverrideService.overrideSideBarBehavior();
        this.radiusOfOperationsData.radiusOfOperationsForm.markAllAsTouched();
        this.router.navigate([this.nextBack.getNext()]);
        break;
    }
  }

  updateMexicoNyc5(states: string[]) {
    this.mexicoDestination = states.some(s => ['NM', 'TX', 'CA', 'AZ', 'LA'].includes(s));
    this.nyc5Boroughs = states.some(s => ['NJ', 'NY', 'CT', 'PA'].includes(s));
  }
}
