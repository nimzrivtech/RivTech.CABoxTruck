import { Component, OnInit, ViewChild } from '@angular/core';
import { Location } from '@angular/common';
import { Router } from '@angular/router';
import { ClickTypes } from '../../../../../../../shared/constants/click-type.constants';
import { NextBackButtonComponent } from '../../../submission-details-shared/components/next-back-button/next-back-button.component';
import { MenuOverrideService } from '../../../../../../../core/services/layout/menu-override.service';
import { ApplicantData } from '../../../../../../../modules/submission/data/applicant/applicant.data';

@Component({
  selector: 'app-additional-interest',
  templateUrl: './additional-interest.component.html',
  styleUrls: ['./additional-interest.component.scss']
})
export class AdditionalInterestComponent implements OnInit {

  @ViewChild(NextBackButtonComponent) nextBack: NextBackButtonComponent;
  isScrollPage: Boolean = true;
  constructor(private location: Location,
    private router: Router,
    private menuOverrideService: MenuOverrideService,
    public applicantData: ApplicantData) { }

  ngOnInit() {
  }

  public onClick(clickType?: any) {
    this.nextBack.location = this.location;
    switch (clickType) {
      case ClickTypes.Back:
        this.router.navigate([this.nextBack.getPrev()]);
        break;
      case ClickTypes.Next:
        this.router.navigate([this.nextBack.getNext()]);
        break;
    }
  }
}
