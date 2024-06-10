import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { AgencyScheduleComponent } from './agency-schedule.component';

describe('AgencyScheduleComponent', () => {
  let component: AgencyScheduleComponent;
  let fixture: ComponentFixture<AgencyScheduleComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AgencyScheduleComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AgencyScheduleComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
