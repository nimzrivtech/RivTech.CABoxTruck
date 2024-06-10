import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { PaymentEftComponent } from './payment-eft.component';

describe('PaymentEftComponent', () => {
  let component: PaymentEftComponent;
  let fixture: ComponentFixture<PaymentEftComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ PaymentEftComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(PaymentEftComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
