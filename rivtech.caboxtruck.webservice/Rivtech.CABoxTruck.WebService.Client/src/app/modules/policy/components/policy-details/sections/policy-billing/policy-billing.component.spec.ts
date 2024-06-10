import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { PolicyBillingComponent } from './policy-billing.component';

describe('PolicyBillingComponent', () => {
  let component: PolicyBillingComponent;
  let fixture: ComponentFixture<PolicyBillingComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ PolicyBillingComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(PolicyBillingComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
