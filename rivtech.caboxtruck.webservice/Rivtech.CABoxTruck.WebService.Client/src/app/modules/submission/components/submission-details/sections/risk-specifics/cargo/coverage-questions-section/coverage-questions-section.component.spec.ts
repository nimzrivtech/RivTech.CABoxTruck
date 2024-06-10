import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { CoverageQuestionsSectionComponent } from './coverage-questions-section.component';

describe('CoverageQuestionsSectionComponent', () => {
  let component: CoverageQuestionsSectionComponent;
  let fixture: ComponentFixture<CoverageQuestionsSectionComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ CoverageQuestionsSectionComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(CoverageQuestionsSectionComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
