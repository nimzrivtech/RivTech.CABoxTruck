import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ReportsSectionComponent } from './reports-section.component';

describe('ReportsSectionComponent', () => {
  let component: ReportsSectionComponent;
  let fixture: ComponentFixture<ReportsSectionComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ReportsSectionComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ReportsSectionComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
