import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { SubmissionTasksComponent } from './submission-tasks.component';

describe('SubmissionTasksComponent', () => {
  let component: SubmissionTasksComponent;
  let fixture: ComponentFixture<SubmissionTasksComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ SubmissionTasksComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(SubmissionTasksComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
