import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { IssueEndorsementModalComponent } from './issue-endorsement-modal.component';

describe('IssueEndorsementModalComponent', () => {
  let component: IssueEndorsementModalComponent;
  let fixture: ComponentFixture<IssueEndorsementModalComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ IssueEndorsementModalComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(IssueEndorsementModalComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
