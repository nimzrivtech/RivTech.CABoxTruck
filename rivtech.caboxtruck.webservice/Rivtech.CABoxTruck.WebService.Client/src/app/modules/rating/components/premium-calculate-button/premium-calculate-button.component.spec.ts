import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { PremiumCalculateButtonComponent } from './premium-calculate-button.component';

describe('PremiumCalculateButtonComponent', () => {
  let component: PremiumCalculateButtonComponent;
  let fixture: ComponentFixture<PremiumCalculateButtonComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ PremiumCalculateButtonComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(PremiumCalculateButtonComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
