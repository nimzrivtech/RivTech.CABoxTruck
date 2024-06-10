import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { AgentDetailsProgramStateComponent } from './agent-details-program-state.component';

describe('AgentDetailsProgramStateComponent', () => {
  let component: AgentDetailsProgramStateComponent;
  let fixture: ComponentFixture<AgentDetailsProgramStateComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AgentDetailsProgramStateComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AgentDetailsProgramStateComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
