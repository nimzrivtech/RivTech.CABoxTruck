import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { CoverageQuestionsChartComponent } from './coverage-questions-chart.component';

describe('CoverageQuestionsChartComponent', () => {
  let component: CoverageQuestionsChartComponent;
  let fixture: ComponentFixture<CoverageQuestionsChartComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ CoverageQuestionsChartComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(CoverageQuestionsChartComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
