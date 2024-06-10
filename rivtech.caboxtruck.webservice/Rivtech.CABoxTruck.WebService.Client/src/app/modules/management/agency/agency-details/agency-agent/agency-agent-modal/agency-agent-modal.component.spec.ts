import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { AgencyAgentModalComponent } from './agency-agent-modal.component';

describe('AgencyAgentModalComponent', () => {
  let component: AgencyAgentModalComponent;
  let fixture: ComponentFixture<AgencyAgentModalComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AgencyAgentModalComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AgencyAgentModalComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
