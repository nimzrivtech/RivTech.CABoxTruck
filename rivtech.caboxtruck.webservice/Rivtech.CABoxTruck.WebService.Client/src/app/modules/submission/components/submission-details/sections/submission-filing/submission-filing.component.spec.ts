import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { SubmissionFilingComponent } from './submission-filing.component';

describe('SubmissionFilingComponent', () => {
  let component: SubmissionFilingComponent;
  let fixture: ComponentFixture<SubmissionFilingComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ SubmissionFilingComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(SubmissionFilingComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
