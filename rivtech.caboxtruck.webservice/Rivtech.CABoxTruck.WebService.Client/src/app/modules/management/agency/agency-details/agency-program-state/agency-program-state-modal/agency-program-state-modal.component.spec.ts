import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { AgencyProgramStateModalComponent } from './agency-program-state-modal.component';

describe('AgencyProgramStateModalComponent', () => {
  let component: AgencyProgramStateModalComponent;
  let fixture: ComponentFixture<AgencyProgramStateModalComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AgencyProgramStateModalComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AgencyProgramStateModalComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
