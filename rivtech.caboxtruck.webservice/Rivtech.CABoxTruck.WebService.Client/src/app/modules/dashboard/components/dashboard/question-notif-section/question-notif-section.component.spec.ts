import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { QuestionNotifSectionComponent } from './question-notif-section.component';

describe('QuestionNotifSectionComponent', () => {
  let component: QuestionNotifSectionComponent;
  let fixture: ComponentFixture<QuestionNotifSectionComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ QuestionNotifSectionComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(QuestionNotifSectionComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
