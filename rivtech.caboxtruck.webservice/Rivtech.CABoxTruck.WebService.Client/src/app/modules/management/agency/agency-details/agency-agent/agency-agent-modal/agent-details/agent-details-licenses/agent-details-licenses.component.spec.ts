import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { AgentDetailsLicensesComponent } from './agent-details-licenses.component';

describe('AgentDetailsLicensesComponent', () => {
  let component: AgentDetailsLicensesComponent;
  let fixture: ComponentFixture<AgentDetailsLicensesComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AgentDetailsLicensesComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AgentDetailsLicensesComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
