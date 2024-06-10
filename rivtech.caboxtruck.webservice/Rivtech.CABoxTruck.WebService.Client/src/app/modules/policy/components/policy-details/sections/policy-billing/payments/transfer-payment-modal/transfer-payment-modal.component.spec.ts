import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { TransferPaymentModalComponent } from './transfer-payment-modal.component';

describe('TransferPaymentModalComponent', () => {
  let component: TransferPaymentModalComponent;
  let fixture: ComponentFixture<TransferPaymentModalComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ TransferPaymentModalComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(TransferPaymentModalComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
