import { Component, OnInit, ViewChild } from '@angular/core';
import { ActivatedRoute } from '@angular/router';


import { ClickTypes } from '../../../../../../shared/constants/click-type.constants';
import { MenuOverrideService } from '../../../../../../core/services/layout/menu-override.service';
import { Router } from '@angular/router';
import { Location } from '@angular/common';
import { NextBackButtonComponent } from '../../submission-details-shared/components/next-back-button/next-back-button.component';

@Component({
  selector: 'app-underwriting',
  templateUrl: './underwriting.component.html',
  styleUrls: ['./underwriting.component.scss']
})
export class UnderWritingComponent implements OnInit {

  constructor(private route: ActivatedRoute,
    private router: Router,
    private menuOverrideService: MenuOverrideService,
    private location: Location
    ) { }
  @ViewChild(NextBackButtonComponent) nextBack : NextBackButtonComponent;

  ngOnInit() {
  }

  public onClick(clickType?) {
    //this handle new or with ID //need more enhancement
    this.nextBack.location = this.location;
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
}
