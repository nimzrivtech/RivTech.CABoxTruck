import { AfterViewInit, Component, EventEmitter, Input, OnDestroy, OnInit, Output, ViewChild } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { NavigationEnd, Router } from '@angular/router';
import { PolicyIssuanceData } from '@app/modules/policy/data/policy-issuance.data';
import { DatePickerComponent } from '@app/shared/components/dynamic/date-picker/date-picker.component';
import { Subject, Subscription, timer } from 'rxjs';
import { takeUntil, tap } from 'rxjs/operators';

@Component({
  selector: 'app-effective-date-modal',
  templateUrl: './effective-date-modal.component.html',
  styleUrls: ['./effective-date-modal.component.scss']
})
export class EffectiveDateModalComponent implements OnInit, OnDestroy, AfterViewInit {

  private readonly destroy$ = new Subject();

  @Input() effectiveDate: Date;
  @Input() initialTimeout: number;

  @Output() close = new EventEmitter<Date>(null);

  @ViewChild(DatePickerComponent)
  public datePicker: DatePickerComponent;

  form: FormGroup;
  loadTimeout: number = 10;
  timeoutStopped: boolean = false;
  routeChanged: boolean = false;
  timerSub: Subscription;

  get progressPercent() {
    return (this.loadTimeout / this.initialTimeout) * 100;
  }

  get timerActive() {
    return this.initialTimeout > 0 && !this.timeoutStopped;
  }

  get infoText() { return "This will pop-up every first visit in Issuance with Pending Endorsement substatus."; }

  constructor(
    public policyIssuanceData: PolicyIssuanceData,
    private router: Router
  ) { }

  ngOnInit() {
    this.router.events.subscribe(val => {
      if (val instanceof NavigationEnd) {
        this.routeChanged = true;
        this.onClose();
      }
    });

    if (this.initialTimeout > 0) {
      this.loadTimeout = this.initialTimeout;
      this.timerSub = timer(0, 1000).pipe(
        tap(() => {
          this.loadTimeout -= 1;
          if (this.loadTimeout == 0) {
            this.onClose(this.effectiveDate);
          }
        }),
        takeUntil(this.destroy$)
      ).subscribe();
    }
  }

  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
  }

  ngAfterViewInit(): void {
    this.datePicker.value = {
      isRange: false, singleDate: {
        jsDate: new Date(this.effectiveDate)
      }
    };

    this.datePicker.registerOnTouched(() => {
      this.timeoutStopped = true;
    });

    this.datePicker.dateChange.subscribe(date => {
      if (date.jsDate == null) {
        this.datePicker.value = this.effectiveDate;
      } else {
        this.onClose(date.jsDate);
      }
    })
  }

  onClose(date?: Date) {
    if (this.routeChanged) {
      this.close.emit(null)
      return;
    }
    this.close.emit(date);
  }

  onDatepickerClicked() {
    this.timeoutStopped = true;
    this.timerSub?.unsubscribe();
  }

  onContinueClicked() {
    this.onClose(this.effectiveDate);
  }
}
