import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { DriverInformationChartComponent } from './driver-information-chart.component';

describe('DriverInformationChartComponent', () => {
  let component: DriverInformationChartComponent;
  let fixture: ComponentFixture<DriverInformationChartComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ DriverInformationChartComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(DriverInformationChartComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
