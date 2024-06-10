import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { AgencyAddressComponent } from './agency-address.component';

describe('AgencyAddressComponent', () => {
  let component: AgencyAddressComponent;
  let fixture: ComponentFixture<AgencyAddressComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AgencyAddressComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AgencyAddressComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
