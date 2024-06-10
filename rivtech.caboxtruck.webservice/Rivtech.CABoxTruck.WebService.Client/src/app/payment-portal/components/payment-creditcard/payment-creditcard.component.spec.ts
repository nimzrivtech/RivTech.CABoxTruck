import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { PaymentCreditcardComponent } from './payment-creditcard.component';

describe('PaymentCreditcardComponent', () => {
  let component: PaymentCreditcardComponent;
  let fixture: ComponentFixture<PaymentCreditcardComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ PaymentCreditcardComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(PaymentCreditcardComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
