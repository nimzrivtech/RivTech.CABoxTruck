import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { AgencyFlatCommissionsComponent } from './agency-flat-commissions.component';

describe('AgencyFlatCommissionsComponent', () => {
  let component: AgencyFlatCommissionsComponent;
  let fixture: ComponentFixture<AgencyFlatCommissionsComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AgencyFlatCommissionsComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AgencyFlatCommissionsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
