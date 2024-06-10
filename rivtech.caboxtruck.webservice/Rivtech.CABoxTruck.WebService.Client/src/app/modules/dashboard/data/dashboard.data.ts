import {Injectable} from '@angular/core';
import {DashboardDTO} from '@app/shared/models/dashboard/dashboard';
import { DashboardService} from '../services/dashboard.service';

@Injectable()
export class DashboardData {
  constructor(private dashboardService: DashboardService) {
  }

  public dashboardCount: DashboardDTO = new DashboardDTO();

  initializeFields() {
    this.dashboardService.getDashboardCount().pipe().take(1).subscribe(data => {
      this.dashboardCount = data;
    });
  }
}
