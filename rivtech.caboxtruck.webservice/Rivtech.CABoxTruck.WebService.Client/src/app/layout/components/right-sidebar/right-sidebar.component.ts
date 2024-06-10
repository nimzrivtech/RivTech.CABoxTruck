import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { NavigationService } from '../../../core/services/navigation/navigation.service';
import { AuthService } from '../../../core/services/auth.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-right-sidebar',
  templateUrl: './right-sidebar.component.html',
  styleUrls: ['./right-sidebar.component.css']
})
export class RightSidebarComponent implements OnInit {

  constructor(public navService: NavigationService, private authService: AuthService, private router: Router) { }

  ngOnInit() {
    this.toggleHideMainSidebar();
  }


  toggleHideMainSidebar() {
    let body = document.getElementsByTagName('body')[0];
    body.classList.remove("aside-menu-show");
  }

  logout() {
    this.authService.logout();
    this.router.navigate(['login']);
  }
}
