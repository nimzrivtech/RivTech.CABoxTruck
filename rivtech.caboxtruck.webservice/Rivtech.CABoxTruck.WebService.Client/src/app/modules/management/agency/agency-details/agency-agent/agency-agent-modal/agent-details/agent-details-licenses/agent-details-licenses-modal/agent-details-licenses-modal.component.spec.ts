import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { AgentDetailsLicensesModalComponent } from './agent-details-licenses-modal.component';

describe('AgentDetailsLicensesModalComponent', () => {
  let component: AgentDetailsLicensesModalComponent;
  let fixture: ComponentFixture<AgentDetailsLicensesModalComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AgentDetailsLicensesModalComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AgentDetailsLicensesModalComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
