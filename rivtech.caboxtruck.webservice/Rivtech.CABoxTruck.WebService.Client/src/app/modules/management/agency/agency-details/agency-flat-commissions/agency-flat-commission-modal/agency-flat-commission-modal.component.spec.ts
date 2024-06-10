import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { AgencyFlatCommissionModalComponent } from './agency-flat-commission-modal.component';

describe('AgencyFlatCommissionModalComponent', () => {
  let component: AgencyFlatCommissionModalComponent;
  let fixture: ComponentFixture<AgencyFlatCommissionModalComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AgencyFlatCommissionModalComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AgencyFlatCommissionModalComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
