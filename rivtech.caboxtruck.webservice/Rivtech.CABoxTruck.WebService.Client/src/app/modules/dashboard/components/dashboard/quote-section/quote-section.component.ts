import { Component, OnInit } from '@angular/core';
import {Router} from '@angular/router';
import {DashboardData} from '../../../data/dashboard.data';

@Component({
  selector: 'app-quote-section',
  templateUrl: './quote-section.component.html',
  styleUrls: ['./quote-section.component.scss']
})
export class QuoteSectionComponent implements OnInit {

  constructor(
    private router: Router,
    public dashboardData: DashboardData
  ) { }


  ngOnInit() { }

  onClick(status: string) {
    this.router.navigate(['/submissions/list'], {queryParams: { status: status, dashboard: true} });
  }

}
