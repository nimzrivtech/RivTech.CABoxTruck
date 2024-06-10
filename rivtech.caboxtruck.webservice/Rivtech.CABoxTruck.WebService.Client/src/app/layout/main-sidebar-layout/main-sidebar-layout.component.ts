import { Component, OnInit, Input } from '@angular/core';
import { NavData } from '../../_nav';
import { createSubmissionMenuItems } from '../../modules/submission/pages/submission/submission-navitems';

@Component({
  selector: 'app-main-sidebar-layout',
  templateUrl: './main-sidebar-layout.component.html',
  styleUrls: ['./main-sidebar-layout.component.scss']
})
export class MainSidebarLayoutComponent implements OnInit {

  @Input() navItems: NavData[];

  sidebarVisible: boolean = false;

  constructor() { }

  ngOnInit() {
    setTimeout(() => { this.sidebarVisible = true; });
    this.navItems = createSubmissionMenuItems(1);
    //console.log('navItems count' + this.navItems.length);
  }

}
