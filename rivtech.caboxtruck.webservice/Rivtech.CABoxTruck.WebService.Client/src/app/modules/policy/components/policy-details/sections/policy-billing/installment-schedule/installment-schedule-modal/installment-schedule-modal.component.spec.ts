import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { InstallmentScheduleModalComponent } from './installment-schedule-modal.component';

describe('InstallmentScheduleModalComponent', () => {
  let component: InstallmentScheduleModalComponent;
  let fixture: ComponentFixture<InstallmentScheduleModalComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ InstallmentScheduleModalComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(InstallmentScheduleModalComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
