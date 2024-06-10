import { Component, OnInit, ViewChild } from '@angular/core';
import { VehicleData } from '../../../../data/vehicle/vehicle.data';

import { ClickTypes } from '../../../../../../shared/constants/click-type.constants';
import { MenuOverrideService } from '../../../../../../core/services/layout/menu-override.service';
import { NavigationEnd, Router } from '@angular/router';
import { NextBackButtonComponent } from '../../submission-details-shared/components/next-back-button/next-back-button.component';
import { Location } from '@angular/common';
import { VehicleDto } from '../../../../../../shared/models/submission/Vehicle/VehicleDto';
import { SubmissionData } from '../../../../../../modules/submission/data/submission.data';
import { VehicleValidationService } from '../../../../../../core/services/validations/vehicle-validation.service';
import { SubmissionNavValidateService } from '../../../../../../core/services/navigation/submission-nav-validate.service';
import { LimitsData } from '@app/modules/submission/data/limits/limits.data';
import { Subject } from 'rxjs-compat';
import { catchError, finalize, map, switchMap, takeUntil, tap } from 'rxjs/operators';
import * as _ from 'lodash';

@Component({
  selector: 'app-vehicle',
  templateUrl: './vehicle.component.html',
  styleUrls: ['./vehicle.component.scss']
})
export class VehicleComponent implements OnInit {

  private reset$ = new Subject();
  private newCategoryRoute: string;
  private currentCategoryRoute: string;

  constructor(
    public vehicleData: VehicleData,
    private router: Router,
    private menuOverrideService: MenuOverrideService,
    private submissionData: SubmissionData,
    private limitsData: LimitsData,
    private location: Location,
    public vehicleValidation: VehicleValidationService,
    public formValidation: SubmissionNavValidateService,
  ) { }

  @ViewChild(NextBackButtonComponent) nextBack: NextBackButtonComponent;

  ngOnInit() {
    this.vehicleData.initiateFormGroup(new VehicleDto());
    this.limitsData.isVehicle = true;

    this.currentCategoryRoute = this.getCurrentCategoryRoute();
    this.savingUponNavigate();
  }

  get isScroll() {
    return (this.submissionData.riskDetail.vehicles != null && this.submissionData.riskDetail.vehicles.length > 4) || null;
  }

  get isPageValid() {
    return this.submissionData.riskDetail?.vehicles?.length > 0 
    && !this.submissionData.riskDetail?.vehicles?.find(x => !x.isValidated);
  }

  public onClick(clickType?) {
    //this handle new or with ID //need more enhancement
    this.nextBack.location = this.location;
    this.limitsData.isVehicle = false;
    let url = "" ;
    switch (clickType) {
      case ClickTypes.Back:
        url = this.nextBack.getPrev();
        this.router.navigate([url]);
        break;
      case ClickTypes.Next:
        this.menuOverrideService.overrideSideBarBehavior();
        url = this.nextBack.getNext();
        this.router.navigate([url]);
        break;
    }
  }

  savingUponNavigate() {
    this.router.events
      .filter(event => event instanceof NavigationEnd)
      .pipe(takeUntil(this.reset$)).subscribe((event: NavigationEnd) => {
        this.newCategoryRoute = this.getCurrentCategoryRoute();
        console.log(this.newCategoryRoute);
        this.limitsData.isVehicle = false;
        this.currentCategoryRoute = this.newCategoryRoute;
      });
  }

  private getCurrentCategoryRoute(): string {
    const splitUrl = _.split(this.router.url, '/');
    const currentCategoryIndex = splitUrl.length - 1;
    return splitUrl[currentCategoryIndex];
  }
}
