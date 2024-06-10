import { Component, OnInit } from '@angular/core';
import {DashboardData} from '../../../data/dashboard.data';
import {Router} from '@angular/router';

@Component({
  selector: 'app-policy-section',
  templateUrl: './policy-section.component.html',
  styleUrls: ['./policy-section.component.scss']
})
export class PolicySectionComponent implements OnInit {

  constructor(public dashboardData: DashboardData,
              private router: Router) { }

  ngOnInit(): void { }

  onClick(status: string) {
    this.router.navigate(['/submissions/list'], {queryParams: { status: status} });
  }
}
