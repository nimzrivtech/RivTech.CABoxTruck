import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { AgencySubagencyComponent } from './agency-subagency.component';

describe('AgencySubagencyComponent', () => {
  let component: AgencySubagencyComponent;
  let fixture: ComponentFixture<AgencySubagencyComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AgencySubagencyComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AgencySubagencyComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
