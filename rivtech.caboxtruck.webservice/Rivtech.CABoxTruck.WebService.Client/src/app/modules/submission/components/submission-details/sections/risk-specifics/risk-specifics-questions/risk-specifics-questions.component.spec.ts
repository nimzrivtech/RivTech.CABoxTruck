import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { RiskSpecificsQuestionsComponent } from './risk-specifics-questions.component';

describe('RiskSpecificsQuestionsComponent', () => {
  let component: RiskSpecificsQuestionsComponent;
  let fixture: ComponentFixture<RiskSpecificsQuestionsComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ RiskSpecificsQuestionsComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(RiskSpecificsQuestionsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
