import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { InstallmentScheduleComponent } from './installment-schedule.component';

describe('InstallmentScheduleComponent', () => {
  let component: InstallmentScheduleComponent;
  let fixture: ComponentFixture<InstallmentScheduleComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ InstallmentScheduleComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(InstallmentScheduleComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
