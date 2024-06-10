import { Component, OnDestroy, OnInit } from '@angular/core';
import { LayoutService } from '../../../../core/services/layout/layout.service';
import { BsModalRef, BsModalService } from 'ngx-bootstrap';
import { IBanner } from '../../../../shared/models/dashboard.banner.model';
import { BaseClass } from '../../../../shared/base-class';
import { DashBoardService } from '../../../../core/services/dashboard.service';
import { takeUntil } from 'rxjs/operators';
import { ProcessEndorsementModalComponent } from '../../components/dashboard/process-endorsement-modal/process-endorsement-modal.component';
import {Router} from '@angular/router';

@Component({
  selector: 'app-agent-dashboard',
  templateUrl: './agent-dashboard.component.html',
  styleUrls: ['./agent-dashboard.component.scss']
})
export class AgentDashboardComponent extends BaseClass implements OnInit, OnDestroy {
  public banners: IBanner[];
  modalRef: BsModalRef | null;

  constructor(
    private layoutService: LayoutService,
    private modalService: BsModalService,
    private dashBoardService: DashBoardService,
    private router: Router
  ) {
    super();
  }

  ngOnInit() {
    this.dashBoardService.getBanners().pipe(takeUntil(this.stop$)).subscribe(response => {
      const convertedResponse = response as IBanner[];
      this.banners = convertedResponse;
    });
  }

  showProcessEndorsementModal(): void {
    const initialState = {
      title: 'Process Endorsement'
    };
    this.modalRef = this.modalService.show(ProcessEndorsementModalComponent, {
      initialState,
      backdrop: true,
      ignoreBackdropClick: true,
    });
  }

  onSearchAnyKeyword(keyword: string): void {
    this.router.navigate(['/submissions/list'], {queryParams: { q: keyword} });
  }
}
