import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { SubmissionActionBarComponent } from './submission-action-bar.component';

describe('SubmissionActionBarComponent', () => {
  let component: SubmissionActionBarComponent;
  let fixture: ComponentFixture<SubmissionActionBarComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ SubmissionActionBarComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(SubmissionActionBarComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
