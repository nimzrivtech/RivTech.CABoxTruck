import { Component, OnInit, Input, AfterViewInit } from '@angular/core';
import { navItems, NavData } from '../../../_nav';
import { LayoutService } from '../../../core/services/layout/layout.service';
import { NavigationService } from '../../../core/services/navigation/navigation.service';
import { createSubmissionMenuItems } from '../../../modules/submission/pages/submission/submission-navitems';

@Component({
  selector: 'app-left-sidebar',
  templateUrl: './left-sidebar.component.html',
  styleUrls: ['./left-sidebar.component.css']
})
export class LeftSidebarComponent implements OnInit, AfterViewInit {

  @Input() visible: boolean = false;

  public sidebarMinimized = false;

  toggleMinimize(e) {
    this.sidebarMinimized = e;
  }

  constructor(public layoutService: LayoutService,
    public navService: NavigationService) {
  }

  ngOnInit(): void {
  }

  ngAfterViewInit(): void {
    var navLink = document.querySelectorAll(".nav-link");
    var navLinkListeners = [];
    var menuDropdownListeners = document.querySelectorAll(".nav-link.nav-dropdown-toggle");
    var menuDropdownOpen = document.querySelectorAll(".nav-item.nav-dropdown.open");

    menuDropdownListeners.forEach(dropdown => {
      dropdown.addEventListener('click', function () {
        menuDropdownOpen = document.querySelectorAll(".nav-item.nav-dropdown.open");
      });
    })

    navLink.forEach(link => {
      if (link.classList.length == 1 || link.classList.contains("active"))
        navLinkListeners.push(link);
    });

    navLinkListeners.forEach(nav => {
      nav.addEventListener('click', function () {
        menuDropdownOpen.forEach(dropdown => {
          setTimeout(function(){
            if (!dropdown.classList.contains("open"))
            dropdown.classList.add("open");
          }, 215); // to wait for coreui js to execute first
        });
      });
    });
  }

}
