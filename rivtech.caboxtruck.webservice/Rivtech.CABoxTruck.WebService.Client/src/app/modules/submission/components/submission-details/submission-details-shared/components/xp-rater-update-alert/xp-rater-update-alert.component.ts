import { Component, OnDestroy, OnInit } from '@angular/core';
import { RaterApiData, RaterVersion } from '@app/modules/submission/data/rater-api.data';
import { ToastrService } from 'ngx-toastr';
import { Subject } from 'rxjs';
import { takeUntil, tap } from 'rxjs/operators';

@Component({
  selector: 'app-xp-rater-update-alert',
  templateUrl: './xp-rater-update-alert.component.html',
  styleUrls: ['./xp-rater-update-alert.component.scss']
})
export class XpRaterUpdateAlertComponent implements OnInit, OnDestroy {
  private readonly destroy$ = new Subject();

  raterVersion: RaterVersion = null;

  constructor(
    private raterApiData: RaterApiData,
    private toastr: ToastrService
  ) { }

  ngOnInit() {
    this.raterApiData.raterVersion.pipe(
      takeUntil(this.destroy$),
      tap(data => console.log(data))
    ).subscribe(raterVersion => this.raterVersion = raterVersion);

    this.raterApiData.loadRaterVersion$().subscribe();
  }

  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
  }

  onClose() {
    this.raterApiData.confirmXpRaterUpdate$().pipe(
      takeUntil(this.destroy$),
      tap(() => this.toastr.info("Experience rater update confirmed!"))
    ).subscribe();
  }
}
