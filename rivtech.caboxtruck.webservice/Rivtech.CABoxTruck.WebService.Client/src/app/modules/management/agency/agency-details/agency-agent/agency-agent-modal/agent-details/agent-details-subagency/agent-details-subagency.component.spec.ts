import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { AgentDetailsSubagencyComponent } from './agent-details-subagency.component';

describe('AgentDetailsSubagencyComponent', () => {
  let component: AgentDetailsSubagencyComponent;
  let fixture: ComponentFixture<AgentDetailsSubagencyComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AgentDetailsSubagencyComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AgentDetailsSubagencyComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
