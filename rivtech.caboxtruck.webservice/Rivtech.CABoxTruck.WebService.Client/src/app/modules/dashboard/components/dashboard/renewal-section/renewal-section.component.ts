import { Component, OnInit } from '@angular/core';
import {DashboardData} from '../../../data/dashboard.data';
import {Router} from '@angular/router';

@Component({
  selector: 'app-renewal-section',
  templateUrl: './renewal-section.component.html',
  styleUrls: ['./renewal-section.component.scss']
})
export class RenewalSectionComponent implements OnInit {

  constructor(public dashboardData: DashboardData,
              private router: Router) { }

  ngOnInit(): void { }

  onClick(status: string) {
    this.router.navigate(['/submissions/list'], {queryParams: { status: status, renewal: true} });
  }
}
