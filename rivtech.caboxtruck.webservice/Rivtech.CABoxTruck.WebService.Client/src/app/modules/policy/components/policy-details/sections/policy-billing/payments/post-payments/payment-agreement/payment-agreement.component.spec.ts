import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { PaymentAgreementComponent } from './payment-agreement.component';

describe('PaymentAgreementComponent', () => {
  let component: PaymentAgreementComponent;
  let fixture: ComponentFixture<PaymentAgreementComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ PaymentAgreementComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(PaymentAgreementComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
