import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { AgencyProgramStateComponent } from './agency-program-state.component';

describe('AgencyProgramStateComponent', () => {
  let component: AgencyProgramStateComponent;
  let fixture: ComponentFixture<AgencyProgramStateComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AgencyProgramStateComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AgencyProgramStateComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
