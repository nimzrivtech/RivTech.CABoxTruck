import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { AgencyAddressModalComponent } from './agency-address-modal.component';

describe('AgencyAddressModalComponent', () => {
  let component: AgencyAddressModalComponent;
  let fixture: ComponentFixture<AgencyAddressModalComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AgencyAddressModalComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AgencyAddressModalComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
