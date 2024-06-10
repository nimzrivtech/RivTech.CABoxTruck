import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { SubmissionDocumentsComponent } from './submission-documents.component';

describe('SubmissionDocumentsComponent', () => {
  let component: SubmissionDocumentsComponent;
  let fixture: ComponentFixture<SubmissionDocumentsComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ SubmissionDocumentsComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(SubmissionDocumentsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
